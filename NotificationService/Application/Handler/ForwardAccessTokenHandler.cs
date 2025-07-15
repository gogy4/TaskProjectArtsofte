namespace NotificationService.Application.Handler
{
    public class ForwardAccessTokenHandler(IHttpContextAccessor ctx) : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var bearer = ctx.HttpContext?
                .Request.Headers.Authorization.FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(bearer))
            {
                request.Headers.TryAddWithoutValidation("Authorization", bearer);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}