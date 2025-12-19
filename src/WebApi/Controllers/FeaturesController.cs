using System.Threading.Tasks;
using ChatBot.Application.Features.Queries.GetPlatformFeatures;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace ChatBot.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeaturesController : ControllerBase
{
    private readonly IMediator _mediator;

    public FeaturesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PlatformFeaturesDto>> Get()
    {
        var response = await _mediator.Send(new GetPlatformFeaturesQuery());
        return Ok(response);
    }
}
