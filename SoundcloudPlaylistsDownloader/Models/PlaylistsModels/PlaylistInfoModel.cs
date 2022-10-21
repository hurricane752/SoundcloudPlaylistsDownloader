using Newtonsoft.Json;
using SoundcloudPlaylistsDownloader.Models.PlaylistModels;

namespace SoundcloudPlaylistsDownloader.Models.PlaylistsModels;

public sealed record PlaylistInfoModel
{
    [JsonConstructor]
    public PlaylistInfoModel(PlaylistShortModel playlist)
    {
        Playlist = playlist;
    }

    [JsonProperty(Required = Required.Always, PropertyName = "playlist")]
    public PlaylistShortModel Playlist { get; }
}