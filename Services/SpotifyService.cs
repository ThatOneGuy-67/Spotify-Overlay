using SpotifyOverlay.Models;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Media.Control;

namespace SpotifyOverlay.Services
{
    public class SpotifyService
    {
        private GlobalSystemMediaTransportControlsSessionManager? _manager;

        public async Task InitializeAsync()
        {
            _manager =
                await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
        }

        public GlobalSystemMediaTransportControlsSession? GetSpotifySession()
        {
            if (_manager == null)
                return null;

            return _manager
                .GetSessions()
                .FirstOrDefault(s =>
                    s.SourceAppUserModelId.Contains("Spotify"));
        }

        public async Task<SongInfo?> GetCurrentSongAsync()
        {
            var session = GetSpotifySession();

            if (session == null)
                return null;

            var media =
                await session.TryGetMediaPropertiesAsync();

            var timeline =
                session.GetTimelineProperties();

            byte[]? art = null;

            if (media.Thumbnail != null)
            {
                using var stream =
                    await media.Thumbnail.OpenReadAsync();

                using var memory = new MemoryStream();

                await stream.AsStreamForRead()
                    .CopyToAsync(memory);

                art = memory.ToArray();
            }

            return new SongInfo
            {
                Title = media.Title,
                Artist = media.Artist,

                Duration =
                    timeline.EndTime.TotalSeconds,

                Position =
                    timeline.Position.TotalSeconds,

                IsPlaying =
                    session.GetPlaybackInfo().PlaybackStatus ==
                    GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing,

                AlbumArt = art
            };
        }

        public async Task PlayPauseAsync()
        {
            var session = GetSpotifySession();

            if (session != null)
                await session.TryTogglePlayPauseAsync();
        }

        public async Task NextAsync()
        {
            var session = GetSpotifySession();

            if (session != null)
                await session.TrySkipNextAsync();
        }

        public async Task PreviousAsync()
        {
            var session = GetSpotifySession();

            if (session != null)
                await session.TrySkipPreviousAsync();
        }
    }
}