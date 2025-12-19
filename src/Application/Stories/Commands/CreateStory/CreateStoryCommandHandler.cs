using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChatBot.Application.Mapping;
using ChatBot.Domain.Entities;
using ChatBot.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Application.Stories.Commands.CreateStory;

public class CreateStoryCommandHandler : IRequestHandler<CreateStoryCommand, StoryDto>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateStoryCommandHandler(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<StoryDto> Handle(CreateStoryCommand request, CancellationToken cancellationToken)
    {
        Category? category = null;
        if (request.CategoryId.HasValue)
        {
            category = await _dbContext.Set<Category>()
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.CategoryId.Value, cancellationToken);
        }

        var story = new Story
        {
            Title = request.Title,
            StoryType = request.StoryType,
            MediaUrl = request.MediaUrl,
            CategoryId = request.CategoryId,
            OwnerId = request.OwnerId
        };

        _dbContext.Add(story);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<StoryDto>(story);
        dto.CategoryName = category?.Name;
        return dto;
    }
}
