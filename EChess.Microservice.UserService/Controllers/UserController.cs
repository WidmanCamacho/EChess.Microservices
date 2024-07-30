using EChess.Microservice.UserService.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public UserController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    [HttpGet]
    [Route("GetAllUsers")]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    [Route("CreateUser")]
    public async Task<ActionResult> AddUser([FromBody] User user)
    {
        await _userService.AddUserAsync(user);
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser([FromBody] User user)
    {
        return await _userService.UpdateUserAsync(user) ? Ok() : BadRequest();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        await _userService.DeleteUserAsync(id);
        return NoContent();
    }

    [HttpPost("authenticate")]
    public async Task<ActionResult<User>> Authenticate([FromBody] User login)
    {
        var token = await _userService.AuthenticateAsync(login.Username, login.PasswordHash);

        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized();
        }
        return Ok(token);
    }
}
