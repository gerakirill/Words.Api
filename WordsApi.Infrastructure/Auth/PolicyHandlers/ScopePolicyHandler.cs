using System.Security.Claims;

namespace WordsApi.Infrastructure.Auth.PolicyHandlers;

public static class ScopePolicyHandler
{
    public static bool Handle(string? scope, ClaimsPrincipal principal)
    {
        if (scope is null)
            return true;

        string? scopes = principal.Claims.FirstOrDefault(x => x.Type == "scope")?.Value;
        return scopes is not null && scopes.Split(" ").Contains(scope);
    }
}