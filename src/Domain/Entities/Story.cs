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
        public ICollection<Interaction> Interactions { get; set; } = new List<Interaction>();
        public Guid OwnerId { get; set; }
        public User? Owner { get; set; }
        public int Downloads { get; set; }
        public int Likes { get; set; }
    }
}
