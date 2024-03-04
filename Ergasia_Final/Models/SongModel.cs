using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Ergasia_Final.Models
{
    public class SongModel
    {
        private int _bpm;

        public string RowID { get; set; } = string.Empty;
        public string ArtistName { get; set; } = string.Empty;
		public string Title { get; set; } = string.Empty;
		public SpeedTypes Speed { get; private set; }
        public GenreTypes Genre { get; set; }
        public int BPM
        {
            get => _bpm;
            set
            {
                _bpm = value;
                // Automatically set Speed
                Speed = _bpm >= 110 ? SpeedTypes.Fast : SpeedTypes.Slow;
            }
        }
        public Uri? AudioPath { get; set; }
        public string Lyrics { get; set; } = string.Empty;
	}
}
