using MediatR;

namespace ChatBot.Application.Features.Queries.GetPlatformFeatures;

public record GetPlatformFeaturesQuery : IRequest<PlatformFeaturesDto>;
