namespace ApiRefitConsumer.Utils.Interfaces
{
    public interface IAuthTokenStorage
    {
        string GetAuthToken();
        void SetAuthToken(string authToken);
    }
}
