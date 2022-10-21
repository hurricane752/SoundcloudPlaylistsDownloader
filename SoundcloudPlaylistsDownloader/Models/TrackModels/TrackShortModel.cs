using Newtonsoft.Json;

namespace SoundcloudPlaylistsDownloader.Models.TrackModels;

public sealed record TrackShortModel : TrackModel
{
    [JsonConstructor]
    public TrackShortModel(ulong id) : base(id)
    {
    }
}