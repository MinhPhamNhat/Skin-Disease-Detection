using System.Threading.Tasks;

namespace SkinDiseaseDetectionApp.HttpClients.Interfaces
{
    public interface ISkinDetectionClient
    {
        Task<TResponse> Get<TResponse>(string endpoint);
        Task<TResponse> Post<TResponse>(string endpoint, Dictionary<string, string> request);
        Task<TResponse> Put<TRequest, TResponse>(string endpoint, TRequest request);
        Task Delete(string endpoint);
    }
}