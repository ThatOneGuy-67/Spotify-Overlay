using SpotifyOverlay.Models;
using System;
using System.IO;
using System.Text.Json;

namespace SpotifyOverlay.Services
{
    public class SettingsService
    {
        private readonly string settingsPath =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "SpotifyOverlay",
                "settings.json");

        public Settings Load()
        {
            try
            {
                if (!File.Exists(settingsPath))
                    return new Settings();

                string json = File.ReadAllText(settingsPath);

                return JsonSerializer.Deserialize<Settings>(json)
                       ?? new Settings();
            }
            catch
            {
                return new Settings();
            }
        }

        public void Save(Settings settings)
        {
            string? folder = Path.GetDirectoryName(settingsPath);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder!);

            File.WriteAllText(
                settingsPath,
                JsonSerializer.Serialize(settings,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                }));
        }
    }
}