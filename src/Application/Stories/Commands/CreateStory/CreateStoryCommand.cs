using System;
using ChatBot.Application.Mapping;
using ChatBot.Domain.Entities;
using MediatR;

namespace ChatBot.Application.Stories.Commands.CreateStory;

public record CreateStoryCommand(
    string Title,
    StoryType StoryType,
    string MediaUrl,
    Guid? CategoryId,
    Guid OwnerId) : IRequest<StoryDto>;
