namespace ChatBot.Domain.Entities
{
    public enum SubscriptionPlan
    {
        Public = 0,
        Bronze = 1,
        Silver = 2,
        Gold = 3,
        Vip = 4
    }

    public enum StoryType
    {
        Public = 0,
        Scheduled = 1,
        Exclusive = 2,
        Series = 3
    }

    public enum InteractionType
    {
        Download = 0,
        Like = 1,
        Reminder = 2
    }
}
