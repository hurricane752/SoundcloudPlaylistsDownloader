using Newtonsoft.Json;

namespace SoundcloudPlaylistsDownloader.Models.PlaylistModels;

public sealed record PlaylistShortModel : PlaylistModel
{
    [JsonConstructor]
    public PlaylistShortModel(ulong id, ulong userId, string title) : base(id, userId, title)
    {
    }
}