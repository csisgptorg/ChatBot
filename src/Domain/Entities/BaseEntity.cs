using System;

namespace ChatBot.Domain.Entities
{
    /// <summary>
    /// Common base entity that provides identity and auditing metadata.
    /// </summary>
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAtUtc { get; set; }
        public bool IsDeleted { get; set; }
    }
}
