using System.Collections.Generic;
using ChatBot.Domain.Entities;

namespace ChatBot.Application.Policies
{
    public record PlanCapabilities(
        int DailyDownloadLimit,
        bool AllowBranding,
        bool AllowScheduling,
        bool AllowExclusiveContent,
        int MaxStoriesPerDay,
        int MaxLikesPerDay);

    public static class SubscriptionPolicy
    {
        private static readonly IReadOnlyDictionary<SubscriptionPlan, PlanCapabilities> Capabilities =
            new Dictionary<SubscriptionPlan, PlanCapabilities>
            {
                [SubscriptionPlan.Public] = new(1, false, false, false, 1, 1),
                [SubscriptionPlan.Bronze] = new(4, false, false, false, 10, 10),
                [SubscriptionPlan.Silver] = new(6, true, false, false, 16, 16),
                [SubscriptionPlan.Gold] = new(10, true, true, true, 24, 24),
                [SubscriptionPlan.Vip] = new(15, true, true, true, 32, 32)
            };

        public static PlanCapabilities GetCapabilities(SubscriptionPlan plan) => Capabilities[plan];

        public static bool CanEnterCategory(SubscriptionPlan plan, Category category)
        {
            if (plan is SubscriptionPlan.Public && category.Name != "عمومی")
            {
                return false;
            }

            if (category.Name.Contains("اختصاصی") && plan < SubscriptionPlan.Vip)
            {
                return false;
            }

            return true;
        }
    }
}
