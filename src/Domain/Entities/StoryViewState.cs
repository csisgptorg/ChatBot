using System;

namespace ChatBot.Domain.Entities
{
    public class StoryViewState : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid StoryId { get; set; }
        public DateTime SeenAtUtc { get; set; } = DateTime.UtcNow;
    }
}
