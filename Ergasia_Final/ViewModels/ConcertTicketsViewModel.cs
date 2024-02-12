using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using Ergasia_Final.Models;

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

        private bool sideVip1Checked = false;
        private bool sideVip2Checked = false;
        private bool vipChecked = false;
        private bool genAdChecked = true;

        private bool isTicketsOutOfRange = false;

        // Properties

        public bool SideVip1Checked
        {
            get => sideVip1Checked;
            set
            {
                if (!(!value && !sideVip2Checked && !genAdChecked && !vipChecked))
                {
                    sideVip1Checked = value;
                    NotifyOfPropertyChange();
                    if (sideVip1Checked)
                    {
                        SideVip2Checked = false;
                        VipChecked = false;
                        GenAdChecked = false;
                    }
                }
            }
        }
        public bool SideVip2Checked
        {
            get => sideVip2Checked;
            set
            {
                if (!(!value && !sideVip1Checked && !genAdChecked && !vipChecked))
                {
                    sideVip2Checked = value;
                    NotifyOfPropertyChange();
                    if (sideVip2Checked)
                    {
                        SideVip1Checked = false;
                        VipChecked = false;
                        GenAdChecked = false;
                    }
                }

            }
        }
        public bool VipChecked
        {
            get => vipChecked;
            set
            {
                if (!(!value && !sideVip1Checked && !genAdChecked && !sideVip2Checked))
                {
                    vipChecked = value;
                    NotifyOfPropertyChange();
                    if (vipChecked)
                    {
                        SideVip1Checked = false;
                        SideVip2Checked = false;
                        GenAdChecked = false;
                    }
                }
            }
        }
        public bool GenAdChecked
        {
            get => genAdChecked;
            set
            {
                if (!(!value && !sideVip1Checked && !sideVip2Checked && !vipChecked))
                {
                    genAdChecked = value;
                    NotifyOfPropertyChange();
                    if (genAdChecked)
                    {
                        SideVip1Checked = false;
                        SideVip2Checked = false;
                        VipChecked = false;
                    }
                }
            }
        }

        public bool IsTicketsOutOfRange
        {
            get => isTicketsOutOfRange;
            set
            {
                isTicketsOutOfRange = value;
                NotifyOfPropertyChange();
            }
        }

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
                if (value.Equals(string.Empty))
                {
                    _ticketQuantity = string.Empty;
                    NotifyOfPropertyChange();
                    TotalPrice = "-";
                }
                else if (ValidateInput(value))
                {
                    _ticketQuantity = value;
                    NotifyOfPropertyChange();
                    SetPrice();
                }
            }
        }

        public string ArtistName
        {
            get => _artistName;
        }

        // Methods
        public void SetPrice()
        {
            if (!_ticketQuantity.Equals(string.Empty))
            {
                _totalPriceNumeric = Math.Round(50.0 * _artistPriceMultiplier * _seatPriceMultiplier * int.Parse(_ticketQuantity), 2);
                TotalPrice = "€" + _totalPriceNumeric.ToString();
            }
        }

        public void GoToEPay()
        {
            if (!_ticketQuantity.Equals(string.Empty))
            {
                _eventAggregator.PublishOnUIThreadAsync(new EPayServiceViewModel(_totalPriceNumeric, _eventAggregator));
            } 
            else
            {
                MessageBox.Show("Please enter a non-empty ticket quantity!", "Need more tickets!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool ValidateInput(string input)
        {
            int num;
            bool isValid = int.TryParse(input, out num);
            if (isValid && num > 0 && num < 100)
            {
                IsTicketsOutOfRange = false;
                return true;
            }
            else if (isValid && num <= 0 || num >= 100)
            {
                IsTicketsOutOfRange = true;
                IsTicketsOutOfRange = false;
                return false;
            }
            return false;
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
