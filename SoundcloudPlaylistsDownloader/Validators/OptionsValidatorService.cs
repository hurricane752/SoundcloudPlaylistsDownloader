using Microsoft.Extensions.Options;
using SoundcloudPlaylistsDownloader.Options;

namespace SoundcloudPlaylistsDownloader.Validators;

public sealed class OptionsValidatorService
{
    private readonly AuthOptions _authOptions;
    private readonly DownloadOptions _downloadOptions;

    public OptionsValidatorService(IOptions<AuthOptions> authOptions, IOptions<DownloadOptions> downloadOptions)
    {
        _authOptions = authOptions.Value;
        _downloadOptions = downloadOptions.Value;
    }

    public string? ValidateOptions()
    {
        var validationResult = ValidateAuthOptions(_authOptions);
        if(validationResult != null)
        {
            return validationResult;
        }

        validationResult = ValidateDownloadOptions(_downloadOptions);
        if(validationResult != null)
        {
            return validationResult;
        }

        return null;
    }

    public string? ValidateAuthOptions(AuthOptions options)
    {
        if(string.IsNullOrWhiteSpace(options.Authorization))
        {
            return "authorization is empty";
        }

        if(string.IsNullOrWhiteSpace(options.ClientId))
        {
            return "client id is empty";
        }

        return null;
    }

    public string? ValidateDownloadOptions(DownloadOptions options)
    {
        if(string.IsNullOrWhiteSpace(options.DownloadPath))
        {
            return "download path is empty";
        }

        return null;
    }
}