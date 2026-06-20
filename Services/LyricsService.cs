using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpotifyOverlay.Services
{
    public class LyricsService
    {
        private static readonly HttpClient client = new();

        public async Task<string> GetLyricsAsync(string title, string artist)
        {
            try
            {
                string url =
                    $"https://lrclib.net/api/get?artist_name={Uri.EscapeDataString(artist)}&track_name={Uri.EscapeDataString(title)}";

                string json = await client.GetStringAsync(url);

                using JsonDocument doc = JsonDocument.Parse(json);

                if (doc.RootElement.TryGetProperty("plainLyrics", out var lyrics))
                    return lyrics.GetString() ?? "No lyrics found.";

                return "No lyrics found.";
            }
            catch
            {
                return "Lyrics unavailable.";
            }
        }
    }
}