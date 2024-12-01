using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/")]
public class RootController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Welcome to the EraZor API! Go to /swagger for API documentation.");
    }
}
