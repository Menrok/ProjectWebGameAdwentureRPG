using Backend.DTOs.Auth;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _auth;

    public AuthController(AuthService auth)
    {
        _auth = auth;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest("Niepoprawne dane rejestracji");

        var success = await _auth.Register(request.Username, request.Password);
        if (!success)
            return BadRequest("Użytkownik już istnieje");

        return Ok(new { message = "Zarejestrowano" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _auth.Login(request.Username, request.Password);
        if (user == null)
            return Unauthorized(new { message = "Niepoprawny login lub hasło" });

        var token = _auth.GenerateToken(user);
        return Ok(new { token });
    }
}