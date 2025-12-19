using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ChatBot.Application.Dto;
using ChatBot.Application.Mapping;
using ChatBot.Application.Services;
using ChatBot.Domain.Entities;
using ChatBot.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoriesController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly StoryService _storyService;

        public StoriesController(AppDbContext dbContext, IMapper mapper, StoryService storyService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _storyService = storyService;
        }

        [HttpGet("feed")]
        public async Task<ActionResult<FeedResponseDto>> GetFeed([FromQuery] Guid userId, [FromQuery] Guid? categoryId)
            => Ok(await _storyService.GetFeedAsync(userId, categoryId));

        [HttpPost]
        public async Task<ActionResult<StoryDto>> Create([FromBody] StoryDto request)
        {
            var story = _mapper.Map<Story>(request);
            story.OwnerId = Guid.Empty; // plug real user context

            _dbContext.Add(story);
            await _dbContext.SaveChangesAsync();

            var response = _mapper.Map<StoryDto>(story);
            return CreatedAtAction(nameof(GetById), new { id = story.Id }, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StoryDto>> GetById(Guid id)
        {
            var story = await _dbContext.Query<Story>()
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (story == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<StoryDto>(story);
            dto.CategoryName = story.Category?.Name;
            return Ok(dto);
        }

        [HttpPost("{id}/download")]
        public async Task<ActionResult<StoryActionResultDto>> Download(Guid id, [FromQuery] Guid userId)
            => Ok(await _storyService.DownloadStoryAsync(userId, id));

        [HttpPost("{id}/like")]
        public async Task<ActionResult<StoryActionResultDto>> Like(Guid id, [FromQuery] Guid userId, [FromQuery] bool remind = false)
            => Ok(await _storyService.LikeStoryAsync(userId, id, remind));

        [HttpPost("{id}/schedule")]
        public async Task<ActionResult<ScheduledStory>> Schedule(Guid id, [FromQuery] Guid userId, [FromBody] ScheduleStoryRequest request)
        {
            if (request.StoryId == Guid.Empty)
            {
                request.StoryId = id;
            }

            var scheduled = await _storyService.ScheduleStoryAsync(userId, request);
            return Ok(scheduled);
        }

        [HttpPost("categories/{categoryId}/click")]
        public async Task<ActionResult<Category>> RegisterCategoryClick(Guid categoryId)
            => Ok(await _storyService.RegisterCategoryClickAsync(categoryId));
    }
}
