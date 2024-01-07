using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WebAppMVC.Data
{
    public class WebApiExecutor : IWebApiExecutor
    {
        private const string apiName = "ShirtsApi";
        private const string authApiName = "AuthorityApi";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WebApiExecutor(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<T?> InvokeGet<T>(string relativeUrl)
        {
            var httpClient = _httpClientFactory.CreateClient(apiName);
            await AddJwtToHeader(httpClient);
            var request = new HttpRequestMessage(HttpMethod.Get, relativeUrl);
            var response = await httpClient.SendAsync(request);
            await HandlePotentialError(response);

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T?> InvokePost<T>(string relativeUrl, T data)
        {
            var httpClient = _httpClientFactory.CreateClient(apiName);
            await AddJwtToHeader(httpClient);
            var response = await httpClient.PostAsJsonAsync(relativeUrl, data);
            
            await HandlePotentialError(response);

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task InvokePut<T>(string relativeUrl, T obj)
        {
            var httpClient = _httpClientFactory.CreateClient(apiName);
            await AddJwtToHeader(httpClient);
            var response = await httpClient.PutAsJsonAsync(relativeUrl, obj);

            await HandlePotentialError(response);
        }

        public async Task InvokeDelete(string relativeUrl)
        {
            var httpClient = _httpClientFactory.CreateClient(apiName);
            await AddJwtToHeader(httpClient);
            var response = await httpClient.DeleteAsync(relativeUrl);

            await HandlePotentialError(response);
        }

        private async Task HandlePotentialError(HttpResponseMessage httpResponse)
        {
            if(!httpResponse.IsSuccessStatusCode)
            {
                var errorJson = await httpResponse.Content.ReadAsStringAsync();
                throw new WebApiException(errorJson);
            }
        }

        private async Task AddJwtToHeader(HttpClient httpClient)
        {
            JwtToken? token = null;
            string? strToken = _httpContextAccessor.HttpContext?.Session.GetString("access_token");
            if (!string.IsNullOrWhiteSpace(strToken))
            {
                token = JsonConvert.DeserializeObject<JwtToken>(strToken);
            }

            if(token == null || token.ExpiresAt <= DateTime.UtcNow)
            {
                var clientId = _configuration.GetValue<string>("AppCredential:ClientId");
                var secret = _configuration.GetValue<string>("AppCredential:Secret");

                // Authenticate
                var authClient = _httpClientFactory.CreateClient(authApiName);
                var response = await authClient.PostAsJsonAsync("auth", new AppCredential
                {
                    ClientId = clientId,
                    Secret = secret
                });
                response.EnsureSuccessStatusCode();

                // Get the JWT
                strToken = await response.Content.ReadAsStringAsync();
                token = JsonConvert.DeserializeObject<JwtToken>(strToken);

                _httpContextAccessor.HttpContext?.Session.SetString("access_token", strToken);
            }            

            // Pass the JWT to endpoints thourgh the http headers
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.AccessToken);
        }
    }
}
