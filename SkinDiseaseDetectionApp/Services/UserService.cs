using SkinDiseaseDetectionApp.HttpClients.Interfaces;
using SkinDiseaseDetectionApp.Services.Interfaces;

public class UserService : IUserService
{
    private readonly ISkinDetectionClient _skinDetectionClient;
    public UserService(ISkinDetectionClient skinDetectionClient)
    {
        _skinDetectionClient = skinDetectionClient;
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

    public Task<UserDetailDto> GetUserDetail(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<string> IsAuth()
    {
        throw new NotImplementedException();
    }

    public Task UpdateUserAsync(UserDto user)
    {
        throw new NotImplementedException();
    }
}