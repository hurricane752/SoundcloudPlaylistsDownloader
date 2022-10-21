using Newtonsoft.Json;

namespace SoundcloudPlaylistsDownloader.Models.TrackModels;

public abstract record TrackModel : EntityModel
{
    [JsonConstructor]
    protected TrackModel(ulong id) : base(id)
    {
    }
}