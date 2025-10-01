using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserProfileController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        throw new NotImplementedException();
    }
}