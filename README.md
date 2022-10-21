# SoundcloudPlaylistsDownloader

Downloads user playlists from soundcloud

Arguments:
1) "authorization" is required
2) "client-id" is required
3) "download-path" is required
4) "download-only-user-playlists" is optional, by default = true

Example:
dotnet SoundcloudPlaylistsDownloader.dll --authorization="OAuth 123" --client-id="asd123" --download-path="C:\playlists" --download-only-user-playlists=true