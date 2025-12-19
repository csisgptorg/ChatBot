using System.Collections.Generic;

namespace ChatBot.Application.Features.Queries.GetPlatformFeatures;

public class PlatformFeaturesDto
{
    public IList<PlanDetailsDto> Plans { get; set; } = new List<PlanDetailsDto>();

    public IList<BotFeatureDto> BotFeatures { get; set; } = new List<BotFeatureDto>();
}

public class PlanDetailsDto
{
    public string Name { get; set; } = string.Empty;

    public IList<string> Benefits { get; set; } = new List<string>();
}

public class BotFeatureDto
{
    public int Id { get; set; }

    public string Description { get; set; } = string.Empty;
}
