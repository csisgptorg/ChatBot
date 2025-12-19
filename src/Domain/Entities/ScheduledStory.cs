using System;

namespace ChatBot.Domain.Entities
{
    public class ScheduledStory : BaseEntity
    {
        public Guid StoryId { get; set; }
        public Story? Story { get; set; }
        public Guid RequestedByUserId { get; set; }
        public DateTime ScheduledForUtc { get; set; }
        public string Destination { get; set; } = "direct"; // direct, bot, page
        public string? FallbackDestination { get; set; }
        public string? Topic { get; set; }
        public bool Completed { get; set; }
    }
}
