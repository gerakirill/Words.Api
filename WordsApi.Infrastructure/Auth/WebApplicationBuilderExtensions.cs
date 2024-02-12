using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WordsApi.Infrastructure.Options;

namespace WordsApi.Infrastructure.Auth
{
    public static class WebApplicationBuilderExtensions
    {
        public static void AddAuthConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<AuthenticationOptions>(
                builder.Configuration.GetSection(AuthenticationOptions.Section));
        }
    }
}
