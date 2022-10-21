using Newtonsoft.Json;

namespace SoundcloudPlaylistsDownloader.Models.PlaylistModels;

public abstract record PlaylistModel : EntityModel
{
    [JsonConstructor]
    protected PlaylistModel(ulong id, ulong userId, string title) : base(id)
    {
        UserId = userId;
        Title = title;
    }

    [JsonProperty(Required = Required.Always, PropertyName = "user_id")]
    public ulong UserId { get; }

    [JsonProperty(Required = Required.Always, PropertyName = "title")]
    public string Title { get; }
}