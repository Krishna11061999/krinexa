using Microsoft.AspNetCore.Mvc;

namespace Krinexa.Api.Controllers;

// [ADDED 2026-09-03] Health check — required by deployment checklist
[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(new { status = "Healthy", timestamp = DateTime.UtcNow });
}
