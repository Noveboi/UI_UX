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
        private readonly IEventAggregator _shellEvents;
        private readonly IEventAggregator _localThreadEvents;
        private bool karaokeOpen = false;
        private Brush effectsButtonColor;
        private static readonly Brush EFFECTS_DISABLED = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#666666"));
        private static readonly Brush EFFECTS_OFF = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#a01039"));
        private static readonly Brush EFFECTS_ON = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#13a51d"));
        private static readonly Uri PLAY_IMAGE = new Uri("/Images/play.png", UriKind.Relative);
        private static readonly Uri PAUSE_IMAGE = new Uri("/Images/pause.png", UriKind.Relative);
        private Color lightsColor;
        private Uri playPauseImage = PLAY_IMAGE;
        private MediaElement _audioPlayer;
        private Uri currentAudioSource;
        private bool isPlaying = false;
        private bool hasMediaOpened = false;
        private bool hasFinishedLoading = true;
        private bool initialLoad = false;
        private bool userSeeking = false;
        private bool exitWhilePlaying = false;
        private TimeSpan savedPosition = TimeSpan.MaxValue;

        private int currentSeekerMaximum;
        private string currentSongDuration;
        private string currentSongTime = "00:00";
        private double currentSongTimeSpan = 0.0;

        private CancellationTokenSource cancelSeekerPositionUpdate;
        private CancellationToken seekerCancelToken;

		public bool PauseAvailable
        {
            get => hasFinishedLoading;
            set
            {
                hasFinishedLoading = value;
                NotifyOfPropertyChange();
            }
        }
        public int CurrentSeekerMaximum
        {
            get => currentSeekerMaximum;
            set
            {
                currentSeekerMaximum = value;
                NotifyOfPropertyChange();
            }
        }

        public double CurrentSongTimeSpan
        {
            get => currentSongTimeSpan;
            set
            {
                currentSongTimeSpan = value;
                NotifyOfPropertyChange();
            }
        }

        public string CurrentSongDuration
        {
            get => currentSongDuration;
            set
            {
                currentSongDuration = value;
                NotifyOfPropertyChange();
            }
        }

        public string CurrentSongTime
        {
            get => currentSongTime;
            set
            {
                currentSongTime = value;
                NotifyOfPropertyChange();
            }
        }

        public double Bpm
        {
            get => bpm;
            set
            {
                bpm = Math.Round(value);
                NotifyOfPropertyChange();
            }
        }
        public BindableCollection<SongModel> SongQueue { get; }
        public bool KaraokeOpen
        {
            get => karaokeOpen;
            set
            {
                karaokeOpen = value;
                NotifyOfPropertyChange();
            }
        }

        public Brush EffectsButtonColor
        {
            get => effectsButtonColor;
            set
            {
                effectsButtonColor = value;
                NotifyOfPropertyChange();
            }
        }

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

        public bool IsPlaying
        {
            get => isPlaying;
            set
            {
                isPlaying = value;
                PlayPauseImage = isPlaying ? PAUSE_IMAGE : PLAY_IMAGE;
            }
        }
        public Uri PlayPauseImage
        {
            get => playPauseImage;
            set
            {
                playPauseImage = value;
                NotifyOfPropertyChange();
            }
        }

        public Uri CurrentAudioSource
        {
            get => currentAudioSource;
            set
            {
                currentAudioSource = value;
                NotifyOfPropertyChange();
            }
        }

        public Brush BorderLightIndicator { get; set; }
        #endregion
        public DJViewModel(IEventAggregator eventAggregator)
        {
			eventAggregator.SubscribeOnUIThread(this);
			_djEvents = new EventAggregator();
			_localThreadEvents = new EventAggregator();
			_djEvents.SubscribeOnUIThread(this);
			_localThreadEvents.SubscribeOnUIThread(this);

            LightsColor = (Color)ColorConverter.ConvertFromString("#cc1188");

			SongQueue = new BindableCollection<SongModel>();
            PopulateUtility.AddSongs(AddToQueue);

            // shuffle the song queue (Blank Space first every time is annoying ( sick song tho >:] ))
            SongQueue = new BindableCollection<SongModel>(SongQueue.OrderBy(_ => Guid.NewGuid()).ToList());
			UpdateSongs();

			currentAudioSource = SongQueue[0].AudioPath;

            effectsButtonColor = EFFECTS_DISABLED;
            bpm = SongQueue[0].BPM;
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
        private void UpdateSongs()
        {
            SongQueue[0].RowID = "⏵";
            for (int i = 1; i < SongQueue.Count; i++)
            {
                SongQueue[i].RowID = (i + 1).ToString();
            }
            SongQueue.Refresh();

            // Send the current song to KaraokeViewModel (if open)
            _djEvents.PublishOnUIThreadAsync(SongQueue[0]);
            Bpm = SongQueue[0].BPM;
            CurrentAudioSource = SongQueue[0].AudioPath;
        }

        // Use this instead of SongQueue.Add()!
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
        public async Task PlayPause()
        {
            IsPlaying = !IsPlaying;
            if (IsPlaying)
            {
                PauseAvailable = false || hasMediaOpened;
                // Do NOT use _audioPlayer.Play() anywhere else!
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

        public void NextInQueue()
        {
            SongModel justFinished = SongQueue[0];
            SongQueue.RemoveAt(0);
            SongQueue.Add(justFinished);
            _audioPlayer.SpeedRatio = 1;
            UpdateSongs();
        }

        public void PreviousInQueue()
        {
            int lastIndex = SongQueue.Count - 1;
            SongModel lastSong = SongQueue[lastIndex];
            SongQueue.RemoveAt(lastIndex);
            SongQueue.Insert(0, lastSong);
            _audioPlayer.SpeedRatio = 1;
            UpdateSongs();
        }

        // Grab the MediaElement control from the View
        public void OnMediaControlsLoaded(Grid source)
        {
            if (!initialLoad) 
            { 
                _audioPlayer = source.FindName("AudioPlayer") as MediaElement;
                initialLoad = true;
            }
        }

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

        public void ChangeBPM()
        {
            _audioPlayer.SpeedRatio = Bpm / SongQueue[0].BPM;
        }

        /// <summary>
        /// When the user manually changes the seeker's position, fire.
        /// This function fires when the MouseUp Event is fired on the Seeker Slider control.
        /// </summary>
        public async void OnUserSeekerValueChanged()
        {
            userSeeking = false;
            _audioPlayer.Position = TimeSpan.FromSeconds(CurrentSongTimeSpan);
            cancelSeekerPositionUpdate = new CancellationTokenSource();
            seekerCancelToken = cancelSeekerPositionUpdate.Token;
            await StartSeekerPositionUpdateAsync(seekerCancelToken);
        }

        public void OnSeekerValueChanged(Slider source)
        {
            if (userSeeking)
            {
                CurrentSongTime = TimeSpan.FromSeconds(source.Value).ToString("mm':'ss");
            }
        }

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
		public Task HandleAsync(int message, CancellationToken cancellationToken)
        {
            KaraokeOpen = false;
            EffectsButtonColor = EFFECTS_DISABLED;
            return Task.CompletedTask;
        }

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
                savedPosition = _audioPlayer.Position;
            }
            if (message == "DJ Opening!" && savedPosition != TimeSpan.MaxValue)
            {
                if (exitWhilePlaying)
                {
                    ChangeBPM();
                    _audioPlayer.Position = savedPosition;
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
            CurrentSongTimeSpan = message.Item2;
            return Task.CompletedTask;
        }

        #endregion
    }
}
