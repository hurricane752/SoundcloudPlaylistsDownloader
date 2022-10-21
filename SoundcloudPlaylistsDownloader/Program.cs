using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SoundcloudPlaylistsDownloader.Extensions;
using SoundcloudPlaylistsDownloader.Services;
using SoundcloudPlaylistsDownloader.Validators;

var configuration = new ConfigurationBuilder().AddCommandLine(args).Build();
var services = new ServiceCollection().AddDependencies(configuration);

await using var serviceProvider = services.BuildServiceProvider();
await using var scope = serviceProvider.CreateAsyncScope();

ILogger logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
if(!logger.IsEnabled(LogLevel.Error))
{
    Console.WriteLine("LOGGER NOT ENABLED");
    return;
}

try
{
    var optionsValidator = scope.ServiceProvider.GetRequiredService<OptionsValidatorService>();
    var downloadService = scope.ServiceProvider.GetRequiredService<SoundcloudPlaylistsDownloaderService>();

    var validationResult = optionsValidator.ValidateOptions();
    if(validationResult != null)
    {
        logger.LogError(validationResult);
        return;
    }

    await downloadService.DownloadUserPlaylistsAsync();
}
catch(Exception ex)
{
    logger.LogError(ex.Message);
}