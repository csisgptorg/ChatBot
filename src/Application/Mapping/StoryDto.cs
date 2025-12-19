using System;
using ChatBot.Domain.Entities;

namespace ChatBot.Application.Mapping
{
    [AutoMap(typeof(Story))]
    public class StoryDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public StoryType StoryType { get; set; }
        public Guid? CategoryId { get; set; }
        public string MediaUrl { get; set; } = string.Empty;
        public string? CategoryName { get; set; }
    }
}
