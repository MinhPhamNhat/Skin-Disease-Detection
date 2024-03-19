
namespace SkinDiseaseDetectionApp.Services.Interfaces;

public interface IUserService
{
    Task<string> Login(string email, string password);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<bool> Register(UserDto user);
    Task UpdateUserAsync(UserDto user);
    Task<UserDetailDto> GetUserDetail(string userId);
    Task<string> IsAuth();
}