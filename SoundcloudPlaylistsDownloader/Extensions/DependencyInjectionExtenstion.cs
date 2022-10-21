using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SoundcloudPlaylistsDownloader.HttpHandlers;
using SoundcloudPlaylistsDownloader.Options;
using SoundcloudPlaylistsDownloader.Services;
using SoundcloudPlaylistsDownloader.Validators;

namespace SoundcloudPlaylistsDownloader.Extensions;

public static class DependencyInjectionExtenstion
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions(configuration)
            .AddServices()
            .AddLogger();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .AddScoped<SoundcloudPlaylistsDownloaderService>()
            .AddScoped<SoundcloudApiService>()
            .AddScoped<AuthHttpMessageHandler>()
            .AddScoped<OptionsValidatorService>();

        services
            .AddHttpClient<SoundcloudApiService>()
            .ConfigureHttpClient(s => { s.BaseAddress = new Uri("https://api-v2.soundcloud.com"); })
            .AddHttpMessageHandler<AuthHttpMessageHandler>();

        return services;
    }

    public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthOptions>(s =>
        {
            s.Authorization = configuration.GetValue<string>("authorization");
            s.ClientId = configuration.GetValue<string>("client-id");
        });

        services.Configure<DownloadOptions>(s =>
        {
            s.DownloadPath = configuration.GetValue<string>("download-path");
            s.DownloadOnlyUserCreatedPlaylists =
                !configuration.GetChildren().Any(x => x.Key.Equals("download-only-user-playlists")) ||
                configuration.GetValue<bool>("download-only-user-playlists");
        });

        return services;
    }

    public static IServiceCollection AddLogger(this IServiceCollection services)
    {
        services.AddLogging(s =>
        {
            s.AddConsole();
            s.SetMinimumLevel(LogLevel.Information);
            s.AddFilter("System.Net.Http.HttpClient", LogLevel.None);
        });

        return services;
    }
}