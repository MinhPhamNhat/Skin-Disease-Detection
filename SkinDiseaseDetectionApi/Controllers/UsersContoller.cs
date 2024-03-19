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

    [HttpPost("login")]
    public IActionResult Login(string username, string password)
    {
        var token = _userService.AuthenticateUserAsync(username, password);

        if (token == null)
            return Unauthorized();

        return Ok(token);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserDto user)
    {
        var result = await _userService.RegisterUser(user);

        if (!result)
            return BadRequest("Failed to register user.");

        return Ok("User registered successfully.");
    }

    [HttpGet("get_user_dedtails/{userId}")]
    public async Task<ActionResult<UserDetail>> GetUserDetail(string userId)
    {
        var result = await _userService.GetUserDetail(userId);
        return Ok(result);
    }
}