using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChatBot.Application.Mapping;
using ChatBot.Domain.Entities;
using ChatBot.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Application.Stories.Queries.GetStories;

public class GetStoriesQueryHandler : IRequestHandler<GetStoriesQuery, IReadOnlyList<StoryDto>>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetStoriesQueryHandler(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<StoryDto>> Handle(GetStoriesQuery request, CancellationToken cancellationToken)
    {
        var take = Math.Clamp(request.MaxItems, 1, 25);

        var stories = await _dbContext.Query<Story>()
            .Include(s => s.Category)
            .Where(s => !s.IsDeleted)
            .OrderByDescending(s => s.CreatedAtUtc)
            .Take(take)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var dto = _mapper.Map<IList<StoryDto>>(stories);
        for (var i = 0; i < dto.Count; i++)
        {
            dto[i].CategoryName = stories[i].Category?.Name;
        }

        return dto.ToList();
    }
}
