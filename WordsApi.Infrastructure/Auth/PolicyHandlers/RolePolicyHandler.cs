using System.Security.Claims;

namespace WordsApi.Infrastructure.Auth.PolicyHandlers;

public static class RolePolicyHandler
{
    public static bool Handle(string? role, ClaimsPrincipal principal)
    {
        return role is null || principal.IsInRole(role);
    }
}