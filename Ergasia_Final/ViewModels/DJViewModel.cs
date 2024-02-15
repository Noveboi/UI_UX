using Caliburn.Micro;
using Ergasia_Final.Models;
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
        private Color lightsColor = (Color)ColorConverter.ConvertFromString("#8811ff");
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
            SongQueue = new BindableCollection<SongModel>();
            AddSongs();

            currentAudioSource = SongQueue[0].AudioPath;

            eventAggregator.SubscribeOnUIThread(this);
            _djEvents = new EventAggregator();
            _localThreadEvents = new EventAggregator();
            _djEvents.SubscribeOnUIThread(this);
            _localThreadEvents.SubscribeOnUIThread(this);

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

        public void AddSongs()
        {
            AddToQueue("Taylor Swift", "Blank Space", GenreTypes.Pop, 96,
                @"Nice to meet you, where you been?
I could show you incredible things
Magic, madness, heaven, sin
Saw you there and I thought
""Oh, my God, look at that face
You look like my next mistake
Love's a game, wanna play?"" Ay

New money, suit and tie
I can read you like a magazine
Ain't it funny? Rumors fly
And I know you heard about me
So hey, let's be friends
I'm dying to see how this one ends
Grab your passport and my hand
I can make the bad guys good for a weekend

So it's gonna be forever
Or it's gonna go down in flames
You can tell me when it's over, mm
If the high was worth the pain
Got a long list of ex-lovers
They'll tell you I'm insane
'Cause you know I love the players
And you love the game

'Cause we're young, and we're reckless
We'll take this way too far
It'll leave you breathless, mm
Or with a nasty scar
Got a long list of ex-lovers
They'll tell you I'm insane
But I've got a blank space, baby
And I'll write your name

Cherry lips, crystal skies
I could show you incredible things
Stolen kisses, pretty lies
You're the King, baby, I'm your Queen
Find out what you want
Be that girl for a month
Wait, the worst is yet to come, oh, no

Screaming, crying, perfect storms
I can make all the tables turn
Rose garden filled with thorns
Keep you second guessing like
""Oh, my God, who is she?""
I get drunk on jealousy
But you'll come back each time you leave
'Cause, darling, I'm a nightmare dressed like a daydream

So it's gonna be forever
Or it's gonna go down in flames
You can tell me when it's over, mm
If the high was worth the pain
Got a long list of ex-lovers
They'll tell you I'm insane
'Cause you know I love the players
And you love the game

'Cause we're young, and we're reckless (oh)
We'll take this way too far
It'll leave you breathless, mm (oh)
Or with a nasty scar
Got a long list of ex-lovers
They'll tell you I'm insane (insane)
But I've got a blank space, baby
And I'll write your name

Boys only want love if it's torture
Don't say I didn't, say I didn't warn ya
Boys only want love if it's torture
Don't say I didn't, say I didn't warn ya

So it's gonna be forever
Or it's gonna go down in flames
You can tell me when it's over (over)
If the high was worth the pain
Got a long list of ex-lovers
They'll tell you I'm insane (I'm insane)
'Cause you know I love the players
And you love the game

'Cause we're young, and we're reckless
We'll take this way too far (ooh)
It'll leave you breathless, mm
Or with a nasty scar (leave a nasty scar)
Got a long list of ex-lovers
They'll tell you I'm insane
But I've got a blank space, baby
And I'll write your name", new Uri("./Audio/ts_blankSpace.mp3", UriKind.RelativeOrAbsolute));

            AddToQueue("alt-J", "Fitzpleasure", GenreTypes.Rock, 144,
                @"Tra la la tra la tra-ah la
La
Tra la la tra la tra-ah la
La
Tra la la tra la tra-ah la
La
Tra la la tra la tra-ah
In your snatch fitzpleasure
Broom-shaped pleasure

Deep greedy and googling every corner
La tra la tra-ah la
La la la la la la la

Dead in the middle
Of the C-O double M-O-N
Little did I know then
That the Mandela Boys soon become Mandela Men
Tall woman
Pull the pylons down
And wrap them around the necks
Of all the feckless men that queue to be the next

Steepled fingers
Ring la la la la la la la la leaders
Queue jumpers
Rock fist paper scissors, la la la la la la lingered fluffers (the choir)
In your hoof lies the heartland
Where we tent for our treasure, pleasure, leisure
Les yeux, it's all in your eyes
In your snatch fitzpleasure
A broom-shaped pleasure

Deep greedy and googling every corner
Tra la la tra la tra-ah la
La la la la la la la
Ohhhhhhh
Blended by the lights", new Uri("./Audio/altj_fitzpleasure.mp3", UriKind.RelativeOrAbsolute));

            AddToQueue("Daft Punk", "Digital Love", GenreTypes.Dance, 120,
                @"Last night I had a dream about you
In this dream I'm dancing right beside you
And it looked like everyone was having fun
the kind of feeling I've waited so long

Don't stop, come a little closer
As we jam the rhythm gets stronger
There's nothing wrong with just a little, little fun
We were dancing all night long

The time is right
To put my arms around you
You're feeling right
You wrap your arms around too
But suddenly I feel the shining sun
Before I knew it this dream was all gone

Oh, I don't know what to do
About this dream and you
I wish this dream comes true

Oh, I don't know what to do
About this dream and you
We'll make this dream come true

Why don't you play the game?
Why don't you play the game?", new Uri("./Audio/dp_digitalLove.mp3", UriKind.RelativeOrAbsolute));

            AddToQueue("M83", "Midnight City", GenreTypes.Dance, 105,
                @"Waiting in a car
Waiting for a ride in the dark
The night city grows
Look at the horizon glow

Waiting in a car
Waiting for a ride in the dark
Drinking in the lounge
Following the neon signs

Waiting for a word
Looking at the milky skyline
The city is my church (city is my church)
It wraps me in its blinding twilight

Waiting in a car
Waiting for the right time
Waiting in a car
Waiting for the right time

Waiting in a car
Waiting for the right time
Waiting in a car
Waiting for the right time

Waiting in a car
Waiting for the right time", new Uri("./Audio/m83_midnightCity.mp3", UriKind.RelativeOrAbsolute));

            AddToQueue("Taylor Swift", "Don't Blame Me", GenreTypes.Pop, 136,
                @"Don't blame me, love made me crazy
If it doesn't, you ain't doin' it right
Lord, save me, my drug is my baby
I'll be usin' for the rest of my life

I've been breakin' hearts a long time
And, toyin' with them older guys
Just playthings for me to use
Something happened for the first time
In the darkest little paradise
Shakin', pacin', I just need you

For you, I would cross the line
I would waste my time
I would lose my mind
They say, she's gone too far this time

Don't blame me, love made me crazy
If it doesn't, you ain't doin' it right
Lord, save me, my drug is my baby
I'll be usin' for the rest of my life

Don't blame me, love made me crazy
If it doesn't, you ain't doin' it right
Oh, Lord, save me, my drug is my baby
I'll be usin' for the rest of my life

My name is whatever you decide, and
I'm just gonna call you mine
I'm insane, but I'm your baby
(Your baby)
Echoes (echoes), of your name inside my mind
Halo, hiding my obsession
I once was poison ivy, but now I'm your daisy

And baby, for you, I would fall from grace
Just to touch your face
If you walk away
I'd beg you on my knees to stay

Don't blame me, love made me crazy
If it doesn't, you ain't doin' it right
Lord, save me, my drug is my baby
I'll be usin' for the rest of my life (yeah, ooh)

Don't blame me, love made me crazy
If it doesn't, you ain't doin' it right
Oh, Lord, save me, my drug is my baby
I'll be usin' for the rest of my life

I get so high, oh!
Every time you're, every time you're lovin' me
You're lovin' me
Trip of my life, oh!
Every time you're, every time you're touchin' me
You're touchin' me
Every time you're, every time you're lovin' me

Oh, Lord, save me, my drug is my baby
I'll be using for the rest of my life
(Using for the rest of my life, oh!)

Don't blame me, love made me crazy
If it doesn't, you ain't doin' it right
(Doin' it right, no)
Lord, save me, my drug is my baby
I'll be usin' for the rest of my life (oh-oh)

Don't blame me, love made me crazy
If it doesn't, you ain't doin' it right
(You ain't doin' it right)
Oh, Lord, save me, my drug is my baby
I'll be usin' (I'll be usin') for the rest of my life (oh, I'll be usin')

I get so high, oh!
Every time you're, every time you're lovin' me
You're lovin' me
Oh, Lord, save me, my drug is my baby
I'll be usin' for the rest of my life", new Uri("./Audio/ts_dontBlameMe.mp3", UriKind.RelativeOrAbsolute));

            AddToQueue("Arctic Monkeys", "Knee Socks", GenreTypes.Rock, 98,
                @"You got the lights on in the afternoon
And the nights are drawn out long
And you're kissing to cut through the gloom
With a cough-drop-coloured tongue
And you were sitting in the corner with the coats all piled high
And I thought you might be mine
In a small world on an exceptionally rainy Tuesday night
In the right place and time

When the zeros line up on the 24 hour clock
When you know who's calling even though the number is blocked
When you walked around your house wearing my sky blue Lacoste
And your knee socks

Well you cured my January blues
Yeah you made it all alright
I got a feeling I might have lit the very fuse
That you were trying not to light
You were a stranger in my phone book I was acting like I knew
'Cause I had nothing to lose
When the winter's in full swing and your dreams just aren't coming true
Ain't it funny what you'll do

When the zeros line up on the 24 hour clock
When you know who's calling even though the number is blocked
When you walked around your house wearing my sky blue Lacoste
And your knee socks

The late afternoon
The ghost in your room that you always thought didn't approve of you knocking boots
Never stopped you letting me get hold of the sweet spot by the scruff of your
Knee socks

You and me could have been a team
Each had a half of a king and queen seat
Like the beginning of Mean Streets
You could Be My Baby
[4x]

(All the zeros lined up but the number's blocked when you've come undone)

When the zeros line up on the 24 hour clock
When you know who's calling even though the number is blocked
When you walked around your house wearing my sky blue Lacoste
And your knee socks
[2x]", new Uri("./Audio/am_kneeSocks.mp3", UriKind.RelativeOrAbsolute));
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
            // Stop the seeker's value from changing
            cancelSeekerPositionUpdate.Cancel();
            userSeeking = true;
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
    }
}
