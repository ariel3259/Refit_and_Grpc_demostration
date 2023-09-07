using ApiRefitConsumer.Utils.Interfaces;

namespace ApiRefitConsumer.Utils
{
    public class AuthTokenStorage : IAuthTokenStorage
    {
        private string token;

        public AuthTokenStorage()
        {
            token = "";
        }
        public string GetAuthToken()
        {
            return token;
        }

        public void SetAuthToken(string authToken)
        {
            token = authToken;
        }
    }
}
