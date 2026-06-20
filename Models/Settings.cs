using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyOverlay.Models
{
    public class Settings
    {
        public bool AlwaysOnTop { get; set; } = true;

        public bool StartWithWindows { get; set; }

        public bool CompactMode { get; set; }

        public bool ShowLyrics { get; set; }

        public double Opacity { get; set; } = 1.0;

        public string Theme { get; set; } = "Spotify";
    }
}
