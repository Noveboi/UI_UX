using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Ergasia_Final.Models
{
    public class ArtistModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
		public BitmapImage Portrait { get; set; } 
        public Uri? SongPath { get; set; }
        public string SongName { get; set; } = string.Empty;
		public string AlbumName { get; set; } = string.Empty;

        public ArtistModel()
        {
            Portrait = new BitmapImage();
        }
	}
}
