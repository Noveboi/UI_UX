using Caliburn.Micro;
using Ergasia_Final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Ergasia_Final.ViewModels
{
    public class DJViewModel : Screen
    {
        private double _bpm;

        public double Bpm
        {
            get => _bpm;
            set
            {
                _bpm = Math.Round(value);
                NotifyOfPropertyChange();
            }
        }

        public BindableCollection<SongModel> SongQueue { get; }
        public DJViewModel()
        {
            SongQueue = new BindableCollection<SongModel>();
            AddSongs();

            _bpm = SongQueue[0].BPM;
        }

        public void OnUpArrowClick(SongModel dataContext)
        {

        }

        public void OnDownArrowClick(SongModel dataContext)
        {
        }

        // Use this instead of SongQueue.Add()!
        private void AddToQueue(string artistName, string title, GenreTypes genre, int bpm)
        {
            int id = SongQueue.Count + 1;
            SongQueue.Add(new SongModel()
            {
                RowID = id,
                ArtistName = artistName,
                Title = title,
                Genre = genre,
                BPM = bpm
            });
        }

        public void OnDragDrop()
        {
            // Make stuff
        }

        public void AddSongs()
        {
            AddToQueue("Taylor Swift", "Blank Space", GenreTypes.Pop, 96);
            AddToQueue("alt-J", "Fitzpleasure", GenreTypes.Rock, 144);
            AddToQueue("Daft Punk", "Digital Love", GenreTypes.Dance, 120);
            AddToQueue("M83", "Midnight City", GenreTypes.Dance, 105);
            AddToQueue("Taylor Swift", "Don't Blame Me", GenreTypes.Pop, 136);
            AddToQueue("Arctic Monkeys", "Knee Socks", GenreTypes.Rock, 98);
        }
    }
}
