using Microsoft.Extensions.Options;
using SoundcloudPlaylistsDownloader.Options;

namespace SoundcloudPlaylistsDownloader.HttpHandlers;

public sealed class AuthHttpMessageHandler : DelegatingHandler
{
    private readonly AuthOptions _authOptions;

    public AuthHttpMessageHandler(IOptions<AuthOptions> authOptions)
    {
        _authOptions = authOptions.Value;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("Authorization", _authOptions.Authorization);

        if(request.Method == HttpMethod.Get && request.RequestUri != null)
        {
            var joinSymbol = "?";
            if(!string.IsNullOrWhiteSpace(request.RequestUri.Query))
            {
                joinSymbol = "&";
            }

            request.RequestUri = new Uri($"{request.RequestUri}{joinSymbol}client_id={_authOptions.ClientId}");
        }

        var response = await base.SendAsync(request, cancellationToken);

        return response;
    }
}