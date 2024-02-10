using Caliburn.Micro;
using Ergasia_Final.Models;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Ergasia_Final.ViewModels
{
    public class DJViewModel : Screen, IDropTarget
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
            UpdateRowIDs();
        }

        public void SortBySpeed(Button sender)
        {
            string xName = sender.Name;
            SpeedTypes sortSpeed = xName.Equals("slowButton") ? SpeedTypes.Slow : SpeedTypes.Fast;

            List<SongModel> speedSongs = (from song in SongQueue
                                          where song.Speed.Equals(sortSpeed)
                                          select song).ToList();
            List<SongModel> restOfSongs = (from song in SongQueue
                                           where !song.Speed.Equals(sortSpeed)
                                           select song).ToList();

            speedSongs.AddRange(restOfSongs);
            SongQueue.Clear();
            SongQueue.AddRange(speedSongs);
            UpdateRowIDs();
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

        public void DragOver(IDropInfo dropInfo)
        {
            dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
            dropInfo.Effects = DragDropEffects.Move;
        }

        public void Drop(IDropInfo dropInfo)
        {
            SongModel sourceSong = (SongModel)dropInfo.Data;
            SongModel targetSong = (SongModel)dropInfo.TargetItem;
            SongQueue.Move(sourceSong.RowID - 1, targetSong.RowID - 1);
            UpdateRowIDs();
        }

        private void UpdateRowIDs()
        {
            for (int i = 0; i < SongQueue.Count; i++)
            {
                SongQueue[i].RowID = i + 1;
            }
            SongQueue.Refresh();
        }
    }
}
