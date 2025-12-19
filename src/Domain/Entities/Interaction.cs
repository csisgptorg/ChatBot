using System;

namespace ChatBot.Domain.Entities
{
    public class Interaction : BaseEntity
    {
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public Guid StoryId { get; set; }
        public Story? Story { get; set; }
        public InteractionType InteractionType { get; set; }
        public DateTime OccurredAtUtc { get; set; } = DateTime.UtcNow;
    }
}
