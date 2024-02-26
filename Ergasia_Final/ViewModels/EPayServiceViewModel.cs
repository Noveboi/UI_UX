using Caliburn.Micro;
using Ergasia_Final.Utilities;
using Microsoft.Xaml.Behaviors.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
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
		private string _cvc = string.Empty;
		private string _cardNumber = string.Empty;
		private string _fullName = string.Empty;
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
        public string ExpirationDate
        {
            get => _dateDisplay;
            set
            {
                if (value.All(c => char.IsDigit(c) || c.Equals('/')))
                {
                    Set(ref _dateDisplay, value);
                    ValidExpirationDate = IsValidExpirationDate(value);
                    NotifyOfPropertyChange(nameof(ValidExpirationDate));
                }
            }
        }

		public string Cvc
        {
            get => _cvc;
            set 
            {
                if (value.All(char.IsDigit) && value.Length <= 3)
                {
                    Set(ref _cvc, value);
                    ValidCVC = IsValidCVC(value);
                    NotifyOfPropertyChange(nameof(ValidCVC));
                }
            }
        }
		public string CardNumber
        {
            get => _cardNumber;
            set
            {
                if (value.All(chr => char.IsDigit(chr) || char.IsWhiteSpace(chr))       // Allow only digits and whitespaces
                    && value.Where(c => !char.IsWhiteSpace(c)).ToArray().Length <= 16)  // Ensure that digits are 16 or less
                {
                    Set(ref _cardNumber, value);
                    ValidCardNumber = IsValidCardNumber(value);
                    NotifyOfPropertyChange(nameof(ValidCardNumber));
                }
			}
        }
		public string FullName 
        { 
            get => _fullName; 
            set
            {
                Set(ref _fullName, value);
                ValidFullName = value.Length > 0;
                NotifyOfPropertyChange(nameof(ValidFullName));
            }
        }

        public bool ValidFullName { get; set; }
        public bool ValidCardNumber { get; set; }
        public bool ValidExpirationDate { get; set; }
        public bool ValidCVC { get; set; }

		// Methods
		public void Pay()
        {
            if (!IsValidCVC(Cvc) || !IsValidCardNumber(CardNumber) || FullName.Length == 0 || !IsValidExpirationDate(ExpirationDate))
            {
                MessageBox.Show("Please fill in valid information!", "Invalid Card Details", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show("Payment successful!\nReturning to Main Menu", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
            _eventAggregator.PublishOnUIThreadAsync("epay to main");
        }

        public void OnMonthPicked(MonthPickerSelectionChangedEventArgs e)
        {
            string dateString = $"{e.Month}/{e.Year}";
            ExpirationDate = dateString;
            IsCalendarOpen = false;
        }

        private bool IsValidCVC(string cvcText)
        {
            return cvcText.All(char.IsDigit) && cvcText.Length == 3; 
        }
        private bool IsValidCardNumber(string cardNumberText)
        {
            return cardNumberText.All(chr => char.IsDigit(chr) || char.IsWhiteSpace(chr))
            && cardNumberText.Where(c => !char.IsWhiteSpace(c)).ToArray().Length == 16;
        }
        private bool IsValidExpirationDate(string expDateText)
        {
            Regex regex = new Regex(@"^([1-9]|(10|11|12))\/2[0-1]([0-9]{2})$");
            return regex.IsMatch(expDateText);
        }
	}
}
