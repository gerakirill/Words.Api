using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Options;
using WordsApi.Infrastructure.Options;
using Microsoft.Extensions.Logging;

namespace WordsApi.Infrastructure.Auth;

public class AuthenticationHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AuthenticationOptions _authOptions;
    private readonly ILogger<AuthenticationHandlingMiddleware> _logger;

    public AuthenticationHandlingMiddleware(
        RequestDelegate next,
        IOptions<AuthenticationOptions> authOptions,
        ILogger<AuthenticationHandlingMiddleware> logger)
    {
        _next = next;
        _authOptions = authOptions.Value;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var jwt = context.Request.Headers.Authorization.ToString();
        var handler = new JwtSecurityTokenHandler();
        string root = Assembly.GetExecutingAssembly().Location;
        string path = Path.GetFullPath(Path.Combine(root, _authOptions.IssuerSigningKey.Path));
        var cert = new X509Certificate2(
            Path.Combine(path, _authOptions.IssuerSigningKey.Name),
            _authOptions.IssuerSigningKey.Password);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidAudiences = _authOptions.ValidAudiences,
            ValidIssuers = _authOptions.ValidIssuers,
            IssuerSigningKey = new X509SecurityKey(cert)
        };

        ClaimsPrincipal? principal;
        try
        {
            principal = handler.ValidateToken(jwt[7..], tokenValidationParameters, out _);
            
        }
        catch(Exception ex)
        {
            _logger.LogInformation("Unauthorized access attempt: {@AuthError}. Exception: {@Exception}", ex.Message, ex);
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }
        
        if (principal is not null)
        {
            context.User = principal;
            await _next(context);
        }
        else
        {
            _logger.LogInformation("Unauthorized access attempt");
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }
    }
}