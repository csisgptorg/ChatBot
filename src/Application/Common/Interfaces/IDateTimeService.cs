using System;

namespace ChatBot.Application.Common.Interfaces;

/// <summary>
/// Exposes the current system time to the application layer.
/// </summary>
public interface IDateTimeService
{
    DateTime UtcNow { get; }
}
