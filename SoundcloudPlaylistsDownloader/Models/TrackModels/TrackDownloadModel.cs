using Newtonsoft.Json;

namespace SoundcloudPlaylistsDownloader.Models.TrackModels;

public sealed record TrackDownloadModel
{
    [JsonConstructor]
    public TrackDownloadModel(string url)
    {
        Url = url;
    }

    [JsonProperty(Required = Required.Always, PropertyName = "url")]
    public string Url { get; }
}