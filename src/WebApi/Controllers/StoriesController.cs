using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChatBot.Application.Mapping;
using ChatBot.Domain.Entities;
using ChatBot.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoriesController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public StoriesController(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StoryDto>>> Get()
        {
            var stories = await _dbContext.Query<Story>()
                .Include(x => x.Category)
                .AsNoTracking()
                .ToListAsync();

            var dto = _mapper.Map<IEnumerable<StoryDto>>(stories.Select(s => new StoryDto
            {
                Id = s.Id,
                Title = s.Title,
                StoryType = s.StoryType,
                MediaUrl = s.MediaUrl,
                CategoryName = s.Category?.Name
            }));

            return Ok(dto);
        }

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
    }
}
