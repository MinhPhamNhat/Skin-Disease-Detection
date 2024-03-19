
using System.Text;
using Newtonsoft.Json;
using SkinDiseaseDetectionApp.HttpClients.Interfaces;

namespace SkinDiseaseDetectionApp.HttpClients;

public class SkinDetectionClient : ISkinDetectionClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private HttpClient _httpClient;

    public SkinDetectionClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient("SkinDetectionClient");
    }

    public async Task Delete(string endpoint)
    {

        HttpResponseMessage response = await _httpClient.DeleteAsync(endpoint);
        response.EnsureSuccessStatusCode();
    }

    public async Task<TResponse> Get<TResponse>(string endpoint)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        TResponse result = JsonConvert.DeserializeObject<TResponse>(responseBody);
        return result;
    }

    public async Task<TResponse> Post<TResponse>(string endpoint, Dictionary<string, string> request)
    {
        using (HttpContent formContent = new FormUrlEncodedContent(request))
        {
            using (HttpResponseMessage response = await _httpClient.PostAsync(endpoint, formContent).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<TResponse>(responseBody);
            }
        }
    }

    public async Task<TResponse> Put<TRequest, TResponse>(string endpoint, TRequest request)
    {
        string jsonRequest = JsonConvert.SerializeObject(request);
        HttpContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PutAsync(endpoint, content);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        TResponse result = JsonConvert.DeserializeObject<TResponse>(responseBody);
        return result;
    }
}