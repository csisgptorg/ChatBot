using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChatBot.Application.Dto;
using ChatBot.Application.Mapping;
using ChatBot.Application.Policies;
using ChatBot.Domain.Entities;
using ChatBot.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Application.Services
{
    public class StoryService
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public StoryService(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<FeedResponseDto> GetFeedAsync(Guid userId, Guid? categoryId, CancellationToken cancellationToken = default)
        {
            var user = await LoadUserAsync(userId, cancellationToken);
            var capabilities = SubscriptionPolicy.GetCapabilities(user.Plan);

            var query = _db.Set<Story>()
                .Include(s => s.Category)
                .Where(s => !s.IsDeleted);

            if (categoryId.HasValue)
            {
                query = query.Where(s => s.CategoryId == categoryId.Value);
                var category = await _db.Set<Category>().FirstOrDefaultAsync(c => c.Id == categoryId.Value, cancellationToken);
                if (category == null || !SubscriptionPolicy.CanEnterCategory(user.Plan, category))
                {
                    throw new InvalidOperationException("دسترسی به این دسته بندی مجاز نیست.");
                }
            }

            if (!capabilities.AllowExclusiveContent)
            {
                query = query.Where(s => !s.IsExclusiveContent);
            }

            var seen = _db.Set<StoryViewState>().Where(v => v.UserId == userId).Select(v => v.StoryId);
            query = query.Where(s => !seen.Contains(s.Id));

            var stories = await query.Take(capabilities.MaxStoriesPerDay).ToListAsync(cancellationToken);

            foreach (var story in stories)
            {
                _db.Add(new StoryViewState { StoryId = story.Id, UserId = userId });
            }

            await _db.SaveChangesAsync(cancellationToken);

            var dtoStories = _mapper.Map<IList<StoryDto>>(stories);
            for (var i = 0; i < dtoStories.Count; i++)
            {
                dtoStories[i].CategoryName = stories[i].Category?.Name;
            }

            return new FeedResponseDto
            {
                Stories = dtoStories,
                RemainingDailyDownloads = Math.Max(capabilities.DailyDownloadLimit - user.DailyDownloadCount, 0),
                RemainingLikes = capabilities.MaxLikesPerDay - CountInteractionsToday(user, InteractionType.Like)
            };
        }

        public async Task<StoryActionResultDto> DownloadStoryAsync(Guid userId, Guid storyId, CancellationToken cancellationToken = default)
        {
            var user = await LoadUserAsync(userId, cancellationToken);
            var capabilities = SubscriptionPolicy.GetCapabilities(user.Plan);
            ResetDownloadWindow(user);

            if (user.DailyDownloadCount >= capabilities.DailyDownloadLimit)
            {
                throw new InvalidOperationException("سقف دانلود روزانه تمام شده است.");
            }

            var story = await _db.Set<Story>().Include(s => s.Category).FirstAsync(s => s.Id == storyId, cancellationToken);
            if (story.Category != null && !SubscriptionPolicy.CanEnterCategory(user.Plan, story.Category))
            {
                throw new InvalidOperationException("پلن فعلی اجازه دسترسی به این دسته بندی را ندارد.");
            }

            user.DailyDownloadCount++;
            story.Downloads++;
            _db.Add(new Interaction { UserId = userId, StoryId = storyId, InteractionType = InteractionType.Download });
            await _db.SaveChangesAsync(cancellationToken);

            return new StoryActionResultDto
            {
                StoryId = storyId,
                Downloads = story.Downloads,
                Likes = story.Likes,
                ReminderCreated = false
            };
        }

        public async Task<StoryActionResultDto> LikeStoryAsync(Guid userId, Guid storyId, bool requestReminder, CancellationToken cancellationToken = default)
        {
            var user = await LoadUserAsync(userId, cancellationToken);
            var capabilities = SubscriptionPolicy.GetCapabilities(user.Plan);
            var likeCountToday = CountInteractionsToday(user, InteractionType.Like);
            if (likeCountToday >= capabilities.MaxLikesPerDay)
            {
                throw new InvalidOperationException("سقف لایک روزانه تمام شده است.");
            }

            var hasDownloaded = await _db.Set<Interaction>()
                .AnyAsync(i => i.UserId == userId && i.StoryId == storyId && i.InteractionType == InteractionType.Download, cancellationToken);

            if (!hasDownloaded)
            {
                throw new InvalidOperationException("برای لایک ابتدا باید استوری را دانلود کرده باشید.");
            }

            var story = await _db.Set<Story>().FirstAsync(s => s.Id == storyId, cancellationToken);
            story.Likes++;
            _db.Add(new Interaction { UserId = userId, StoryId = storyId, InteractionType = InteractionType.Like });

            var reminderCreated = false;
            if (requestReminder)
            {
                _db.Add(new Interaction
                {
                    UserId = userId,
                    StoryId = storyId,
                    InteractionType = InteractionType.Reminder,
                    OccurredAtUtc = DateTime.UtcNow.AddHours(24)
                });
                reminderCreated = true;
            }

            await _db.SaveChangesAsync(cancellationToken);

            return new StoryActionResultDto
            {
                StoryId = storyId,
                Downloads = story.Downloads,
                Likes = story.Likes,
                ReminderCreated = reminderCreated
            };
        }

        public async Task<Category> RegisterCategoryClickAsync(Guid categoryId, CancellationToken cancellationToken = default)
        {
            var category = await _db.Set<Category>().FirstAsync(c => c.Id == categoryId, cancellationToken);
            category.Clicks++;
            await _db.SaveChangesAsync(cancellationToken);
            return category;
        }

        public async Task<ScheduledStory> ScheduleStoryAsync(Guid userId, ScheduleStoryRequest request, CancellationToken cancellationToken = default)
        {
            var user = await LoadUserAsync(userId, cancellationToken);
            var capabilities = SubscriptionPolicy.GetCapabilities(user.Plan);
            if (!capabilities.AllowScheduling)
            {
                throw new InvalidOperationException("پلن فعلی امکان زمان‌بندی ندارد.");
            }

            var story = await _db.Set<Story>().FirstAsync(s => s.Id == request.StoryId, cancellationToken);
            var scheduled = new ScheduledStory
            {
                StoryId = request.StoryId,
                RequestedByUserId = userId,
                ScheduledForUtc = request.ScheduledForUtc,
                Destination = request.Destination,
                FallbackDestination = request.FallbackDestination,
                Topic = request.Topic,
                Story = story
            };

            _db.Add(scheduled);
            await _db.SaveChangesAsync(cancellationToken);
            return scheduled;
        }

        private static void ResetDownloadWindow(User user)
        {
            var nowDate = DateTime.UtcNow.Date;
            if (user.DailyDownloadResetUtc == null || user.DailyDownloadResetUtc.Value.Date < nowDate)
            {
                user.DailyDownloadResetUtc = nowDate;
                user.DailyDownloadCount = 0;
            }
        }

        private static int CountInteractionsToday(User user, InteractionType type)
        {
            var today = DateTime.UtcNow.Date;
            return user.Interactions.Count(i => i.InteractionType == type && i.OccurredAtUtc.Date == today);
        }

        private async Task<User> LoadUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _db.Set<User>()
                .Include(u => u.Interactions)
                .Include(u => u.ViewedStories)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null)
            {
                throw new InvalidOperationException("کاربر پیدا نشد.");
            }

            return user;
        }
    }
}
