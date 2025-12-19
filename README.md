# ChatBot

نمونه اسکلت برای بک‌اند ASP.NET Core با کنترلر، کشف Reflection برای DbSetها و AutoMapper بدون پروفایل‌های جداگانه. این ساختار شامل لایه دامنه، برنامه، زیرساخت و API است و پایه‌ای برای اتصال کلاینت Flutter و لایه ربات تلگرام فراهم می‌کند.

## نکات کلیدی
- **کنترلرها**: در مسیر `src/API/Controllers` نمونه `StoriesController` قرار دارد و می‌توانید سایر کنترلرها را مشابه اضافه کنید.
- **DbSet با Reflection**: در `AppDbContext` همه Entityهایی که از `BaseEntity` ارث می‌برند به‌صورت خودکار در مدل EF Core ثبت و DbSet آن‌ها در دیکشنری داخلی آماده می‌شود.
- **BaseEntity**: تمام کلاس‌های دامنه از `BaseEntity` مشتق می‌شوند تا شناسه، زمان ایجاد/به‌روزرسانی و حذف منطقی را یکپارچه کنند.
- **AutoMapper بدون پروفایل**: از Attribute اختصاصی `AutoMapAttribute` برای تعریف نگاشت استفاده شده و پیکربندی در زمان اجرا با Reflection انجام می‌شود.

## اجرای API
پروژه نمونه با پایگاه‌داده InMemory تنظیم شده است. برای اجرای محلی:

```bash
cd src/API
dotnet run
```

سپس Swagger روی `https://localhost:5001/swagger` در دسترس خواهد بود. برای استفاده از SQL Server/PostgreSQL تنظیمات `AddDbContext` را در `Program.cs` و مقداردهی ConnectionString تغییر دهید.
