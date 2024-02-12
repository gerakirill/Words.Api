using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using WordsApi.Application.WordsChecker;
using WordsApi.Infrastructure.Dictionaries;
using WordsApi.Infrastructure.Options;

namespace WordsApi.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDictionaries(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<DictionaryResourcesOptions>(
            builder.Configuration.GetSection(DictionaryResourcesOptions.Section));
        builder.Services.AddSingleton<IWordsChecker, WordsChecker>();
    }

    public static void UseLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));
    }
}