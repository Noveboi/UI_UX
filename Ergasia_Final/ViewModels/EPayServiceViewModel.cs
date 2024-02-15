using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ergasia_Final.ViewModels
{
    public class EPayServiceViewModel : Screen
    {
        public EPayServiceViewModel(double price, IEventAggregator events)
        {
            _eventAggregator = events;
            _price = $"€{price}";
        }

        // Fields
        private string _price;
        private IEventAggregator _eventAggregator;

        // Properties
        public string Price
        {
            get => _price;
        }

        // Methods
        public void Pay()
        {
            MessageBox.Show("Payment successful!\nReturning to Main Menu", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
            _eventAggregator.PublishOnUIThreadAsync("epay to main");
        }
    }
}
