using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergasia_Final.ViewModels
{
    public class EPayServiceViewModel
    {
        public EPayServiceViewModel(double price, IEventAggregator events)
        {
            _eventAggregator = events;
            _price = $"Finalize Payment (€{price})";
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
            _eventAggregator.PublishOnUIThreadAsync("epay to main");
        }
    }
}
