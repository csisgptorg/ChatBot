using System.Security.Claims;
using ChatBot.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ChatBot.Services;

/// <summary>
/// Retrieves user information from the current HTTP context.
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
        }
    }
}
