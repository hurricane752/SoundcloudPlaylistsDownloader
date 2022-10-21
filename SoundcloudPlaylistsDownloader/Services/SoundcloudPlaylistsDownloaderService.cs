using System.Globalization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SoundcloudPlaylistsDownloader.Models.PlaylistModels;
using SoundcloudPlaylistsDownloader.Models.TrackModels;
using SoundcloudPlaylistsDownloader.Options;

namespace SoundcloudPlaylistsDownloader.Services;

public sealed class SoundcloudPlaylistsDownloaderService
{
    private const int CountTracksPerPage = 30;

    private readonly HashSet<TrackFullModel> _notDownloadedTracks = new();

    private readonly SoundcloudApiService _apiService;
    private readonly DownloadOptions _downloadOptions;
    private readonly ILogger<SoundcloudPlaylistsDownloaderService> _logger;

    public SoundcloudPlaylistsDownloaderService(
        SoundcloudApiService apiService,
        IOptions<DownloadOptions> downloadOptions,
        ILogger<SoundcloudPlaylistsDownloaderService> logger)
    {
        _apiService = apiService;
        _logger = logger;
        _downloadOptions = downloadOptions.Value;
    }

    public async Task DownloadUserPlaylistsAsync()
    {
        var user = await _apiService.GetUserAsync();
        var userPlaylists = await _apiService.GetUserPlaylistsAsync();

        IReadOnlyCollection<PlaylistShortModel> playlists = userPlaylists.Collection.Select(s => s.Playlist).ToArray();
        if(_downloadOptions.DownloadOnlyUserCreatedPlaylists)
        {
            playlists = GetUserCreatedPlaylists(user.Id, playlists);
        }

        foreach(var userCreatedPlaylist in playlists)
        {
            await DownloadPlaylistAsync(userCreatedPlaylist.Id);
        }

        await GenerateNotDownloadedTracksReport();
    }

    private Task GenerateNotDownloadedTracksReport()
    {
        var reportItems = new HashSet<string>();
        foreach(var notDownloadedTrack in _notDownloadedTracks)
        {
            var trackName = GetTrackName(notDownloadedTrack);
            reportItems.Add($"{trackName} | {notDownloadedTrack.PermalinkUrl} | {notDownloadedTrack.Id}");
        }

        var fileName = GetValidFileName($"notDownloadedTracks_{DateTime.Now.ToString(CultureInfo.CurrentCulture)}.txt");

        return File.WriteAllLinesAsync(GetPath(fileName), reportItems);
    }

    private IReadOnlyCollection<PlaylistShortModel> GetUserCreatedPlaylists(
        ulong id, IReadOnlyCollection<PlaylistShortModel> playlists)
    {
        return playlists.Where(s => s.UserId == id).ToArray();
    }

    private async Task DownloadPlaylistAsync(ulong id)
    {
        var playlist = await _apiService.GetPlaylistAsync(id);
        var playlistTitle = playlist.Title;
        var playlistTrackIds = playlist.Tracks.Select(s => s.Id).ToArray();

        CreatePlaylistDirectory(playlistTitle);

        var page = 0;
        while(true)
        {
            var pageTrackIds = playlistTrackIds.Skip(CountTracksPerPage * page).Take(CountTracksPerPage).ToArray();
            if(pageTrackIds.Length == 0)
            {
                break;
            }

            var pageTracks = await _apiService.GetTracksAsync(pageTrackIds);
            foreach(var track in pageTracks)
            {
                await DownloadTrackAsync(track, playlistTitle);
            }

            page++;
        }
    }

    private async Task DownloadTrackAsync(TrackFullModel track, string playlistTitle)
    {
        var trackName = GetTrackName(track);
        var trackFilePath = GetTrackFilePath(playlistTitle, trackName);

        if(File.Exists(trackFilePath))
        {
            return;
        }

        var transcodingUrl = GetTranscodingProgressiveUrl(track.Media.Transcodings);
        if(transcodingUrl == null)
        {
            _notDownloadedTracks.Add(track);

            _logger.LogError($"track doesn't have progressive transcoding url, id: {track.Id}");
            return;
        }

        var trackBytes = await _apiService.DownloadTrackAsync(transcodingUrl);
        if(trackBytes.Length == 0)
        {
            _notDownloadedTracks.Add(track);

            _logger.LogError($"track has 0 bytes, id: {track.Id}");
            return;
        }

        await File.WriteAllBytesAsync(trackFilePath, trackBytes);

        _logger.LogInformation($"downloaded from {playlistTitle} track {trackName}");
    }

    private string GetTrackName(TrackFullModel track)
    {
        return $"{track.User.UserName} - {track.Title}";
    }

    private string GetTrackFilePath(string playlistTitle, string trackName)
    {
        return GetPath(playlistTitle, $"{GetValidFileName(trackName)}.mp3");
    }

    private string GetValidFileName(string fileName)
    {
        var validFileName = fileName;
        foreach(var invalidFileNameChar in Path.GetInvalidFileNameChars())
        {
            validFileName = validFileName.Replace(invalidFileNameChar, '_');
        }

        return validFileName;
    }

    private void CreatePlaylistDirectory(string title)
    {
        var path = GetPath(title);

        Directory.CreateDirectory(path);
    }

    private string GetPath(params string[] paths)
    {
        if(string.IsNullOrWhiteSpace(_downloadOptions.DownloadPath))
        {
            throw new Exception("download path is null");
        }

        return Path.Combine(_downloadOptions.DownloadPath, Path.Combine(paths));
    }

    private string? GetTranscodingProgressiveUrl(IReadOnlyCollection<TrackMediaTranscodingModel> transcodings)
    {
        var progressiveTranscoding = transcodings.FirstOrDefault(s => s.Format.Protocol.Equals("progressive"));

        return progressiveTranscoding?.Url;
    }
}