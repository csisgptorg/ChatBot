using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace ChatBot.Application.Features.Queries.GetPlatformFeatures;

public class GetPlatformFeaturesQueryHandler : IRequestHandler<GetPlatformFeaturesQuery, PlatformFeaturesDto>
{
    public Task<PlatformFeaturesDto> Handle(GetPlatformFeaturesQuery request, CancellationToken cancellationToken)
    {
        var response = new PlatformFeaturesDto
        {
            Plans = new List<PlanDetailsDto>
            {
                new()
                {
                    Name = "اشتراک عمومی",
                    Benefits = new List<string>
                    {
                        "امکان دیدن استوری‌های عمومی",
                        "دانلود یک استوری از محتوای عمومی بدون دسترسی به دسته‌بندی‌های ویژه"
                    }
                },
                new()
                {
                    Name = "اشتراک برنزی",
                    Benefits = new List<string>
                    {
                        "امکان استفاده از اکثر دسته‌بندی‌ها",
                        "دانلود چهار استوری در روز"
                    }
                },
                new()
                {
                    Name = "اشتراک نقره‌ای",
                    Benefits = new List<string>
                    {
                        "امکان استفاده از اکثر دسته‌بندی‌ها",
                        "دانلود شش استوری در روز",
                        "امکان اضافه کردن لوگو به استوری‌ها"
                    }
                },
                new()
                {
                    Name = "اشتراک طلایی",
                    Benefits = new List<string>
                    {
                        "امکان استفاده از تمامی دسته‌بندی‌ها",
                        "دانلود ده استوری در روز",
                        "امکان اضافه کردن لوگو به استوری‌ها",
                        "امکان ارسال استوری زمان‌بندی‌شده با موضوع دلخواه به دایرکت"
                    }
                },
                new()
                {
                    Name = "اشتراک VIP",
                    Benefits = new List<string>
                    {
                        "امکان استفاده از تمامی دسته‌بندی‌ها",
                        "دانلود ۱۵ استوری در روز",
                        "امکان اضافه کردن لوگو به استوری‌ها",
                        "امکان ارسال استوری زمان‌بندی‌شده با موضوع دلخواه به دایرکت",
                        "امکان داشتن محتوای اختصاصی"
                    }
                }
            },
            BotFeatures = new List<BotFeatureDto>
            {
                new() { Id = 1, Description = "گذاشتن لوگوی پیج با طرح دلخواه روی استوری‌ها" },
                new() { Id = 2, Description = "ارسال ریلز با دسته‌بندی انتخاب‌شده به خصوصی اعضا" },
                new() { Id = 3, Description = "ارسال هشدار یادآور استوری" },
                new() { Id = 4, Description = "امکان ارسال متن و زیبا‌سازی آن با قالب‌های مختلف برای عکس" },
                new() { Id = 5, Description = "امکان ارسال متن و میکس کردن با بک‌گراند تصویری یا ویدئو و موسیقی متن" },
                new() { Id = 6, Description = "امکان انتخاب استوری آماده با دسته‌بندی و زیرشاخه‌های گوناگون" },
                new() { Id = 7, Description = "امکان ارسال استوری انتخاب‌شده و بارگذاری مستقیم روی پیج در ساعت مشخص یا ارسال به خصوصی تلگرام" },
                new() { Id = 8, Description = "امکان بارگذاری استوری دلخواه مخاطب در ساعت مشخص‌شده" },
                new() { Id = 9, Description = "امکان دانلود استوری دیگران با ربات" },
                new() { Id = 10, Description = "انتخاب هوشمند استوری بر اساس موضوع پیج مخاطب" },
                new() { Id = 11, Description = "قرار دادن استوری‌های سریالی و پشت سر هم" },
                new() { Id = 12, Description = "هر کاربر لینک دعوت اختصاصی برای دعوت دیگران داشته باشد" },
                new() { Id = 13, Description = "نمایش استوری‌های عمومی بر اساس سلیقه و دانلودهای قبلی هر مخاطب" },
                new() { Id = 14, Description = "دسته‌بندی ایام هفته و پیشنهاد استوری مناسب هر روز" },
                new() { Id = 15, Description = "اضافه کردن دسته‌بندی مذهبی و زیرشاخه‌های آن" },
                new() { Id = 16, Description = "ایجاد دسته‌بندی استوری‌های مناسب زمان‌های خاص" },
                new() { Id = 17, Description = "ایجاد دسته‌بندی استوری‌های تعاملی (سؤال دوگزینه‌ای، اعترافی و …)" },
                new() { Id = 18, Description = "اضافه کردن دسته دکلمه و گویندگی" },
                new() { Id = 19, Description = "ایجاد دسته‌بندی ایران‌گردی" },
                new() { Id = 20, Description = "امکان گرفتن تبلیغ و گذاشتن پست دیگران" },
                new() { Id = 21, Description = "عدم نمایش استوری تکراری به یک مخاطب" },
                new() { Id = 22, Description = "نمایش تعداد استوری‌های آماده کنار هر دسته‌بندی" },
                new() { Id = 23, Description = "ثبت تعداد کلیک روی هر دسته‌بندی و زیرشاخه" },
                new() { Id = 24, Description = "امکان لایک کردن استوری و نمایش تعداد لایک برای مخاطبین" },
                new() { Id = 25, Description = "یادآوری بعد از ۲۴ یا ۴۸ ساعت برای لایک در صورت تعامل خوب و محدودیت لایک فقط برای دانلودکنندگان" }
            }
        };

        return Task.FromResult(response);
    }
}
