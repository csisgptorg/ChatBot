using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatBot.Application.Mapping;
using ChatBot.Application.Stories.Commands.CreateStory;
using ChatBot.Application.Stories.Queries.GetStories;
using ChatBot.Application.Stories.Queries.GetStoryById;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace ChatBot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StoryDto>>> Get([FromQuery] int maxItems = 25)
        {
            var stories = await _mediator.Send(new GetStoriesQuery(maxItems));
            return Ok(stories);
        }

        [HttpPost]
        public async Task<ActionResult<StoryDto>> Create([FromBody] StoryDto request)
        {
            var command = new CreateStoryCommand(
                request.Title,
                request.StoryType,
                request.MediaUrl,
                request.CategoryId,
                Guid.Empty);

            var response = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StoryDto>> GetById(Guid id)
        {
            var story = await _mediator.Send(new GetStoryByIdQuery(id));

            if (story == null)
            {
                return NotFound();
            }

            return Ok(story);
        }
    }
}
