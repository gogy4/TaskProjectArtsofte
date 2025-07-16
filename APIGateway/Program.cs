using System.Reflection;
using System.Security.Claims;
using System.Text;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Caching.Memory;
using Polly;
using Polly.Extensions.Http;
using Yarp.ReverseProxy.Configuration;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        Env.Load();

        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        ConfigureMiddleware(app);

        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();

        services.AddControllers(); 
        services.AddSingleton<IBlackListService, BlackListService>();

        services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            options.AddServer(new OpenApiServer
            {
                Url = Environment.GetEnvironmentVariable("TASK_API") ?? "https://localhost:5044",
                Description = "Task Service"
            });
            options.AddServer(new OpenApiServer
            {
                Url = Environment.GetEnvironmentVariable("AUTH_API") ?? "https://localhost:5240",
                Description = "Auth Service"
            });
            options.AddServer(new OpenApiServer
            {
                Url = Environment.GetEnvironmentVariable("NOTIFICATION_API") ?? "https://localhost:5073",
                Description = "Notification Service"
            });
        });

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        services.AddMemoryCache();

        services.AddHttpClient("withPolly")
            .AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 2,
                    durationOfBreak: TimeSpan.FromSeconds(30)));

        services.AddReverseProxy()
            .LoadFromMemory(
                routes: GetRoutes(),
                clusters: GetClusters());

        AddAuthentication(services, configuration);

        services.AddAuthorization();
    }

    private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")!)),
                    RoleClaimType = ClaimTypes.Role
                };
            });
    }

    private static void ConfigureMiddleware(WebApplication app)
    {
        app.UseMiddleware<JwtBlacklistMiddleware>();
        app.UseCors("AllowAll");

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(Environment.GetEnvironmentVariable("GATEWAY") + "/swagger/v1/swagger.json", "API Gateway");
                c.SwaggerEndpoint(Environment.GetEnvironmentVariable("TASK_API") + "/swagger/v1/swagger.json", "Task Service");
                c.SwaggerEndpoint(Environment.GetEnvironmentVariable("AUTH_API") + "/swagger/v1/swagger.json", "Auth Service");
                c.SwaggerEndpoint(Environment.GetEnvironmentVariable("NOTIFICATION_API") + "/swagger/v1/swagger.json", "Notification Service");
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.Use(async (context, next) =>
        {
            var cache = context.RequestServices.GetRequiredService<IMemoryCache>();

            if (context.Request.Method == HttpMethod.Get.Method)
            {
                var cacheKey = context.Request.Path + context.Request.QueryString;

                if (cache.TryGetValue(cacheKey, out string cachedResponse))
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(cachedResponse);
                    return;
                }

                var originalBody = context.Response.Body;
                using var newBody = new MemoryStream();
                context.Response.Body = newBody;

                await next();

                newBody.Seek(0, SeekOrigin.Begin);
                var responseText = await new StreamReader(newBody).ReadToEndAsync();
                cache.Set(cacheKey, responseText, TimeSpan.FromSeconds(30));

                newBody.Seek(0, SeekOrigin.Begin);
                await newBody.CopyToAsync(originalBody);
                context.Response.Body = originalBody;
            }
            else
            {
                await next();
            }
        });

        app.MapReverseProxy().RequireAuthorization();
    }

    private static RouteConfig[] GetRoutes() => new[]
    {
        new RouteConfig
        {
            RouteId = "taskRoute",
            ClusterId = "taskCluster",
            Match = new RouteMatch
            {
                Path = "/api/tasks/{**catch-all}"
            }
        },
        new RouteConfig
        {
            RouteId = "authRoute",
            ClusterId = "authCluster",
            Match = new RouteMatch
            {
                Path = "/api/auth/{**catch-all}"
            }
        },
        new RouteConfig
        {
            RouteId = "notificationRoute",
            ClusterId = "notificationCluster",
            Match = new RouteMatch
            {
                Path = "/api/notifications/{**catch-all}"
            }
        }
    };

    private static ClusterConfig[] GetClusters() => new[]
    {
        new ClusterConfig
        {
            ClusterId = "taskCluster",
            Destinations = new Dictionary<string, DestinationConfig>
            {
                { "taskService", new DestinationConfig { Address = Environment.GetEnvironmentVariable("TASK_API")} }
            },
            HttpClient = new HttpClientConfig
            {
                DangerousAcceptAnyServerCertificate = true
            }
        },
        new ClusterConfig
        {
            ClusterId = "authCluster",
            Destinations = new Dictionary<string, DestinationConfig>
            {
                { "authService", new DestinationConfig { Address = Environment.GetEnvironmentVariable("AUTH_API")} }
            },
            HttpClient = new HttpClientConfig
            {
                DangerousAcceptAnyServerCertificate = true
            }
        },
        new ClusterConfig
        {
            ClusterId = "notificationCluster",
            Destinations = new Dictionary<string, DestinationConfig>
            {
                { "notificationService", new DestinationConfig { Address = Environment.GetEnvironmentVariable("NOTIFICATION_API")} }
            },
            HttpClient = new HttpClientConfig
            {
                DangerousAcceptAnyServerCertificate = true
            }
        }
    };
}
