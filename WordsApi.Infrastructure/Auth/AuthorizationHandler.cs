using System.Security.Claims;
using WordsApi.Infrastructure.Auth.PolicyHandlers;

namespace WordsApi.Infrastructure.Auth;

public static class AuthorizationHandler
{
    public static bool Handle(AuthorizeAttribute attribute, ClaimsPrincipal user) =>
        RolePolicyHandler.Handle(attribute.Role, user) && ScopePolicyHandler.Handle(attribute.Scope, user);
}