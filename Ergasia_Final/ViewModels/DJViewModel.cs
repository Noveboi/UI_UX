using Caliburn.Micro;
using Ergasia_Final.Models;
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

namespace Ergasia_Final.ViewModels
{
    public class DJViewModel : Screen, IDropTarget, IHandle<int>
    {
        #region Fields & Properties
        private double bpm;
        /// <summary>
        /// EventAggregator used to send updates to the <see cref="KaraokeViewModel"/> when a new song plays
        /// </summary>
        private IEventAggregator _djEvents;
        private bool karaokeOpen = false;
        private Brush effectsButtonColor;
        private Brush _effectsDisabled = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#666666"));
        private Brush _effectsOff = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#a01039"));
        private Brush _effectsOn = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#13a51d"));
        private Color lightsColor = (Color)ColorConverter.ConvertFromString("#8811ff");
        private Brush borderLightIndicator;
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

        public Brush BorderLightIndicator { get; set; }
        #endregion
        public DJViewModel()
        {
            SongQueue = new BindableCollection<SongModel>();
            _djEvents = new EventAggregator();
            _djEvents.SubscribeOnUIThread(this);
            AddSongs();

            effectsButtonColor = _effectsDisabled;

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
                                          where song.Genre.Equals(sortGenre)
                                          select song).ToList();
            List<SongModel> restOfSongs = (from song in SongQueue
                                           where !song.Genre.Equals(sortGenre)
                                           select song).ToList();

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
                                     orderby song.BPM ascending
                                     select song).ToList()
                                     :
                                     (from song in SongQueue
                                      orderby song.BPM descending
                                      select song).ToList();

            SongQueue.Clear();
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
        }

        // Use this instead of SongQueue.Add()!
        private void AddToQueue(string artistName, string title, GenreTypes genre, int bpm, string lyrics)
        {
            int id = SongQueue.Count + 1;
            SongQueue.Add(new SongModel()
            {
                RowID = SongQueue.Count == 0 ? "⏵" : id.ToString(),
                ArtistName = artistName,
                Title = title,
                Genre = genre,
                BPM = bpm,
                Lyrics = lyrics
            });
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
And I'll write your name");

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
Blended by the lights");

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
Why don't you play the game?");

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
Waiting for the right time");

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
I'll be usin' for the rest of my life");

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
[2x]");
        }
        #endregion
        public void OpenKaraoke()
        {
            if (!karaokeOpen)
            {
                IWindowManager manager = new WindowManager();
                manager.ShowWindowAsync(new KaraokeViewModel(SongQueue[0], _djEvents));
                KaraokeOpen = true;
                EffectsButtonColor = _effectsOff;
            }
        }

        public void AddVoiceEffects()
        {
            if (effectsButtonColor is null || effectsButtonColor == _effectsOff)
            {
                EffectsButtonColor = _effectsOn;
            }
            else
            {
                EffectsButtonColor = _effectsOff;
            }
        }

        // Simulate the backup of the current setup of the DJ App (song queue state, bpm setting)
        public void OnKeyDown() 
        {
            
        }

        public async Task HandleAsync(int message, CancellationToken cancellationToken)
        {
            KaraokeOpen = false;
            EffectsButtonColor = _effectsDisabled;
        }
    }
}
