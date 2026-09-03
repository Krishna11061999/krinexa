using Krinexa.Application.DTOs.Auth;
using Krinexa.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Krinexa.Api.Controllers;

// [ADDED 2026-09-03] Auth endpoints — OTP send/verify, register, login
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    public AuthController(IAuthService auth) => _auth = auth;

    /// <summary>Send a 6-digit OTP to the given email address.</summary>
    [HttpPost("send-otp")]
    public async Task<IActionResult> SendOtp([FromBody] SendOtpRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Email))
            return BadRequest(new { message = "Email is required." });

        var result = await _auth.SendOtpAsync(req.Email);
        return Ok(result);
    }

    /// <summary>Register a new talent (student / intern / junior / senior).</summary>
    [HttpPost("register/talent")]
    public async Task<IActionResult> RegisterTalent([FromBody] RegisterTalentRequest req)
    {
        var result = await _auth.RegisterTalentAsync(req);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>Register a new client / interviewer organization.</summary>
    [HttpPost("register/client")]
    public async Task<IActionResult> RegisterClient([FromBody] RegisterClientRequest req)
    {
        var result = await _auth.RegisterClientAsync(req);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>Login with email + password. Returns JWT token.</summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        var result = await _auth.LoginAsync(req);
        return result.Success ? Ok(result) : Unauthorized(result);
    }
}
