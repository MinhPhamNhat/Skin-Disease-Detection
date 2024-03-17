using SkinDiseaseDetectionApi.Service;

using SkinDiseaseDetectionApi.Service.Interfaces; // Add the missing using directive

public class UserService : IUserService
{
    private readonly JwtTokenManager _jwtTokenManager;
    private readonly MongoDBService _mongoDBService;

    public UserService(JwtTokenManager jwtTokenManager, MongoDBService mongoDBService)
    {
        _jwtTokenManager = jwtTokenManager;
        _mongoDBService = mongoDBService;
    }

    public async Task<string> AuthenticateUserAsync(string username, string password)
    {
        var user = await _mongoDBService.FirstOrDefaultAsync<User>(x => x.Email == username && x.Password == password);
        if (user != null)
        {
            return _jwtTokenManager.GenerateToken(user);
        }

        return null;
    }

    public async Task<bool> RegisterUser(UserDto userDto)
    {
        var user = new User()
        {
            Email = userDto.Email,
            Password = userDto.Password,
            Fullname = userDto.FullName,
            PhoneNumber = userDto.PhoneNumber
        };

        await _mongoDBService.CreateAsync<User>(user);
        return true;
    }

    public async Task<User> GetUserById(int id)
    {
        return await _mongoDBService.FindByIdAsync<User>(id);
    }

    public async Task<User> UpdateUser(User user)
    {
        await _mongoDBService.UpdateAsync<User>(user.Id, user);
        return user;
    }

}