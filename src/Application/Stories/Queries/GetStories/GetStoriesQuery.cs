using System.Collections.Generic;
using ChatBot.Application.Mapping;
using MediatR;

namespace ChatBot.Application.Stories.Queries.GetStories;

public record GetStoriesQuery(int MaxItems = 25) : IRequest<IReadOnlyList<StoryDto>>;
