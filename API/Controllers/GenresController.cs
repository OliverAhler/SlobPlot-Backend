using Application.Common.Interfaces.Handlers;
using Application.Features.Genres.Queries.GetGenres;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenresController(IDispatcher dispatcher) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetGenres(CancellationToken cancellationToken)
    {
        var query = new GetGenresQuery();
        
        var result = await dispatcher.Dispatch(query, cancellationToken);
        
        return result.IsSuccess 
            ? Ok(result.Value) 
            : BadRequest(result.Error);
    }
}