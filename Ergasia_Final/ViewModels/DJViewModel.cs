using Caliburn.Micro;
using CommunityToolkit.Mvvm.Input;
using Ergasia_Final.Models;
using Ergasia_Final.Utilities;
using Ergasia_Final.Views;
using GongSolutions.Wpf.DragDrop;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Ergasia_Final.ViewModels
{
    public class DJViewModel : Screen, IDropTarget, 
                               IHandle<int>, IHandle<string>, IHandle<Tuple<string, double>>
    {
        #region Fields & Properties
        private double bpm;
        /// <summary>
        /// EventAggregator used to send updates to the <see cref="KaraokeViewModel"/> when a new song plays
        /// </summary>
        private readonly IEventAggregator _djEvents;
        /// <summary>
        /// EventAggregator used for sending/handling messages between threads within this ViewModel. 
        /// See for example the method <see cref="StartSeekerPositionUpdateAsync(CancellationToken)"/>
        /// </summary>
        private readonly IEventAggregator _localThreadEvents;

        // Brushes and Colors
		private Color lightsColor = (Color)ColorConverter.ConvertFromString("#cc1188");
		private Brush effectsButtonColor;
        private static readonly Brush EFFECTS_DISABLED = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#666666"));
        private static readonly Brush EFFECTS_OFF = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#a01039"));
        private static readonly Brush EFFECTS_ON = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#13a51d"));

        // Locators for audio/images
        private static readonly Uri PLAY_IMAGE = new Uri("/Images/play.png", UriKind.Relative);
        private static readonly Uri PAUSE_IMAGE = new Uri("/Images/pause.png", UriKind.Relative);
        private Uri currentAudioSource;

        // Booleans for simple condition checking
        private bool isPlaying = false;
        private bool hasMediaOpened = false;
		private bool karaokeOpen = false;
		private bool hasFinishedLoading = true;
        private bool initialLoad = false;
        private bool userSeeking = false;
        private bool exitWhilePlaying = false;

        // Seeker-related fields
        private int currentSeekerMaximum;
        private string currentSongDuration;
        private string currentSongTime = "00:00";
        private double currentSongElapsedSeconds = 0.0;

        private CancellationTokenSource cancelSeekerPositionUpdate;
        private CancellationToken seekerCancelToken;

		private MediaElement _audioPlayer;

        /// <summary>
        /// Bound to the IsEnabled property of the play/pause media control button.
        /// This property ensures that the user does not click "Pause" before the media (audio) has finished loading.
        /// </summary>
		public bool PauseAvailable
        {
            get => hasFinishedLoading;
            set
            {
                hasFinishedLoading = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Bound to the <see cref="Slider.MaximizeValue"/> property
        /// </summary>
        public int CurrentSeekerMaximum
        {
            get => currentSeekerMaximum;
            set
            {
                currentSeekerMaximum = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Store the current song's elapsed time from the <see cref="MediaElement.Position"/> property
        /// </summary>
        public double CurrentSongElapsedSeconds
        {
            get => currentSongElapsedSeconds;
            set
            {
                currentSongElapsedSeconds = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Displays the current song's duration in mm:ss format
        /// </summary>
        public string CurrentSongDuration
        {
            get => currentSongDuration;
            set
            {
                currentSongDuration = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Displays the running song time in mm:ss format
        /// </summary>
        public string CurrentSongTime
        {
            get => currentSongTime;
            set
            {
                currentSongTime = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Bound [Two-Way] to the <see cref="Slider"/> (Seeker) Value property
        /// </summary>
        public double Bpm
        {
            get => bpm;
            set
            {
                bpm = Math.Round(value);
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Bound to the <see cref="DataGrid"/> of the <see cref="DJView"/>
        /// </summary>
        public BindableCollection<SongModel> SongQueue { get; }

        /// <summary>
        /// Bound to the "Effects" button's IsEnabled property. Used for other logic concerning the karaoke window as well!
        /// </summary>
        public bool KaraokeOpen
        {
            get => karaokeOpen;
            set
            {
                karaokeOpen = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Bound to the "Effects" button (really?? no way!)
        /// </summary>
        public Brush EffectsButtonColor
        {
            get => effectsButtonColor;
            set
            {
                effectsButtonColor = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Bound to <see cref="Xceed.Wpf.Toolkit.ColorCanvas"/>, notifies and updates the BorderBrush of the outermost Border
        /// </summary>
        public Color LightsColor
        {
            get => lightsColor;
            set
            {
                lightsColor = value;
                BorderLightIndicator = new SolidColorBrush(lightsColor); 
                NotifyOfPropertyChange();
                NotifyOfPropertyChange("BorderLightIndicator");
            }
        }

		/// <summary>
		/// Private property and not bound to anything. This boolean is responsible for changing and notifying the 
		/// image (<see cref="PlayPauseImage"/>) used in the play/pause media control in the <see cref="DJView"/>.
		/// </summary>
		private bool IsPlaying
        {
            get => isPlaying;
            set
            {
                isPlaying = value;
                PlayPauseImage = isPlaying ? PAUSE_IMAGE : PLAY_IMAGE;
                NotifyOfPropertyChange("PlayPauseImage");
            }
        }
        
        /// <summary>
        /// Bound to <see cref="MediaElement.Source"/>
        /// </summary>
        public Uri CurrentAudioSource
        {
            get => currentAudioSource;
            set
            {
                currentAudioSource = value;
                NotifyOfPropertyChange();
            }
        }

        public Uri PlayPauseImage { get; set; } = PLAY_IMAGE;
        public Brush BorderLightIndicator { get; set; }
		#endregion
		public DJViewModel(IEventAggregator eventAggregator)
        {
            // Instatiate EventAggregators and subscribe on the UI Thread
			eventAggregator.SubscribeOnUIThread(this);
			_djEvents = new EventAggregator();
			_localThreadEvents = new EventAggregator();
			_djEvents.SubscribeOnUIThread(this);
			_localThreadEvents.SubscribeOnUIThread(this);

			SongQueue = new BindableCollection<SongModel>();
            PopulateUtility.AddSongs(AddToQueue);

            // Set the border color
            BorderLightIndicator = new SolidColorBrush(lightsColor);

            // shuffle the song queue (Blank Space first every time is annoying ( sick song tho >:] ))
            SongQueue = new BindableCollection<SongModel>(SongQueue.OrderBy(_ => Guid.NewGuid()).ToList());
			UpdateSongs();

			currentAudioSource = SongQueue[0].AudioPath;
            effectsButtonColor = EFFECTS_DISABLED;
            bpm = SongQueue[0].BPM;
        }

        public void OnViewLoaded(DJView view)
        {
            // Grab the MediaElement control from the view
            if (!initialLoad)
            {
                _audioPlayer = view.AudioPlayer;
                initialLoad = true;
            }
		}

        #region Sort Buttons
        // The sorting is done as follows:
        //   - Create 2 lists
        //      - For list 1: Add all songs with the specified genre
        //      - For list 2: Add the rest of the songs as they appear in the original SongQueue
        public void SortByGenre(Button sender)
        {
            string xName = sender.Name;
            GenreTypes sortGenre = GenreTypes.Rock;

            if (xName.Equals("popButton")) { sortGenre = GenreTypes.Pop; }
            else if (xName.Equals("danceButton")) { sortGenre = GenreTypes.Dance; }

            List<SongModel> genreSongs = (from song in SongQueue
                                          where song.Genre.Equals(sortGenre) && song.RowID != "⏵"
                                          select song).ToList();
            List<SongModel> restOfSongs = (from song in SongQueue
                                           where !song.Genre.Equals(sortGenre) && song.RowID != "⏵"
                                           select song).ToList();
            genreSongs.Insert(0, SongQueue[0]);
            genreSongs.AddRange(restOfSongs);
            SongQueue.Clear();
            SongQueue.AddRange(genreSongs);
            UpdateSongs();
        }

        public void SortBySpeed(Button sender)
        {
            string xName = sender.Name;
            SpeedTypes sortSpeed = xName.Equals("slowButton") ? SpeedTypes.Slow : SpeedTypes.Fast;

            List<SongModel> sorted = sortSpeed == SpeedTypes.Slow ?
                                    (from song in SongQueue
                                     where song.RowID != "⏵"
                                     orderby song.BPM ascending
                                     select song).ToList()
                                     :
                                     (from song in SongQueue
                                      where song.RowID != "⏵"
                                      orderby song.BPM descending
                                      select song).ToList();

            SongModel nowPlaying = SongQueue[0];
            SongQueue.Clear();
            SongQueue.Add(nowPlaying);
            SongQueue.AddRange(sorted);
            UpdateSongs();
        }
        #endregion
        #region Drap & Drop Logic
        public void DragOver(IDropInfo dropInfo)
        {
            dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
            dropInfo.Effects = DragDropEffects.Move;
        }

        public void Drop(IDropInfo dropInfo)
        {
            SongModel sourceSong = (SongModel)dropInfo.Data;
            SongModel targetSong = (SongModel)dropInfo.TargetItem;

            int sourceId = sourceSong.RowID == "⏵" ? 0 : int.Parse(sourceSong.RowID) - 1;
            int targetId = targetSong.RowID == "⏵" ? 0 : int.Parse(targetSong.RowID) - 1;

            SongQueue.Move(sourceId, targetId);
            UpdateSongs();
        }
		#endregion
		#region Song Queue Manipulation

		/// <summary>
		/// Ensures dynamic feedback to the user when the <see cref="SongQueue"/> is manipulated
		/// 
		/// <para>
		///     The changes made are the following:
		/// <list type="bullet"> 
		///     <item>
		///         <see cref="SongQueue"/> RowIDs are re-initialized
		///     </item>
		///     <item>
		///         After the songs are re-ordered and if <see cref="KaraokeViewModel"/> is open, a message is sent to the ViewModel
        ///         containing the currently playing song. This allows <see cref="KaraokeViewModel"/> to change the lyrics displayed
 		///     </item>
        ///     <item>
        ///         The <see cref="Bpm"/> property (Binding with BPM Slider) is updated 
        ///     </item>
        ///     <item>
        ///         The <see cref="MediaElement.Source"/> is updated
        ///     </item>
		/// </list>
		/// </para>
		/// </summary>
		private void UpdateSongs()
        {
            SongQueue[0].RowID = "⏵";
            for (int i = 1; i < SongQueue.Count; i++)
            {
                SongQueue[i].RowID = (i + 1).ToString();
            }
            SongQueue.Refresh();

            // Send the current song to KaraokeViewModel (if open)
            if (KaraokeOpen)
            {
                _djEvents.PublishOnUIThreadAsync(SongQueue[0]);
            }

            Bpm = SongQueue[0].BPM;
            CurrentAudioSource = SongQueue[0].AudioPath;
        }

        // Use this instead of SongQueue.Add()!
        /// <summary>
        /// Wrapper method for <see cref="List{T}.Add(T)"/>
        /// </summary>
        private void AddToQueue(string artistName, string title, GenreTypes genre, int bpm, string lyrics, Uri? audioPath = null)
        {
            int id = SongQueue.Count + 1;
            SongModel song = new SongModel()
            {
                RowID = SongQueue.Count == 0 ? "⏵" : id.ToString(),
                ArtistName = artistName,
                Title = title,
                Genre = genre,
                BPM = bpm,
                Lyrics = lyrics
            };

            if (audioPath is not null)
            {
                song.AudioPath = audioPath;
            }

            SongQueue.Add(song);
        }

        #endregion
        #region Karaoke 
        /// <summary>
        /// Open the karaoke window using the <see cref="Caliburn.Micro"/> <see cref="WindowManager"/>
        /// </summary>
        public void OpenKaraoke()
        {
            if (!karaokeOpen)
            {
                IWindowManager manager = new WindowManager();
                manager.ShowWindowAsync(new KaraokeViewModel(SongQueue[0], _djEvents));
                KaraokeOpen = true;
                EffectsButtonColor = EFFECTS_OFF;
            }
        }

        /// <summary>
        /// [OBSOLETE, IMPLEMENT IN XAML] On button Click, change the background of the button
        /// </summary>
        public void AddVoiceEffects()
        {
            if (effectsButtonColor is null || effectsButtonColor == EFFECTS_OFF)
            {
                EffectsButtonColor = EFFECTS_ON;
            }
            else
            {
                EffectsButtonColor = EFFECTS_OFF;
            }
        }
        #endregion
        #region Media Controls

        /// <summary>
        /// Essentially a wrapper method for <see cref="MediaElement.Play()"/> and <see cref="MediaElement.Pause()"/>.
        /// 
        /// <para>
        ///     Along with handling the pausing/playing of the audio player, it sets other local ViewModel settings for 
        ///     reasons such as bug prevention and retaining the UI
        /// </para>
        /// </summary>
        /// <returns></returns>
        public async Task PlayPause()
        {
            IsPlaying = !IsPlaying;
            if (IsPlaying)
            {
                PauseAvailable = false || hasMediaOpened;
                _audioPlayer.Play();

                if (hasMediaOpened)
                {
                    cancelSeekerPositionUpdate = new CancellationTokenSource();
                    seekerCancelToken = cancelSeekerPositionUpdate.Token;
                    await StartSeekerPositionUpdateAsync(seekerCancelToken);
                }
            }
            else
            {
                _audioPlayer.Pause();
                cancelSeekerPositionUpdate.Cancel();
            }
        }

        /// <summary>
        /// Removes the top song (currently playing) and puts it to the end of the queue
        /// </summary>
        public void NextInQueue()
        {
            SongModel justFinished = SongQueue[0];
            SongQueue.RemoveAt(0);
            SongQueue.Add(justFinished);
            _audioPlayer.SpeedRatio = 1;
            UpdateSongs();
        }

        /// <summary>
        /// Removes the top song (currently playing) and puts it second in queue
        /// </summary>
        public void PreviousInQueue()
        {
            int lastIndex = SongQueue.Count - 1;
            SongModel lastSong = SongQueue[lastIndex];
            SongQueue.RemoveAt(lastIndex);
            SongQueue.Insert(0, lastSong);
            _audioPlayer.SpeedRatio = 1;
            UpdateSongs();
        }

        /// <summary>
        /// Is called when the event is fired from the audio player <see cref="MediaElement"/> 
        /// (i.e. when the source (audio) finishes loading and is ready to play)
        /// <para>
        ///     The method updates properties related to song duration and seeker Maximum value
        /// </para>
        /// </summary>
        /// <returns></returns>
        public async Task OnSongChanged()
        {
            CurrentSongDuration = _audioPlayer.NaturalDuration.TimeSpan.ToString("mm':'ss");
            CurrentSeekerMaximum = (int)_audioPlayer.NaturalDuration.TimeSpan.TotalSeconds;

            cancelSeekerPositionUpdate = new CancellationTokenSource();
            seekerCancelToken = cancelSeekerPositionUpdate.Token;
            hasMediaOpened = true;
            PauseAvailable = true;
            await StartSeekerPositionUpdateAsync(seekerCancelToken);
        }

        /// <summary>
        /// Changes the <see cref="MediaElement"/>'s SpeedRatio property, the audio is updated automatically
        /// </summary>
        public void ChangeBPM()
        {
            _audioPlayer.SpeedRatio = Bpm / SongQueue[0].BPM;
        }

        /// <summary>
        /// Is called when the ValueChanged event is fired on the seeker
        /// <para>
        ///     Here, we are only interested in the case that the value changes WHEN the user is dragging the seeker thumb!
        ///     If that is the case then we update the song time display (mm:ss) to reflect the seeker's current position
        /// </para>
        /// </summary>
        /// <param name="source">The seeker control, so that we can grab its Value property</param>
        public void OnSeekerValueChanged(Slider source)
        {
            if (userSeeking)
            {
                CurrentSongTime = TimeSpan.FromSeconds(source.Value).ToString("mm':'ss");
            }
        }

		/// <summary>
		/// This method is called when the user starts seeking and dragging the seeker's thumb.
		/// When MouseDown is detected on the seeker, call this method
		/// 
		/// <para>
		///     The method tells the async method <see cref="StartSeekerPositionUpdateAsync(CancellationToken)"/> to stop firing periodically by cancelling
        ///     the <see cref="CancellationTokenSource"/> associated with it.
		/// </para>
		/// </summary>
		public void OnUserSeeking()
        {
            if (hasMediaOpened)
            {
                // Stop the seeker's value from changing
                cancelSeekerPositionUpdate.Cancel();
                userSeeking = true;
            }
        }

		/// <summary>
		/// When the user manually changes the seeker's position, call this method.
		/// This method is called when the MouseUp Event is fired on the Seeker Slider control.
		/// </summary>
		public async void OnUserSeekerValueChanged()
		{
			userSeeking = false;
			_audioPlayer.Position = TimeSpan.FromSeconds(CurrentSongElapsedSeconds);
			cancelSeekerPositionUpdate = new CancellationTokenSource();
			seekerCancelToken = cancelSeekerPositionUpdate.Token;
			await StartSeekerPositionUpdateAsync(seekerCancelToken);
		}

		/// <summary>
		/// Periodically update send messages indicating the seeker's current positions
		/// to the UI Thread.
		/// </summary>
		private async Task StartSeekerPositionUpdateAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (_audioPlayer.NaturalDuration.HasTimeSpan
                       && _audioPlayer.Position.TotalSeconds <= _audioPlayer.NaturalDuration.TimeSpan.TotalSeconds)
                {
                    Tuple<string, double> message = Tuple.Create(_audioPlayer.Position.ToString("mm':'ss"),
                                                                _audioPlayer.Position.TotalSeconds);
                    // Ensure the properties values are changed on the UI Thread!
                    // This is why we don't change the properties from here and rather send a message to the UIThread
                    await _localThreadEvents.PublishOnUIThreadAsync(message);
                    await Task.Delay(500, cancellationToken);
                }
            } 
            catch (TaskCanceledException) { }
        }
        #endregion
		#region EventAggregator Handlers (Implemented from IHandle<TMessage>)

        /// <summary>
        /// Handles messages sent from <see cref="KaraokeViewModel"/>
        /// </summary>
		public Task HandleAsync(int message, CancellationToken cancellationToken)
        {
            KaraokeOpen = false;
            EffectsButtonColor = EFFECTS_DISABLED;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Handles messages sent from the main <see cref="IEventAggregator"/>.
        /// <para>
        ///     This handler is concerned with stopping/starting the <see cref="MediaElement"/> audio player 
        ///     when the user clicks the "Previous Window" button. Without handling, the audio would continue playing on another thread.
        /// </para>
        /// </summary>
        public async Task HandleAsync(string message, CancellationToken cancellationToken)
        {
            if (message == "DJ Exiting!")
            {
                if (IsPlaying)
                {
                    exitWhilePlaying = true;
                    await PlayPause();
                }
                else
                {
                    exitWhilePlaying = false;
                }
            }
            if (message == "DJ Opening!")
            {
                if (exitWhilePlaying)
                {
                    ChangeBPM();
                    await PlayPause();
                }
            }
        }

        /// <summary>
        /// Handles song times sent from the UpdateSeekerPositionAsync thread!
        /// </summary>
        public Task HandleAsync(Tuple<string, double> message, CancellationToken cancellationToken)
        {
            CurrentSongTime = message.Item1;
            CurrentSongElapsedSeconds = message.Item2;
            return Task.CompletedTask;
        }

        #endregion
    }
}
