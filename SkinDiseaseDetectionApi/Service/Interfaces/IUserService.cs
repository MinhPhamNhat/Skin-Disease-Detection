namespace SkinDiseaseDetectionApi.Service.Interfaces
{
    public interface IUserService
    {
        Task<string> AuthenticateUserAsync(string username, string password);
        Task<User> GetUserById(int id);
        Task<bool> RegisterUser(UserDto user);
        Task<User> UpdateUser(User user);
        Task<UserDetail> GetUserDetail(string userId);
    }
}