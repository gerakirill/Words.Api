namespace WordsApi.Infrastructure.Options
{
    public class AuthenticationOptions
    {
        public const string Section = "AuthenticationOptions";

        public string[] ValidAudiences { get; set; } = [];

        public string[] ValidIssuers { get; set; } = [];

        public IssuerSigningKey IssuerSigningKey { get; set; } = new();
    }

    public class IssuerSigningKey
    {
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
