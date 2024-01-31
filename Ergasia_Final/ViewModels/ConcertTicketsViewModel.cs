using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ergasia_Final.ViewModels
{
    public class ConcertTicketsViewModel : Screen
    {
        private string _test;

        public string Test
        {
            get { return _test; }
            set 
            { 
                _test = value;
                NotifyOfPropertyChange();
            }
        }

        public ConcertTicketsViewModel(string sender)
        {
            _test = sender;
        }
    }
}
