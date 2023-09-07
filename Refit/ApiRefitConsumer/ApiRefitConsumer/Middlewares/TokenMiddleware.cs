using ApiRefitConsumer.Utils.Interfaces;
using System.Text;

namespace ApiRefitConsumer.Middlewares
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAuthTokenStorage _tokenStorage;
        public TokenMiddleware(RequestDelegate next, IAuthTokenStorage tokenStorage) {
            _next = next;
            _tokenStorage = tokenStorage;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string? jwt = context.Request.Headers.Authorization;
            if (jwt != null)
                {
                    StringBuilder token = new(jwt);
                    token.Remove(0, 7);
                    _tokenStorage.SetAuthToken(token.ToString());
                }
            
            await _next(context);
        }
    }
}
