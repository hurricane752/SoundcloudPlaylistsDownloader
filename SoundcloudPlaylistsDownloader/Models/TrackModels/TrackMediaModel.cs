using Newtonsoft.Json;

namespace SoundcloudPlaylistsDownloader.Models.TrackModels;

public sealed record TrackMediaModel
{
    [JsonConstructor]
    public TrackMediaModel(IReadOnlyCollection<TrackMediaTranscodingModel> transcodings)
    {
        Transcodings = transcodings;
    }

    [JsonProperty(Required = Required.Always, PropertyName = "transcodings")]
    public IReadOnlyCollection<TrackMediaTranscodingModel> Transcodings { get; }
}