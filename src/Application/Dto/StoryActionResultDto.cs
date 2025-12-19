using System;
using ChatBot.Application.Mapping;
using ChatBot.Domain.Entities;

namespace ChatBot.Application.Dto
{
    [AutoMap(typeof(Story))]
    public class StoryActionResultDto
    {
        public Guid StoryId { get; set; }
        public int Downloads { get; set; }
        public int Likes { get; set; }
        public bool ReminderCreated { get; set; }
    }
}
