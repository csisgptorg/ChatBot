using ChatBot.Application.Common.Interfaces;

namespace ChatBot.Services;

/// <summary>
/// Provides the current UTC time, allowing the application layer to remain agnostic of system clocks.
/// </summary>
public class DateTimeService : IDateTimeService
{
    public DateTime UtcNow => DateTime.UtcNow;
}
