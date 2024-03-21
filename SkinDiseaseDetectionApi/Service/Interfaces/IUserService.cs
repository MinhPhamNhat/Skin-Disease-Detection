namespace SkinDiseaseDetectionApi.Service.Interfaces
{
    public interface IUserService
    {
        Task<User> AuthenticateUserAsync(string username, string password);
        Task<User> GetUserById(int id);
        Task<bool> RegisterUser(UserDto user);
        Task UpdateUserDetail(string userId, UserDetail user);
        Task<UserDetail> GetUserDetail(string userId);
        Task CreateUserHistory(string userId, UserHistory history);
        Task<List<UserHistory>> GetUserHistories(string userId);
    }
}