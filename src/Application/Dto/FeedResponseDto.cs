using System.Collections.Generic;
using ChatBot.Application.Mapping;

namespace ChatBot.Application.Dto
{
    public class FeedResponseDto
    {
        public IList<StoryDto> Stories { get; set; } = new List<StoryDto>();
        public int RemainingDailyDownloads { get; set; }
        public int RemainingLikes { get; set; }
    }
}
