using System.Security.Claims;

namespace RestaurantAPI_n.Interfaces
{
    public interface IUserContextService
    {
        ClaimsPrincipal User { get; }
        int? GetUserId { get; }
    }
}
