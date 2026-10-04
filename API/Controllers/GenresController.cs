using Application.Features.Genres.Queries.GetGenres;
using Microsoft.AspNetCore.Mvc;
using Vesia.Dispatch;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenresController(IDispatcher dispatcher) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetGenres(CancellationToken cancellationToken)
    {
        var query = new GetGenresQuery();
        
        var result = await dispatcher.DispatchAsync(query, cancellationToken);
        
        return result.IsSuccess 
            ? Ok(result.Value) 
            : BadRequest(result.Error);
    }
}