using SwoopMarketplaceProjectFrontend.Services;
using System.Net.Http.Headers;

namespace SwoopMarketplaceProjectFrontend.Services
{
    public class JwtBearerHandler : DelegatingHandler
    {
        private readonly AuthSession _session;
        public JwtBearerHandler(AuthSession session) => _session = session;

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var token = _session.GetToken();
            if (!string.IsNullOrWhiteSpace(token))
            {
                // attach Authorization header
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // debugging header — shows up in browser devtools Request Headers
                // remove this header in production
                try
                {
                    request.Headers.Remove("X-Debug-Auth");
                }
                catch { }
                request.Headers.Add("X-Debug-Auth", "present");
            }
            else
            {
                // no token present — add debug header to indicate that
                request.Headers.Remove("X-Debug-Auth");
                request.Headers.Add("X-Debug-Auth", "missing");
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
