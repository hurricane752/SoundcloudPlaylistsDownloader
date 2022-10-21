using Newtonsoft.Json;
using SoundcloudPlaylistsDownloader.Models.PlaylistModels;
using SoundcloudPlaylistsDownloader.Models.PlaylistsModels;
using SoundcloudPlaylistsDownloader.Models.TrackModels;
using SoundcloudPlaylistsDownloader.Models.UserModels;

namespace SoundcloudPlaylistsDownloader.Services;

public sealed class SoundcloudApiService
{
    private readonly HttpClient _httpClient;

    public SoundcloudApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<UserModel> GetUserAsync()
    {
        return GetAsync<UserModel>("me");
    }

    public Task<PlaylistsModel> GetUserPlaylistsAsync()
    {
        return GetAsync<PlaylistsModel>($"me/library/all?&limit={10000}&offset={0}");
    }

    public Task<PlaylistFullModel> GetPlaylistAsync(ulong id)
    {
        return GetAsync<PlaylistFullModel>($"playlists/{id}?representation=full");
    }

    public Task<IReadOnlyCollection<TrackFullModel>> GetTracksAsync(IReadOnlyCollection<ulong> ids)
    {
        return GetAsync<IReadOnlyCollection<TrackFullModel>>($"tracks?ids={string.Join(",", ids)}");
    }

    public async Task<byte[]> DownloadTrackAsync(string transcodingUrl)
    {
        var model = await GetAsync<TrackDownloadModel>(transcodingUrl);

        return await _httpClient.GetByteArrayAsync(model.Url);
    }

    private Task<T> GetAsync<T>(string url) where T : class
    {
        return SendAsync<T>(new HttpRequestMessage(HttpMethod.Get, url));
    }

    private async Task<T> SendAsync<T>(HttpRequestMessage message) where T : class
    {
        var response = await _httpClient.SendAsync(message);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<T>(json);
        if(result == null)
        {
            throw new Exception($"deserialize result is null, url: {message.RequestUri}, response: {json}");
        }

        return result;
    }
}