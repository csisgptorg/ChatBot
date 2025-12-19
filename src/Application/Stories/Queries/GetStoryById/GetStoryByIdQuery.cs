using System;
using ChatBot.Application.Mapping;
using MediatR;

namespace ChatBot.Application.Stories.Queries.GetStoryById;

public record GetStoryByIdQuery(Guid Id) : IRequest<StoryDto?>;
