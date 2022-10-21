using Newtonsoft.Json;
using SoundcloudPlaylistsDownloader.Models.TrackModels;

namespace SoundcloudPlaylistsDownloader.Models.PlaylistModels;

public sealed record PlaylistFullModel : PlaylistModel
{
    [JsonConstructor]
    public PlaylistFullModel(ulong id, ulong userId, string title, IReadOnlyCollection<TrackShortModel> tracks)
        : base(id, userId, title)
    {
        Tracks = tracks;
    }

    public IReadOnlyCollection<TrackShortModel> Tracks { get; }
}