using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Windows.Media.Control;

namespace SpotifyOverlay
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MouseLeftButtonDown += (s, e) =>
            {
                DragMove();
            };

            PreviewKeyDown += MainWindow_PreviewKeyDown;

            Loaded += async (s, e) =>
            {
                while (true)
                {
                    await UpdateSpotifyInfo();
                    await Task.Delay(1000);
                }
            };
        }

        private async Task<GlobalSystemMediaTransportControlsSession?> GetSpotifySession()
        {
            var manager =
                await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();

            return manager.GetSessions()
                .FirstOrDefault(s =>
                    s.SourceAppUserModelId.Contains("Spotify"));
        }

        private async Task UpdatePlayButton(
            GlobalSystemMediaTransportControlsSession spotifySession)
        {
            var playback = spotifySession.GetPlaybackInfo();

            if (playback.PlaybackStatus ==
                GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing)
            {
                PlayPauseImage.Source =
                    new BitmapImage(
                        new Uri("pack://application:,,,/Assets/Pause.jpg"));
            }
            else
            {
                PlayPauseImage.Source =
                    new BitmapImage(
                        new Uri("pack://application:,,,/Assets/Play.jpg"));
            }
        }

        private async Task UpdateSpotifyInfo()
        {
            try
            {
                var spotifySession = await GetSpotifySession();

                if (spotifySession == null)
                {
                    SongTitle.Text = "Spotify Not Found";
                    ArtistName.Text = "Open Spotify";
                    SongProgress.Value = 0;
                    return;
                }

                var media =
                    await spotifySession.TryGetMediaPropertiesAsync();

                SongTitle.Text =
                    string.IsNullOrWhiteSpace(media.Title)
                    ? "Unknown Song"
                    : media.Title;

                ArtistName.Text =
                    string.IsNullOrWhiteSpace(media.Artist)
                    ? "Unknown Artist"
                    : media.Artist;

                await UpdatePlayButton(spotifySession);

                var timeline =
                    spotifySession.GetTimelineProperties();

                if (timeline.EndTime.TotalSeconds > 0)
                {
                    SongProgress.Maximum =
                        timeline.EndTime.TotalSeconds;

                    SongProgress.Value =
                        timeline.Position.TotalSeconds;
                }

                if (media.Thumbnail != null)
                {
                    using var stream =
                        await media.Thumbnail.OpenReadAsync();

                    var bitmap = new BitmapImage();

                    bitmap.BeginInit();
                    bitmap.StreamSource =
                        stream.AsStreamForRead();

                    bitmap.CacheOption =
                        BitmapCacheOption.OnLoad;

                    bitmap.EndInit();
                    bitmap.Freeze();

                    AlbumArt.Source = bitmap;
                }
            }
            catch (Exception ex)
            {
                SongTitle.Text = "Error";
                ArtistName.Text = ex.Message;
            }
        }

        private async void PlayPause_Click(
            object sender,
            RoutedEventArgs e)
        {
            var spotifySession =
                await GetSpotifySession();

            if (spotifySession != null)
            {
                await spotifySession.TryTogglePlayPauseAsync();

                await Task.Delay(250);

                await UpdateSpotifyInfo();
            }
        }

        private async void Prev_Click(
            object sender,
            RoutedEventArgs e)
        {
            var spotifySession =
                await GetSpotifySession();

            if (spotifySession != null)
            {
                await spotifySession.TrySkipPreviousAsync();
            }
        }

        private async void Next_Click(
            object sender,
            RoutedEventArgs e)
        {
            var spotifySession =
                await GetSpotifySession();

            if (spotifySession != null)
            {
                await spotifySession.TrySkipNextAsync();
            }
        }

        private async void MainWindow_PreviewKeyDown(
            object sender,
            KeyEventArgs e)
        {
            var spotifySession =
                await GetSpotifySession();

            if (spotifySession == null)
                return;

            switch (e.Key)
            {
                case Key.F1:
                    await spotifySession.TrySkipPreviousAsync();
                    break;

                case Key.F2:
                    await spotifySession.TryTogglePlayPauseAsync();
                    break;

                case Key.F3:
                    await spotifySession.TrySkipNextAsync();
                    break;

                case Key.F4:
                    Visibility =
                        Visibility == Visibility.Visible
                        ? Visibility.Hidden
                        : Visibility.Visible;
                    break;
            }
        }

        private void Close_Click(
            object sender,
            RoutedEventArgs e)
        {
            Close();
        }

        private void Github_Click(
            object sender,
            RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/ThatOneGuy-67",
                UseShellExecute = true
            });
        }
    }
}