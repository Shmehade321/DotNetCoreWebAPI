namespace WebAppMVC.Data
{
    public class WebApiExecutor : IWebApiExecutor
    {
        private const string apiName = "ShirtsApi";
        private readonly IHttpClientFactory _httpClientFactory;

        public WebApiExecutor(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T?> InvokeGet<T>(string relativeUrl)
        {
            var httpClient = _httpClientFactory.CreateClient(apiName);
            return await httpClient.GetFromJsonAsync<T>(relativeUrl);
        }

        public async Task<T?> InvokePost<T>(string relativeUrl, T data)
        {
            var httpClient = _httpClientFactory.CreateClient(apiName);
            var response = await httpClient.PostAsJsonAsync(relativeUrl, data);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}
