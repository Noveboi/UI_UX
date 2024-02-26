using Caliburn.Micro;
using Ergasia_Final.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

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
        private bool _isCalendarOpen = false;
        private string _dateDisplay = string.Empty;
        private string _price;
        private IEventAggregator _eventAggregator;

        // Properties
        public string Price
        {
            get => _price;
        }
        public bool IsCalendarOpen
        {
            get => _isCalendarOpen;
            set
            {
                _isCalendarOpen = value;
                NotifyOfPropertyChange();
            }
        }
        public string DateDisplay
        {
            get => _dateDisplay;
            set
            {
                _dateDisplay = value;
                NotifyOfPropertyChange();
            }
        }

        // Methods
        public void Pay()
        {
            MessageBox.Show("Payment successful!\nReturning to Main Menu", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
            _eventAggregator.PublishOnUIThreadAsync("epay to main");
        }

        public void OnMonthPicked(MonthPickerSelectionChangedEventArgs e)
        {
            string dateString = $"{e.Month}/{e.Year}";
            DateDisplay = dateString;
            IsCalendarOpen = false;
        }
    }
}
