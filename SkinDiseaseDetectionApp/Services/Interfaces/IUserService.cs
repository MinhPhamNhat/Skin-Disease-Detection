
namespace SkinDiseaseDetectionApp.Services.Interfaces;

public interface IUserService
{
    Task<string> Login(string email, string password);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<bool> Register(UserDto user);
    Task SaveUserDetail(UserHistory user);
    Task<UserDetailDto> GetUserDetail();
    Task<string> GetUserId();
    Task<string> IsAuth();
}