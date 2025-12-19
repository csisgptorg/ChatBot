using System;
using ChatBot.Domain.Entities;

namespace ChatBot.Application.Mapping
{
    [AutoMap(typeof(User))]
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public SubscriptionPlan Plan { get; set; }
    }
}
