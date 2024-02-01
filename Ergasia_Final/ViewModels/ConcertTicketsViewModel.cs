using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Text.RegularExpressions;

namespace Ergasia_Final.ViewModels
{
    public class ConcertTicketsViewModel : Screen
    {
        public ConcertTicketsViewModel(IEventAggregator events, double priceMultiplier)
        {
            _eventAggregator = events;
            _priceMultiplier = priceMultiplier;
            SetPrice();
        }


        // Fields
        private readonly IEventAggregator _eventAggregator;
        private double _priceMultiplier;
        private double _price;
        private string _totalPrice;
        private string _ticketQuantity = "1";
        private Regex _onlyNums = new Regex("[1-9][0-9]{1,3}");

        // Properties
        public string TotalPrice
        {
            get => _totalPrice;
            set
            {
                _totalPrice = value;
                NotifyOfPropertyChange();
            }
        }

        public string TicketQuantity
        {
            get => _ticketQuantity;
            set
            {
                _ticketQuantity = value;
                NotifyOfPropertyChange();
                if (value != "" && _onlyNums.IsMatch(value))
                {
                    SetPrice();
                }
            }
        }

        // Methods
        public void SetPrice()
        {
            _price = Math.Round(50.0 * _priceMultiplier * int.Parse(_ticketQuantity), 2);
            TotalPrice = "€" + _price.ToString();
        }

        public void GoToEPay()
        {
            _eventAggregator.PublishOnUIThreadAsync(new EPayServiceViewModel(_price));
        }
    }
}
