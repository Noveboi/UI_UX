using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Ergasia_Final.Models;
using Ergasia_Final.Views;
using Xceed.Wpf.AvalonDock.Controls;

namespace Ergasia_Final.ViewModels
{
    public class ExHallViewModel : Screen
    {
        private List<ArtistModel> _artists;
        private ArtistModel _currentArtist;
        private int _currentIndex = 0;
        private int _maxIndex;
        private Visibility _leftNavButtonEnabled = Visibility.Hidden;
        private Visibility _rightNavButtonEnabled = Visibility.Visible;
        private Storyboard _vinylStoryboard;

        public Visibility LeftNavButtonEnabled
        {
            get => _leftNavButtonEnabled;
            set
            {
                _leftNavButtonEnabled = value;
                NotifyOfPropertyChange();
            }
        }
        public Visibility RightNavButtonEnabled
        {
            get => _rightNavButtonEnabled;
            set
            {
                _rightNavButtonEnabled = value;
                NotifyOfPropertyChange();
            }
        }
        public ArtistModel CurrentArtist
        {
            get => _currentArtist;
            set
            {
                _currentArtist = value;
                NotifyOfPropertyChange();
            }
        }
        public ExHallViewModel()
        {
            _artists = new List<ArtistModel>();
            addArtists();

            _currentArtist = _artists[0];
            _maxIndex = _artists.Count - 1;
        }
        private void addArtists()
        {
            _artists.Add(new ArtistModel
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
               "modern-day pop icon, Taylor Swift continues to shape the industry with her unparalleled creativity and unwavering authenticity.",
                SongName = "Cruel Summer",
                Portrait = new BitmapImage(new Uri(@"/Ergasia_Final;component/Images/taylor_portrait.jpg", UriKind.Relative)),
                AlbumName = "Lover"
            });

            _artists.Add(new ArtistModel
            {
                Name = "M83",
                Description = "M83 is a French electronic music project formed in 2001 by musician and producer Anthony Gonzalez. " +
                "The group takes its name from the spiral galaxy Messier 83. M83 is known for its ethereal and atmospheric " +
                "sound, blending elements of shoegaze, dream pop, and synth-pop. One of their breakthrough albums, \"Hurry Up, " +
                "We're Dreaming\" (2011), gained widespread acclaim and featured the hit single \"Midnight City,\" which became a " +
                "commercial success and is often regarded as one of their signature tracks. M83's music is characterized by lush " +
                "synthesizers, emotive melodies, and cinematic qualities, often evoking a sense of nostalgia and otherworldly beauty." +
                " Over the years, M83 has continued to evolve its sound, creating immersive sonic landscapes that resonate with a diverse audience.",
                SongName = "Midnight City",
                Portrait = new BitmapImage(new Uri(@"/Ergasia_Final;component/Images/m83_portrait.jpg", UriKind.Relative)),
                AlbumName = "Hurry up, We're Dreaming"
            });

            _artists.Add(new ArtistModel
            {
                Name = "alt-J",
                Description = "Alt-J, stylized as ∆, is an English indie rock band formed in 2007. The group's name is derived from the keyboard" +
                " shortcut used to create a delta symbol (∆) on Mac computers. The band consists of Joe Newman (guitar/vocals), Gus Unger-Hamilton" +
                " (keyboards/vocals), and Thom Green (drums). Alt-J gained widespread recognition with their debut album, \"An Awesome Wave\"" +
                " (2012), which won the Mercury Prize. Their music is characterized by a unique blend of intricate instrumentation, experimental" +
                " soundscapes, and Newman's distinctive falsetto vocals. Alt-J's sound incorporates elements of indie rock, folk, and electronic" +
                " music, creating a genre-defying sonic experience. Subsequent albums, such as \"This Is All Yours\" (2014) and \"Relaxer\" " +
                "(2017), further showcased their innovative approach to music-making. The band's complex and layered compositions, coupled with" +
                " enigmatic lyrics, contribute to their reputation as one of the more inventive and genre-crossing acts in contemporary indie music.",
                SongName = "Breezeblocks",
                Portrait = new BitmapImage(new Uri(@"/Ergasia_Final;component/Images/altj_portrait.jpg", UriKind.Relative)),
                AlbumName = "An Awesome Wave"
            });
        }
        
        private void CheckHideNavButtons()
        {
            LeftNavButtonEnabled = _currentIndex > 0 ? Visibility.Visible : Visibility.Hidden;
            RightNavButtonEnabled = _currentIndex < _maxIndex ? Visibility.Visible : Visibility.Hidden;
        }

        public void GoLeft()
        {
            if (_currentIndex > 0)
            {
                _currentIndex -= 1;
                CurrentArtist = _artists[_currentIndex];
            }

            CheckHideNavButtons();
        }

        public void GoRight()
        {
            if (_currentIndex < _maxIndex)
            {
                _currentIndex += 1;
                CurrentArtist = _artists[_currentIndex];
            }

            CheckHideNavButtons();
        }

        public void CreateStoryboard(Image source)
        {
            _vinylStoryboard = new Storyboard();
            DoubleAnimation constantRotation = new DoubleAnimation(0.0, 360.0, new Duration(TimeSpan.FromSeconds(3)));
            constantRotation.RepeatBehavior = RepeatBehavior.Forever;
            _vinylStoryboard.Children.Add(constantRotation);
            Storyboard.SetTargetProperty(constantRotation, new PropertyPath("RenderTransform.Angle"));
            Storyboard.SetTargetName(constantRotation, "vinylIcon");
            _vinylStoryboard.Begin((Button)source.Parent, true);
        }

        public void PlayPause(Button sender)
        {
            if (_vinylStoryboard.GetIsPaused(sender))
            {
                _vinylStoryboard.Resume(sender);
            }
            else
            {
                _vinylStoryboard.Pause(sender);
            }
        }
    }
}
