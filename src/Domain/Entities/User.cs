using System.Collections.Generic;

namespace ChatBot.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public SubscriptionPlan Plan { get; set; } = SubscriptionPlan.Public;
        public string? InviteCode { get; set; }
        public DateTime? DailyDownloadResetUtc { get; set; }
        public int DailyDownloadCount { get; set; }
        public ICollection<Story> Stories { get; set; } = new List<Story>();
        public ICollection<Interaction> Interactions { get; set; } = new List<Interaction>();
        public ICollection<StoryViewState> ViewedStories { get; set; } = new List<StoryViewState>();
    }
}
