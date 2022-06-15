namespace ProxyApi.OuterApi
{
    public class InnerApiClient : IInnerApiClient
    {
        protected readonly HttpClient HttpClient;

        public InnerApiClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await HttpClient.GetAsync(url);
        }
    }

    public interface IInnerApiClient
    {
        Task<HttpResponseMessage> GetAsync(string url);
    }
}
