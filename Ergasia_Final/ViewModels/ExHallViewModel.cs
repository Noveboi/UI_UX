using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ergasia_Final.Models;

namespace Ergasia_Final.ViewModels
{
    public class ExHallViewModel
    {
        List<ArtistModel> artists;
        public ExHallViewModel()
        {
            artists = new List<ArtistModel>();
        }
        private void addArtists()
        {
            artists.Add(new ArtistModel
            {
                Name = "Taylor Swift",
                Description = "Taylor Swift, a globally acclaimed singer-songwriter and performer, stands as a prominent figure in " +
               "the contemporary music landscape. Born on December 13, 1989, in Reading, Pennsylvania, Swift catapulted to fame " +
               "with her innate talent for crafting emotionally resonant lyrics and infectious melodies. Recognized for her ability" +
               " to seamlessly navigate various music genres, Swift has evolved from her country roots to embrace pop and indie " +
               "influences, showcasing her versatility and artistic growth. With a career marked by chart-topping albums such " +
               "as \"Fearless,\" \"1989,\" and \"Lover,\" she has amassed numerous accolades, including multiple Grammy Awards. " +
               "Beyond her musical prowess, Swift is renowned for her advocacy work, outspokenness on social issues, and her " +
               "ability to connect intimately with her fan base through candid storytelling in her songs. The epitome of a " +
               "modern-day pop icon, Taylor Swift continues to shape the industry with her unparalleled creativity and unwavering " +
               "authenticity.",
                ImagePath = "",
                SongPath = ""
            });
        }
    }
}
