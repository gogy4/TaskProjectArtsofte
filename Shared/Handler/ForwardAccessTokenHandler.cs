using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MyCompany.Shared.Http;

public class ForwardAccessTokenHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var bearer = httpContextAccessor.HttpContext?
            .Request.Headers["Authorization"].FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(bearer))
        {
            request.Headers.TryAddWithoutValidation("Authorization", bearer);
        }

        return base.SendAsync(request, cancellationToken);
    }
}