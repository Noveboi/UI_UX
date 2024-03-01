using Caliburn.Micro;
using Ergasia_Final.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace Ergasia_Final.ViewModels
{
    public class ArtistHallViewModel : Screen
    {

        public Uri? ArtistBackground { get; private set; }
        public VideoPaketoModel? Concert1 { get; set; }
        public VideoPaketoModel? Concert2 { get; set; }
        public VideoPaketoModel? Interview { get; set; }
        public VideoPaketoModel? Cover { get; set; }

        /// <param name="artistName">Determines what content to render based on the artist's name given</param>
        public ArtistHallViewModel(string artistName)
        {
            SetArtistBackground(artistName);
            SetNavigateUris(artistName);
        }

        // Sets the Image background of the main Grid
        private void SetArtistBackground(string artistName)
        {
            ArtistBackground = new Uri($"/Images/{artistName}_bg.jpg", UriKind.Relative);
        } 

        /// <summary>
        /// Instantiates the <see cref="VideoPaketoModel"/> models based on the <paramref name="artistName"/> parameter
        /// </summary>
        /// <param name="artistName"></param>
        private void SetNavigateUris(string artistName)
        {
            Concert1 = new VideoPaketoModel()
            {
                ImagePath = new Uri($"/Images/{artistName}_video1.jpg", UriKind.Relative)
            };
            Concert2 = new VideoPaketoModel()
            {
                ImagePath = new Uri($"/Images/{artistName}_video2.jpg", UriKind.Relative)
            };
            Interview = new VideoPaketoModel()
            {
                ImagePath = new Uri($"/Images/{artistName}_video3.jpg", UriKind.Relative)
            };
            Cover = new VideoPaketoModel()
            {
                ImagePath = new Uri($"/Images/{artistName}_video4.jpg", UriKind.Relative)
            };

            if (artistName == "ts")
            {
                Concert1.VideoLink = new Uri("https://www.youtube.com/watch?v=SVY8I46dkb0");
                Concert2.VideoLink = new Uri("https://www.youtube.com/watch?v=qdk01wkzKzo");
                Interview.VideoLink = new Uri("https://www.youtube.com/watch?v=XnbCSboujF4");
                Cover.VideoLink = new Uri("https://www.youtube.com/watch?v=16hiMn6V9Wk&list=WL&index=225");
			}
            if (artistName == "altj")
            {
			    Concert1.VideoLink = new Uri("https://www.youtube.com/watch?v=pcVRrlmpcWk");
			    Concert2.VideoLink = new Uri("https://www.youtube.com/watch?v=8zHdLF3-coA");
			    Interview.VideoLink = new Uri("https://www.youtube.com/watch?v=W4eGSuu92uA");
                Cover.VideoLink = new Uri("https://www.youtube.com/watch?v=YJzxYFyZvFM");
			}
            if (artistName == "m83")
            {
                Concert1.VideoLink = new Uri("https://www.youtube.com/watch?v=ErrKRT3VjKk");
                Concert2.VideoLink = new Uri("https://www.youtube.com/watch?v=XY-Sw9uBw9k");
			    Interview.VideoLink = new Uri("https://www.youtube.com/watch?v=7XWjaOlk8P0&t=17s");
                Cover.VideoLink = new Uri("https://www.youtube.com/watch?v=18_Gg2vwybY");
			}
		}

        public static void OnRequestNavigate(Button source)
        {
            Hyperlink? href = source.Content as Hyperlink;

            if (href is null || href?.NavigateUri is null)
            {
                MessageBox.Show("Video not available!", "No link", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo(href.NavigateUri.AbsoluteUri)
            };
            process.StartInfo.UseShellExecute = true;
            process.Start();
        }
    }
}
