using Caliburn.Micro;
using Ergasia_Final.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace Ergasia_Final.ViewModels
{
    public class ArtistHallViewModel : Screen
    {

        public Uri ArtistBackground { get; private set; }
        public VideoPaketoModel Concert1 { get; set; }
        public VideoPaketoModel Concert2 { get; set; }
        public VideoPaketoModel Interview { get; set; }
        public VideoPaketoModel Cover { get; set; }

        /// <param name="artistName">Determines what content to render based on the artist's name given</param>
        public ArtistHallViewModel(string artistName)
        {
            SetArtistBackground(artistName);
            SetNavigateUris(artistName);
        }

        private void SetArtistBackground(string artistName)
        {
            if (artistName == "taylor")
            {
                ArtistBackground = new Uri("./Images/ts_bg.jpg", UriKind.RelativeOrAbsolute);
            }
            else if (artistName == "m83")
            {
                
            }
            else if (artistName == "altj")
            {

            }
        } 

        private void SetNavigateUris(string artistName)
        {
            if (artistName == "taylor")
            {
                Concert1 = new VideoPaketoModel()
                {
                    VideoLink = new Uri("https://www.youtube.com/watch?v=P5JLMp08GC0")
                };
            }
        }

        public void OnRequestNavigate(Button source)
        {
            Hyperlink href = source.Content as Hyperlink;
            Process process = new Process()
            {
                StartInfo = new ProcessStartInfo(href.NavigateUri.AbsoluteUri)
            };
            process.StartInfo.UseShellExecute = true;
            process.Start();
        }
    }
}
