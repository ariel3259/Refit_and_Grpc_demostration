using ApiRefitConsumer.Utils.Interfaces;
using System.Net.Http.Headers;

namespace ApiRefitConsumer.Utils
{
    public class AuthDelegate: DelegatingHandler
    {
        private readonly IAuthTokenStorage _tokenStorage;
        public AuthDelegate(IAuthTokenStorage tokenStorage)
        {
            _tokenStorage = tokenStorage;
        }
        
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancelationToken)
        {
            string token = _tokenStorage.GetAuthToken();
            request.Headers.Add("Authorization", $"Bearer {token}");
            Console.WriteLine(request.Headers.Authorization);
            return await base.SendAsync(request, cancelationToken);
        }
    }
}
