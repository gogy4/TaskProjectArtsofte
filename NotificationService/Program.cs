using System.Reflection;
using System.Security.Claims;
using System.Text;
using DotNetEnv;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyCompany.Shared.Http;
using NotificationService.Application.Models;
using NotificationService.Application.Services.Abstractions;
using NotificationService.Application.Validators;
using NotificationService.Controllers;
using NotificationService.Hubs;
using NotificationService.Repository.Abstractions;
using NotificationService.Repository.Data;
using NotificationService.Repository.Implementations;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        Env.Load();

        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();
        ApplyMigrations(app);

        ConfigureMiddleware(app);

        app.MapControllers();
        app.MapHub<NotificationHub>("/notificationhub");

        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddSingleton<IDbConnectionFactory>(new NpgsqlConnectionFactory(connectionString));
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
        services.AddControllers();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme.",
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
        });

        services.AddCors(options =>
        {
            options.AddPolicy("AllowApiGateway", policy =>
            {
                policy.WithOrigins(Environment.GetEnvironmentVariable("GATEWAY"))
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        services.AddAuthorization();

        services.AddSignalR();
        services.AddHttpContextAccessor();
        services.AddTransient<ForwardAccessTokenHandler>();
        services
            .AddScoped<INotificationService,
                NotificationService.Application.Services.Implementations.NotificationService>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddHttpClient<CreateNotificationDtoValidator>(client =>
                client.BaseAddress = new Uri($"{Environment.GetEnvironmentVariable("AUTH_API")}/api/auth/"))
            .ConfigurePrimaryHttpMessageHandler(() =>
                new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                })
            .AddHttpMessageHandler<ForwardAccessTokenHandler>();

        services.AddHttpClient<MarkAsReadNotificationValidator>(client =>
                client.BaseAddress = new Uri($"{Environment.GetEnvironmentVariable("AUTH_API")}/api/auth/"))
            .ConfigurePrimaryHttpMessageHandler(() =>
                new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                })
            .AddHttpMessageHandler<ForwardAccessTokenHandler>();


        services.AddScoped<IValidator<CreateNotificationDto>>(sp =>
            sp.GetRequiredService<CreateNotificationDtoValidator>());
        services.AddScoped<IValidator<NotificationDto>>(sp =>
            sp.GetRequiredService<MarkAsReadNotificationValidator>());


        AddAuthentication(services, configuration);
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
                        Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")!)
                    ),
                    RoleClaimType = ClaimTypes.Role
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notificationhub"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });
    }

    private static void ApplyMigrations(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
    }

    private static void ConfigureMiddleware(WebApplication app)
    {
        app.UseCors("AllowApiGateway");

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();
    }
}