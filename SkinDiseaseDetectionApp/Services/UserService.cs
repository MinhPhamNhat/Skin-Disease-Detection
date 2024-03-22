using Microsoft.JSInterop;
using Newtonsoft.Json;
using SkinDiseaseDetectionApi.Models;
using SkinDiseaseDetectionApp.HttpClients.Interfaces;
using SkinDiseaseDetectionApp.Services.Interfaces;

public class UserService : IUserService
{
    private readonly ISkinDetectionClient _skinDetectionClient;
    private readonly IJSRuntime _jsRuntime;
    public UserService(ISkinDetectionClient skinDetectionClient, IJSRuntime jsRuntime)
    {
        _skinDetectionClient = skinDetectionClient;
        _jsRuntime = jsRuntime;
    }

    public async Task<bool> Register(UserDto user)
    {
        return await _skinDetectionClient.Post<bool>("/api/Users/register", new Dictionary<string, string>()
        {
            { "Email", user.Email },
            { "Password", user.Password }
        });
    }

    public Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<string> Login(string email, string password)
    {
        throw new NotImplementedException();
    }

    public async Task<UserDetailDto> GetUserDetail()
    {
        var userId = await GetUserId();
        if (userId == null) return null;
        return await _skinDetectionClient.Get<UserDetailDto>($"/api/Users/get_user_details/{userId}");
    }

    public Task<List<Doctor>> GetDoctors(){
        return _skinDetectionClient.Get<List<Doctor>>($"/Doctors");
    }
    public Task<string> IsAuth()
    {
        throw new NotImplementedException();
    }

    public async Task SaveUserDetail(UserHistory user)
    {
        var updatedser = await _skinDetectionClient.Post($"api/Users/create_history/{user.UserId}", ToDictionary(user));
    }

    public async Task<string> GetUserId()
    {
        return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "userId");
    }

    public static Dictionary<string, string> ToDictionary(object obj)
    {
        var json = JsonConvert.SerializeObject(obj);
        var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        return dictionary;
    }
}