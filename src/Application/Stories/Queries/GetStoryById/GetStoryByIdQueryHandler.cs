using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChatBot.Application.Mapping;
using ChatBot.Domain.Entities;
using ChatBot.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Application.Stories.Queries.GetStoryById;

public class GetStoryByIdQueryHandler : IRequestHandler<GetStoryByIdQuery, StoryDto?>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetStoryByIdQueryHandler(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<StoryDto?> Handle(GetStoryByIdQuery request, CancellationToken cancellationToken)
    {
        var story = await _dbContext.Query<Story>()
            .Include(s => s.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (story is null)
        {
            return null;
        }

        var dto = _mapper.Map<StoryDto>(story);
        dto.CategoryName = story.Category?.Name;
        return dto;
    }
}
