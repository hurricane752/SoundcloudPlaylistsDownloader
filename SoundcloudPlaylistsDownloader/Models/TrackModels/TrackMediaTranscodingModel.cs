using Newtonsoft.Json;

namespace SoundcloudPlaylistsDownloader.Models.TrackModels;

public sealed record TrackMediaTranscodingModel
{
    [JsonConstructor]
    public TrackMediaTranscodingModel(string url, TrackMediaTranscodingFormatModel format)
    {
        Url = url;
        Format = format;
    }

    [JsonProperty(Required = Required.Always, PropertyName = "url")]
    public string Url { get; }

    [JsonProperty(Required = Required.Always, PropertyName = "format")]
    public TrackMediaTranscodingFormatModel Format { get; }
}