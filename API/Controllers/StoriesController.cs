using Application.Common.Interfaces;
using Application.Common.Interfaces.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StoriesController(IDispatcher dispatcher) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetStories()
    {
        throw new NotImplementedException();
    }
}



//

// public async Task<IActionResult> Get(CancellationToken cancellationToken)
// {
//     var query = new GetUserByIdPSubQuery(currentUserService.GetUserId());
//         
//     var result = await dispatcher.Dispatch(query, cancellationToken);
//         
//     return result.IsSuccess 
//         ? Ok(result.Value) 
//         : NotFound(result.Error);
// }