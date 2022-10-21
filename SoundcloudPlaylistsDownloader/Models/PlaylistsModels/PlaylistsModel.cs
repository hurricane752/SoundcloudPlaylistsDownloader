using Newtonsoft.Json;

namespace SoundcloudPlaylistsDownloader.Models.PlaylistsModels;

public sealed record PlaylistsModel
{
    [JsonConstructor]
    public PlaylistsModel(IReadOnlyCollection<PlaylistInfoModel> collection)
    {
        Collection = collection;
    }

    [JsonProperty(Required = Required.Always, PropertyName = "collection")]
    public IReadOnlyCollection<PlaylistInfoModel> Collection { get; }
}