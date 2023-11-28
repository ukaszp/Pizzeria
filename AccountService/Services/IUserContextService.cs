using System.Security.Claims;

namespace AccountApi.Services
{
    public interface IUserContextService
    {
        int? GetUserId { get; }
        ClaimsPrincipal? User { get; }
    }
}