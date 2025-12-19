using System;
using ChatBot.Domain.Entities;

namespace ChatBot.Application.Dto
{
    public class ScheduleStoryRequest
    {
        public Guid StoryId { get; set; }
        public DateTime ScheduledForUtc { get; set; }
        public string Destination { get; set; } = "direct";
        public string? FallbackDestination { get; set; } = "bot";
        public string? Topic { get; set; }
    }
}
