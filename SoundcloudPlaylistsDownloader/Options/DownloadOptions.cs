namespace SoundcloudPlaylistsDownloader.Options;

public sealed class DownloadOptions
{
    public string? DownloadPath { get; set; }

    public bool DownloadOnlyUserCreatedPlaylists { get; set; } = true;
}