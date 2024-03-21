using Microsoft.AspNetCore.Mvc;
using SkinDiseaseDetectionApi.Service.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("login")]
    public async Task<IActionResult> Login(string username, string password)
    {
        var user = await _userService.AuthenticateUserAsync(username, password);

        if (user == null)
            return Unauthorized();

        return Ok(user);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] UserDto user)
    {
        var result = await _userService.RegisterUser(user);

        if (!result)
            return BadRequest("Failed to register user.");

        return Ok("User registered successfully.");
    }

    [HttpGet("update_user/{userId}")]
    public async Task<ActionResult<UserDetail>> UpdateUserDetail(string userId, UserDetail userDetail)
    {
        await _userService.UpdateUserDetail(userId, userDetail);
        return Ok();
    }

    [HttpGet("get_user_details/{userId}")]
    public async Task<ActionResult<UserDetail>> GetUserDetail(string userId)
    {
        var result = await _userService.GetUserDetail(userId);
        return Ok(result);
    }

    [HttpPost("create_history/{userId}")]
    public async Task<ActionResult<UserDetail>> CreateHistory(string userId, [FromForm] UserHistory userHistory)
    {
        await _userService.CreateUserHistory(userHistory.UserId, userHistory);
        return Ok("History created successfully.");
    }

    [HttpGet("get_user_history/{userId}")]
    public async Task<ActionResult<UserDetail>> GetUserHistories(string userId)
    {
        var result = await _userService.GetUserHistories(userId);
        return Ok(result);
    }
}