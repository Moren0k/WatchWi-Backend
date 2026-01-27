using Microsoft.AspNetCore.Mvc;
using Watchwi.Application.Services.AuthService;

namespace Watchwi.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
    {
        var result = await _authService.LoginAsync(loginRequest);

        if (result == null)
            return Unauthorized("Invalid credentials");

        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequest)
    {
        var result = await _authService.RegisterAsync(registerRequest);

        if (result == null)
            return BadRequest("User already exists");

        return Ok(result);
    }
}