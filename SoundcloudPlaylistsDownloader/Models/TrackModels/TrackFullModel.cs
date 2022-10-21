using Newtonsoft.Json;
using SoundcloudPlaylistsDownloader.Models.UserModels;

namespace SoundcloudPlaylistsDownloader.Models.TrackModels;

public sealed record TrackFullModel : TrackModel
{
    [JsonConstructor]
    public TrackFullModel(ulong id, string title, string permalinkUrl, TrackMediaModel media, UserModel user) : base(id)
    {
        Title = title;
        PermalinkUrl = permalinkUrl;
        Media = media;
        User = user;
    }

    [JsonProperty(Required = Required.Always, PropertyName = "title")]
    public string Title { get; }

    [JsonProperty(Required = Required.Always, PropertyName = "permalink_url")]
    public string PermalinkUrl { get; set; }

    [JsonProperty(Required = Required.Always, PropertyName = "media")]
    public TrackMediaModel Media { get; }

    [JsonProperty(Required = Required.Always, PropertyName = "user")]
    public UserModel User { get; }
}