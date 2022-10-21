using Newtonsoft.Json;

namespace SoundcloudPlaylistsDownloader.Models.UserModels;

public sealed record UserModel : EntityModel
{
    [JsonConstructor]
    public UserModel(ulong id, string userName) : base(id)
    {
        UserName = userName;
    }

    [JsonProperty(Required = Required.Always, PropertyName = "username")]
    public string UserName { get; }
}