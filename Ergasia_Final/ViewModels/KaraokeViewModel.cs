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

namespace Ergasia_Final.ViewModels
{
    public class KaraokeViewModel : Screen, IHandle<SongModel>
    {
        private SongModel _nowPlaying;
        private readonly IEventAggregator _djEvents;
        public SongModel NowPlaying
        {
            get => _nowPlaying;
            set
            {
                _nowPlaying = value;
                NotifyOfPropertyChange();
            }
        }
        public KaraokeViewModel(SongModel currentSong, IEventAggregator djEvents)
        {
            _djEvents = djEvents;
            _djEvents.SubscribeOnUIThread(this);

            _nowPlaying = currentSong;
        }

        // Fires when a new song starts playing from DJViewModel
        public async Task HandleAsync(SongModel message, CancellationToken cancellationToken)
        {
            NowPlaying = message;
        }

        public void OnClose()
        {
            _djEvents.PublishOnUIThreadAsync(1);
        }
    }
}
