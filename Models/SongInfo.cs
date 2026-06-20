using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyOverlay.Models
{
    public class SongInfo
    {
        public string Title { get; set; } = "";

        public string Artist { get; set; } = "";

        public double Duration { get; set; }

        public double Position { get; set; }

        public bool IsPlaying { get; set; }

        public byte[]? AlbumArt { get; set; }
    }
}
