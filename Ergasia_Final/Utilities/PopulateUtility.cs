using Caliburn.Micro;
using Ergasia_Final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Provider;
using System.Windows.Media.Imaging;

namespace Ergasia_Final.Utilities
{
    public static class PopulateUtility
    {
		/// <summary>
		/// Adds <see cref="SongModel"/> objects using the <paramref name="addMethod"/> delegate given.
 		/// </summary>
		public static void AddSongs(Action<string, string, GenreTypes, int, string, Uri> addMethod)
		{
			addMethod("Taylor Swift", "Blank Space", GenreTypes.Pop, 96,
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

			addMethod("alt-J", "Fitzpleasure", GenreTypes.Rock, 144,
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

			addMethod("Daft Punk", "Digital Love", GenreTypes.Dance, 120,
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

			addMethod("M83", "Midnight City", GenreTypes.Dance, 105,
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

			addMethod("Taylor Swift", "Don't Blame Me", GenreTypes.Pop, 136,
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

			addMethod("Arctic Monkeys", "Knee Socks", GenreTypes.Rock, 98,
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

			addMethod("alt-J", "Breezeblocks", GenreTypes.Rock, 150, 
				@"She may contain the urge to run away
But hold her down with soggy clothes and breezeblocks
Citrezene, your fevers gripped me again
Never kisses, all you ever send are fullstops
(La la la la)

Do you know where the wild things go?
They go along to take your honey
(La la la la)
Break down, now weep
Build up breakfast, now let's eat
My love, my love, love, love
(La la la la)

Muscle to muscle and toe to toe
The fear has gripped me but here I go
My heart sinks as I jump up
Your hand grips hand as my eyes shut

Do you know where the wild things go?
They go along to take our honey
(La la la la)
Break down, now sleep
Build up breakfast now let's eat
My love, my love, love, love

She bruises, coughs, she splutters pistol shots
Hold her down with soggy clothes and breezeblocks
(La la la la)
She's morphine, queen of my vaccine
My love, my love, love, love
(La la la la)

Muscle to muscle and toe to toe
The fear has gripped me but here I go
My heart sinks as I jump up
Your hand grips hand as my eyes shut

She may contain the urge to runaway
But hold her down with soggy clothes and breezeblocks
Germaline, disinfect the scene
My love, my love, love, love
Please don't go
I love you so
My lovely

Please don't go, please don't go
I love you so, I love you so
Please don't go, please don't go
I love you so, I love you so
Please break my heart (hey)

Please don't go, please don't go
I love you so, I love you so
Please don't go, please don't go
I love you so, I love you so
Please break my heart

Please don't go
(Please don't go) I'll eat you whole
(I'll eat you whole) I love you so
(I love you so) I love you so, I love you so
(I love you so, I love you so) please don't go
(Please don't go) I'll eat you whole
(I'll eat you whole) I love you so
(I love you so)

(I love you so, I love you so) please don't go
(Please don't go) I'll eat you whole
(I'll eat you whole) I love you so
(I love you so) I love you so, I love you so
(I love you so, I love you so) please don't go
(Please don't go) I'll eat you whole
(I'll eat you whole) I love you so
(I love you so)

(I love you so, I love you so) please don't go
(Please don't go) I'll eat you whole
(I'll eat you whole) I love you so
(I love you so) I love you so, I love you so
(I love you so, I love you so) please don't go
(Please don't go) I'll eat you whole
(I'll eat you whole) I love you so
(I love you so)

(I love you so, I love you so) please don't go
I'll eat you whole
I love you so
I love you so, I love you so
Please don't go
I'll eat you whole
I love you so
I love you so, I love you so", new Uri("./Audio/altj_breezeblocks.mp3", UriKind.RelativeOrAbsolute));
		}

		/// <summary>
		/// Adds <see cref="ArtistModel"/> objects to a <see cref="List{T}"/>
		/// </summary>
		/// <param name="artistList"></param>
		public static void AddArtists(List<ArtistModel> artistList)
		{
			artistList.Add(new ArtistModel
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
				AlbumName = "Lover",
				SongPath = new Uri("./Audio/ts_cruelSummer.mp3", UriKind.RelativeOrAbsolute)
			});

			artistList.Add(new ArtistModel
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
				AlbumName = "Hurry up, We're Dreaming",
				SongPath = new Uri("./Audio/m83_midnightCity.mp3", UriKind.RelativeOrAbsolute)
			});

			artistList.Add(new ArtistModel
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
				AlbumName = "An Awesome Wave",
				SongPath = new Uri("./Audio/altj_breezeblocks.mp3", UriKind.RelativeOrAbsolute)
			});
		}

		public static void AddCatalogueItems(List<CatalogueItemModel> catalogueItems)
		{
			catalogueItems.Add(new CatalogueItemModel()
			{
				Name = "Freddo Espresso",
				Type = CatalogueItemTypes.Coffee,
				Price = 2.5
			});
			catalogueItems.Add(new CatalogueItemModel()
			{
				Name = "Freddo Cappuccino",
				Type = CatalogueItemTypes.Coffee,
				Price = 2.8
			});
			catalogueItems.Add(new CatalogueItemModel()
			{
				Name = "Espresso",
				Type = CatalogueItemTypes.Coffee,
				Price = 1.8
			});
			catalogueItems.Add(new CatalogueItemModel()
			{
				Name = "Cappuccino",
				Type = CatalogueItemTypes.Coffee,
				Price = 2.2
			});
			catalogueItems.Add(new CatalogueItemModel()
			{
				Name = "Macchiato",
				Type = CatalogueItemTypes.Coffee,
				Price = 2,
				OptionalDescription = "Espresso with foam milk on top."
			});
			catalogueItems.Add(new CatalogueItemModel()
			{
				Name = "Latte",
				Type = CatalogueItemTypes.Coffee,
				Price = 2.5
			});

			catalogueItems.Add(new CatalogueItemModel()
			{
				Name = "Green Tea",
				Type = CatalogueItemTypes.Tea,
				Price = 3
			});
			catalogueItems.Add(new CatalogueItemModel()
			{
				Name = "Black Tea",
				Type = CatalogueItemTypes.Tea,
				Price = 3
			});
			catalogueItems.Add(new CatalogueItemModel()
			{
				Name = "Oolong Tea",
				Type = CatalogueItemTypes.Tea,
				Price = 3.2
			});
			catalogueItems.Add(new CatalogueItemModel()
			{
				Name = "Ice Tea",
				Type = CatalogueItemTypes.Tea,
				Price = 2.5,
				OptionalDescription = "Choose between: Peach, Lemon, Berry, Watermelon."
			});

			catalogueItems.Add(new CatalogueItemModel()
			{
				Name = "Water",
				Type = CatalogueItemTypes.Beverages,
				Price = 0.5
			}); 
			catalogueItems.Add(new CatalogueItemModel()
			{
				Name = "Coca Cola",
				Type = CatalogueItemTypes.Beverages,
				Price = 2,
				OptionalDescription = "Choose between: Normal, Light, Zero"
			});
			catalogueItems.Add(new CatalogueItemModel()
			{
				Name = "Sprite",
				Type = CatalogueItemTypes.Beverages,
				Price = 2
			});
			catalogueItems.Add(new CatalogueItemModel()
			{
				Name = "Fanta",
				Type = CatalogueItemTypes.Beverages,
				Price = 2,
				OptionalDescription = "Choose between: Carbonated, Not Carbonated"
			});
			catalogueItems.Add(new CatalogueItemModel()
			{
				Name = "Fanta Lemon",
				Type = CatalogueItemTypes.Beverages,
				Price = 2
			});

			catalogueItems.Add(new CatalogueItemModel()
			{
				Name = "Chips",
				Type = CatalogueItemTypes.Snacks,
				Price = 3.6,
				OptionalDescription = "Choose between: Salted, Oregano, Barbeque, Sour Cream"
			});
			catalogueItems.Add(new CatalogueItemModel()
			{
				Name = "Doritos",
				Type = CatalogueItemTypes.Snacks,
				Price = 3.5,
			});
			catalogueItems.Add(new CatalogueItemModel()
			{
				Name = "Sandwich",
				Type = CatalogueItemTypes.Snacks,
				Price = 4,
				AllergyWarning = true
			});
			catalogueItems.Add(new CatalogueItemModel()
			{
				Name = "Club Sandwich",
				Type = CatalogueItemTypes.Snacks,
				Price = 6.7,
				AllergyWarning = true
			});

			catalogueItems.Add(new CatalogueItemModel()
			{
				Name = "Butter Croissant",
				Type = CatalogueItemTypes.Sweets,
				Price = 2,
				AllergyWarning = true
			});
			catalogueItems.Add(new CatalogueItemModel()
			{
				Name = "Chocolate Croissant",
				Type = CatalogueItemTypes.Sweets,
				Price = 2.4,
				AllergyWarning = true
			});
			catalogueItems.Add(new CatalogueItemModel()
			{
				Name = "Pancakes",
				Type = CatalogueItemTypes.Sweets,
				Price = 5.5,
				OptionalDescription = "Choose syrup: Chocolate, Sour Cherry, Strawberry, Bueno"
			});
			catalogueItems.Add(new CatalogueItemModel()
			{
				Name = "Ice Cream",
				Type = CatalogueItemTypes.Sweets,
				Price = 3.5,
				OptionalDescription = "1 serving (ball): Chocolate, Vanilla, Strawberry, Lila Pause, Bitter Chocolate, Kaimaki"
			});

			catalogueItems.Add(new CatalogueItemModel()
			{
				Name = "Gin Tonic",
				Type = CatalogueItemTypes.Drinks,
				Price = 6.0,
				OptionalDescription = "Gordon's Gin distilled with tonic water"
			});
			catalogueItems.Add(new CatalogueItemModel()
			{
				Name = "Manhattan",
				Type = CatalogueItemTypes.Drinks,
				Price = 7.5,
				OptionalDescription = "Bourbon with sweet vermouth and bitters"
			});
			catalogueItems.Add(new CatalogueItemModel()
			{
				Name = "Margarita",
				Type = CatalogueItemTypes.Drinks,
				Price = 8.0,
				OptionalDescription = "Tequila with triple sec and fresh lime juice"
			});
			catalogueItems.Add(new CatalogueItemModel()
			{
				Name = "Mai Tai",
				Type = CatalogueItemTypes.Drinks,
				Price = 7.5,
				OptionalDescription = "Rum with orange curaçao, fresh lime juice and orgeat syrup"
			});
			catalogueItems.Add(new CatalogueItemModel()
			{
				Name = "Aperol Spritz",
				Type = CatalogueItemTypes.Drinks,
				Price = 5.5
			});

			catalogueItems.ForEach(item => SetCatalogueItemAvailability(item));
		}

		private static void SetCatalogueItemAvailability(CatalogueItemModel item)
		{
			if (item.Type == CatalogueItemTypes.Snacks || item.Type == CatalogueItemTypes.Sweets)
			{
				item.CurrentlyAvailable = DateTime.Now.Hour >= 10 && DateTime.Now.Hour <= 20;
			}

			else if (item.Type == CatalogueItemTypes.Drinks)
			{
				item.CurrentlyAvailable = DateTime.Now.Hour >= 18 && DateTime.Now.Hour <= 24;
			}

			else
			{
				item.CurrentlyAvailable = true;
			}
		}
	}
}
