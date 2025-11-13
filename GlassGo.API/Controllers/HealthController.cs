using Microsoft.AspNetCore.Mvc;

namespace GlassGo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok("API is running");
}
