using Application.Features.Statuses.Queries.GetStatuses;
using Microsoft.AspNetCore.Mvc;
using Vesia.Dispatch;

namespace API.Controllers;

[ApiController]
[Route("api/story-statuses")] 
public class StoryStatusesController(IDispatcher dispatcher) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetStatuses(CancellationToken cancellationToken)
    {
        var query = new GetStatusesQuery();
        
        var result = await dispatcher.DispatchAsync(query, cancellationToken);
        
        return result.IsSuccess 
            ? Ok(result.Value) 
            : BadRequest(result.Error);
    }
}