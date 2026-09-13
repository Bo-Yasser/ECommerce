using ECommerce.UseCases.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ECommerce.Infrastructure.Identity;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public bool IsAuthenticated => httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    public Guid? UserId
    {
        get
        {
            var value = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(value, out var id) ? id : null;
        }
    }

    public string? Email =>
        httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email)
        ?? httpContextAccessor.HttpContext?.User.FindFirstValue("email");

    public Guid? GuestId
    {
        get
        {
            // Guest Mobile Request
            var headerValue = httpContextAccessor.HttpContext?.Request.Headers["X-Buyer-Id"].FirstOrDefault();
            if (Guid.TryParse(headerValue, out var guestHeaderId) && guestHeaderId != Guid.Empty)
                return guestHeaderId;

            // Guest Web Request
            var cookieValue = httpContextAccessor.HttpContext?.Request.Cookies["guest-session"];
            if (Guid.TryParse(cookieValue, out var guestCookieId) && guestCookieId != Guid.Empty)
                return guestCookieId;

            return null;
        }
    }

    public Guid? BuyerId => UserId ?? GuestId;

}
