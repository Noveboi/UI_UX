using Caliburn.Micro;
using Ergasia_Final.Models;
using Ergasia_Final.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Ergasia_Final.ViewModels
{
    public class KaraokeViewModel : Screen, IHandle<SongModel>
    {
        private SongModel _nowPlaying;
        private double ratingsAverage = 0;
        private int ratingsCount = 0;
        private readonly IEventAggregator _djEvents;
        private string rateText;

        public SongModel NowPlaying
        {
            get => _nowPlaying;
            set
            {
                _nowPlaying = value;
                NotifyOfPropertyChange();
            }
        }
        public BindableCollection<double> Ratings { get; set; }
        public double RatingsAverage
        {
            get => ratingsAverage;
            set
            {
                ratingsAverage = Math.Round(value, 3);
                NotifyOfPropertyChange();
            }
        }
        public int RatingsCount
        {
            get => ratingsCount;
            set
            {
                ratingsCount = value;
                NotifyOfPropertyChange();
            }
        }

        public KaraokeViewModel(SongModel currentSong, IEventAggregator djEvents)
        {
            _djEvents = djEvents;
            _djEvents.SubscribeOnUIThread(this);

            Ratings = new BindableCollection<double>();

            _nowPlaying = currentSong;
        }

        public void AddRating(double rating)
        {
            Ratings.Add(rating);
            RatingsAverage = Ratings.Sum() / Ratings.Count;
            RatingsCount = Ratings.Count;
        }


        // Fires when a new song starts playing from DJViewModel
        public async Task HandleAsync(SongModel message, CancellationToken cancellationToken)
        {
            NowPlaying = message;
            Ratings.Clear();
            RatingsAverage = 0;
            RatingsCount = 0;
        }

        public void OnRateButtonClick(Button source)
        {
            int rating = int.Parse(source.Content.ToString());

            AddRating(rating);
        }


		public void OnClose()
        {
            _djEvents.PublishOnUIThreadAsync(1);
        }
    }
}
