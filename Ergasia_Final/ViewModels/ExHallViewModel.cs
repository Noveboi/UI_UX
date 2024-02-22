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
        private List<ArtistModel> _artists;
        private ArtistModel _currentArtist;
        private int _currentIndex = 0;
        private int _maxIndex;
        private double _volume = 0.5;
        private Visibility _leftNavButtonEnabled = Visibility.Hidden;
        private Visibility _rightNavButtonEnabled = Visibility.Visible;
        private Storyboard _vinylStoryboard;
        private MediaElement _audioPlayer;
        private Uri _currentSong;
        private bool _isPlaying = true;
        private bool _hasInitialized = false;


        private static readonly Uri VOLUME_HIGH_IMG = new Uri("/Images/volume_high.png", UriKind.Relative);
        private static readonly Uri VOLUME_MED_IMG = new Uri("/Images/volume_medium.png", UriKind.Relative);
        private static readonly Uri VOLUME_LOW_IMG = new Uri("/Images/volume_low.png", UriKind.Relative);
        private static readonly Uri VOLUME_MUTE_IMG = new Uri("/Images/volume_mute.png", UriKind.Relative);

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
        public ExHallViewModel(IEventAggregator shellEvents)
        {
            _artists = new List<ArtistModel>();
            PopulateUtility.AddArtists(_artists);

            _currentArtist = _artists[0];
            _maxIndex = _artists.Count - 1;
            _currentSong = _currentArtist.SongPath;

            shellEvents.SubscribeOnUIThread(this);
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

        public void CreateStoryboard(Button source)
        {
            _vinylStoryboard = new Storyboard();
            DoubleAnimation constantRotation = new DoubleAnimation(0.0, 360.0, new Duration(TimeSpan.FromSeconds(3)));
            constantRotation.RepeatBehavior = RepeatBehavior.Forever;
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
