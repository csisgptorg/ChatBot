using System.Collections.Generic;

namespace ChatBot.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public int StoryCount { get; set; }
        public int Clicks { get; set; }
        public ICollection<Story> Stories { get; set; } = new List<Story>();
    }
}
