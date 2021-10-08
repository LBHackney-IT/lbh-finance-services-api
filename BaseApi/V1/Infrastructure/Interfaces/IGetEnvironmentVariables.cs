namespace BaseApi.V1.Infrastructure.Interfaces
{
    public interface IGetEnvironmentVariables
    {
        public string GetAccountApiUrl();
        public string GetAccountApiToken();
        public string GetTransactionApiUrl();
        public string GetTransactionApiKey();
    }
}
