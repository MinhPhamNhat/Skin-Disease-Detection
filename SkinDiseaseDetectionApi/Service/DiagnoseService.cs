using System;
using System.Text;
using Newtonsoft.Json;
using SkinDiseaseDetectionApi.Dto;

namespace SkinDiseaseDetectionApi.Service
{
    public class DiagnoseService : IDiagnoseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        public DiagnoseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("SkinDetectionClient");
        }

        public async Task<PredictionDto> DiagnoseSkinDisease(string modelType, PredictRequest request)
        {
            string jsonRequest = JsonConvert.SerializeObject(request);
            var data = new Dictionary<string, string>()
            {
                { "Image", request.Image }
            };
            using (HttpContent formContent = new FormUrlEncodedContent(data))
            {
                using (HttpResponseMessage response = await _httpClient.PostAsync($"/predict/{modelType}", formContent).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                    var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<PredictionDto>(responseBody);
                }
            }
        }

        public Task<byte[]> GenerateReport()
        {
            throw new NotImplementedException();
        }
    }
}