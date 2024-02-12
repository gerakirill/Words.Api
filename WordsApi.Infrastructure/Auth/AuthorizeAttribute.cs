namespace WordsApi.Infrastructure.Auth;

[AttributeUsage(AttributeTargets.Field)]
public class AuthorizeAttribute : Attribute
{
    public string? Role { get; set; }

    public string? Scope { get; set; }
}