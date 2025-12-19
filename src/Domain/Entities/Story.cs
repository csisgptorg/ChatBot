using System;
using System.Collections.Generic;

namespace ChatBot.Domain.Entities
{
    public class Story : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public StoryType StoryType { get; set; } = StoryType.Public;
        public Guid? CategoryId { get; set; }
        public Category? Category { get; set; }
        public string MediaUrl { get; set; } = string.Empty;
        public string? BackgroundUrl { get; set; }
        public string? AudioUrl { get; set; }
        public string? LogoUrl { get; set; }
        public string? TextTemplate { get; set; }
        public string? SeriesGroup { get; set; }
        public bool IsExclusiveContent { get; set; }
        public bool IsInteractive { get; set; }
        public DateTime? ScheduledAtUtc { get; set; }
        public bool AllowBranding { get; set; }
        public ICollection<Interaction> Interactions { get; set; } = new List<Interaction>();
        public Guid OwnerId { get; set; }
        public User? Owner { get; set; }
        public int Downloads { get; set; }
        public int Likes { get; set; }
    }
}
