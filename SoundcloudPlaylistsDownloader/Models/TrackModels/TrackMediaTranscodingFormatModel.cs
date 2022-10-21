using Newtonsoft.Json;

namespace SoundcloudPlaylistsDownloader.Models.TrackModels;

public sealed record TrackMediaTranscodingFormatModel
{
    [JsonConstructor]
    public TrackMediaTranscodingFormatModel(string protocol)
    {
        Protocol = protocol;
    }

    [JsonProperty(Required = Required.Always, PropertyName = "protocol")]
    public string Protocol { get; }
}