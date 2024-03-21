using SkinDiseaseDetectionApi.Service;

using SkinDiseaseDetectionApi.Service.Interfaces; // Add the missing using directive

public class UserService : IUserService
{
    private readonly MongoDBService _mongoDBService;

    public UserService(MongoDBService mongoDBService)
    {
        _mongoDBService = mongoDBService;
    }

    public async Task<User> AuthenticateUserAsync(string username, string password)
    {
        var user = await _mongoDBService.FirstOrDefaultAsync<User>(x => x.Email == username && x.Password == password);
        if (user != null)
        {
            return user;
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
        var userDetail = new UserDetail()
        {
            UserId = user.Id,
            Email = userDto.Email,
            Fullname = userDto.FullName,
            PhoneNumber = userDto.PhoneNumber
        };

        await _mongoDBService.CreateAsync<UserDetail>(userDetail);
        return true;
    }

    public async Task<User> GetUserById(int id)
    {
        return await _mongoDBService.FindByIdAsync<User>(id);
    }

    public async Task UpdateUserDetail(string userId, UserDetail user)
    {
        await _mongoDBService.UpdateAsync<UserDetail>(userId, user);
    }

    public Task<UserDetail> GetUserDetail(string userId)
    {
        return _mongoDBService.FirstOrDefaultAsync<UserDetail>(x => x.UserId == userId);
    }

    public async Task CreateUserHistory(string userId, UserHistory history)
    {
        var user = await GetUserDetail(userId);
        if (user == null)
        {
            await _mongoDBService.CreateAsync<UserDetail>(new UserDetail()
            {
                UserId = userId,
                Fullname = history.FullName,
                PhoneNumber = history.PhoneNumber,
                Email = history.Email,
                Address = history.Address,
                DateOfBirth = history.DateOfBirth.GetValueOrDefault(),
            });
        }
        await _mongoDBService.CreateAsync<UserHistory>(history);
    }

    public async Task<List<UserHistory>> GetUserHistories(string userId)
    {
        return await _mongoDBService.FindAsync<UserHistory>(x => x.UserId == userId);
    }
}