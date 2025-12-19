using System;

namespace ChatBot.Application.Common.Interfaces;

/// <summary>
/// Provides information about the current authenticated user.
/// </summary>
public interface ICurrentUserService
{
    Guid? UserId { get; }
}
