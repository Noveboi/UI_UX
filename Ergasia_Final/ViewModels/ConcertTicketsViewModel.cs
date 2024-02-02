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
        public ConcertTicketsViewModel(IEventAggregator events, double priceMultiplier, string artistName)
        {
            _eventAggregator = events;
            _artistPriceMultiplier = priceMultiplier;
            _artistName = artistName;
            SetPrice();
        }


        // Fields
        private readonly IEventAggregator _eventAggregator;

        private double _artistPriceMultiplier;
        private double _seatPriceMultiplier = 1.0;

        private double _totalPriceNumeric;
        private string _totalPriceString;

        private string _ticketQuantity = "1";

        private string _artistName;

        // Match numbers from 1 - 9999
        private Regex _onlyNums = new Regex("[1-9][0-9]{0,3}");

        // Properties
        // Displayed on ConcertTicketView
        public string TotalPrice
        {
            get => _totalPriceString;
            set
            {
                _totalPriceString = value;
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

        public string ArtistName
        {
            get => _artistName;
        }

        // Methods
        /// <summary>
        /// 
        /// </summary>
        public void SetPrice()
        {
            _totalPriceNumeric = Math.Round(50.0 * _artistPriceMultiplier * _seatPriceMultiplier * int.Parse(_ticketQuantity), 2);
            TotalPrice = "€" + _totalPriceNumeric.ToString();
        }

        public void GoToEPay()
        {
            _eventAggregator.PublishOnUIThreadAsync(new EPayServiceViewModel(_totalPriceNumeric, _eventAggregator));
        }

        public void ChooseSide1()
        {
            _seatPriceMultiplier = 1.5;
            SetPrice();
        }
        
        public void ChooseSide2()
        {
            _seatPriceMultiplier = 1.5;
            SetPrice();
        }

        public void ChooseGenAdmission()
        {
            _seatPriceMultiplier = 1;
            SetPrice();
        }

        public void ChooseVip()
        {
            _seatPriceMultiplier = 2;
            SetPrice();
        }
    }
}
