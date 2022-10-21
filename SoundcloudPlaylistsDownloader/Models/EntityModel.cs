using Newtonsoft.Json;

namespace SoundcloudPlaylistsDownloader.Models;

public abstract record EntityModel
{
    [JsonConstructor]
    protected EntityModel(ulong id)
    {
        Id = id;
    }

    [JsonProperty(Required = Required.Always, PropertyName = "id")]
    public ulong Id { get; set; }
}