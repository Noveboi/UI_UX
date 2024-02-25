using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Ergasia_Final.Models;
using Ergasia_Final.Utilities;
using Ergasia_Final.Views;

namespace Ergasia_Final.ViewModels
{
	public class ExHallViewModel : Screen, IHandle<string>
    {
        // Museum Artist Data
        private readonly List<ArtistModel> _artists;
        private ArtistModel _currentArtist;

        // Artist Page related stuff
        private int _currentIndex = 0;
        private readonly int MAX_INDEX;
		private Visibility _leftNavButtonEnabled = Visibility.Hidden;
		private Visibility _rightNavButtonEnabled = Visibility.Visible;

        // Audio Player
		private double _volume = 0.5;
		private MediaElement _audioPlayer;
		private Uri _currentSong; // MediaElement 'Source' binding
		private bool _isPlaying = true;
        /// <summary>
        /// Necessary flag for when entering and exiting the window. The <see cref="WindowFactory"/> class maintains the ViewModels, 
        /// but the Views have to be re-initialized! That means that the <see cref="Window.Activated"/> event is NOT fired only once. 
        /// Thus the method for handling the event, <see cref="OnViewLoaded(ExHallView)"/> is invoked every time we come back to this 
        /// ViewModel and since <see cref="ExHallView"/> is re-initialized, it obtains a new <see cref="MediaElement"/> instance. Because
        /// <see cref="MediaElement"/>s use separate threads, that means we will have TWO MediaElements playing at the same time!
        /// </summary>
		private bool _hasInitialized = false; 

		// Storyboard for the vinyl icon rotating
		private Storyboard _vinylStoryboard;

        // Icons for the volume indicator image
		private static readonly Uri VOLUME_HIGH_IMG = new("/Images/volume_high.png", UriKind.Relative);
		private static readonly Uri VOLUME_MED_IMG = new("/Images/volume_medium.png", UriKind.Relative);
        private static readonly Uri VOLUME_LOW_IMG = new("/Images/volume_low.png", UriKind.Relative);
		private static readonly Uri VOLUME_MUTE_IMG = new("/Images/volume_mute.png", UriKind.Relative);

        public double Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                NotifyOfPropertyChange();

                // Set volume image according to value
                if (_volume  == 0)
                {
                    VolumeImage = VOLUME_MUTE_IMG;
                }
                else if (_volume > 0 && _volume <= 0.25)
                {
                    VolumeImage = VOLUME_LOW_IMG;
                }
                else if (_volume > 0.25 && _volume <= 0.75)
                {
                    VolumeImage = VOLUME_MED_IMG;
                }
                else 
                {
                    VolumeImage = VOLUME_HIGH_IMG;
                }
                NotifyOfPropertyChange(nameof(VolumeImage));
            }
        }
        public Uri VolumeImage { get; set; } = VOLUME_MED_IMG;
        public Uri CurrentSong
        {
            get => _currentSong;
            set
            {
                _currentSong = value;
                NotifyOfPropertyChange();
            }
        }
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
                CurrentSong = _currentArtist.SongPath;
                NotifyOfPropertyChange();
            }
        }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		public ExHallViewModel(IEventAggregator shellEvents)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		{
            _artists = new List<ArtistModel>();
            PopulateUtility.AddArtists(_artists);

            _currentArtist = _artists[0];
            MAX_INDEX = _artists.Count - 1;
            _currentSong = _currentArtist.SongPath;

            shellEvents.SubscribeOnUIThread(this);
        }

        private void CheckHideNavButtons()
        {
            LeftNavButtonEnabled = _currentIndex > 0 ? Visibility.Visible : Visibility.Hidden;
            RightNavButtonEnabled = _currentIndex < MAX_INDEX ? Visibility.Visible : Visibility.Hidden;
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
            if (_currentIndex < MAX_INDEX)
            {
                _currentIndex += 1;
                CurrentArtist = _artists[_currentIndex];
            }

            CheckHideNavButtons();
        }

        public void CreateStoryboard(Button source)
        {
            _vinylStoryboard = new Storyboard();
			DoubleAnimation constantRotation = new(0.0, 360.0, new Duration(TimeSpan.FromSeconds(3)))
			{
				RepeatBehavior = RepeatBehavior.Forever
			};
			_vinylStoryboard.Children.Add(constantRotation);
            Storyboard.SetTargetProperty(constantRotation, new PropertyPath("RenderTransform.Angle"));
            Storyboard.SetTargetName(constantRotation, "vinylIcon");
            _vinylStoryboard.Begin(source, true);
        }

        public void PlayPause(Button sender)
        {
            if (_vinylStoryboard.GetIsPaused(sender))
            {
                _isPlaying = true;
				_audioPlayer.Play();
                _vinylStoryboard.Resume(sender);
            }
            else
            {
				_isPlaying = false;
				_audioPlayer.Pause();
                _vinylStoryboard.Pause(sender);
            }
        }

        public void OnViewLoaded(ExHallView view)
        {
            if (!_hasInitialized)
            {
                _audioPlayer = view.AudioPlayer;
                _audioPlayer.Play();

                _hasInitialized = true;
            }
		}

		public Task HandleAsync(string message, CancellationToken cancellationToken)
		{
            if (message == "ExHall Exiting!")
            {
                _audioPlayer.Pause();
            }
            else if (message == "ExHall Opening!" && _isPlaying && _hasInitialized)
            {
                _audioPlayer.Play();
            }

            return Task.CompletedTask;
		}
	}
}
