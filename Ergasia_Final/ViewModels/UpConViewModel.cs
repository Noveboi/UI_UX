using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Ergasia_Final.ViewModels
{
    public class UpConViewModel
    {
        private readonly IEventAggregator _eventAggregator; 
        public UpConViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        // Methods
        public void GoToTickets(Button sender)
        {
            _eventAggregator.PublishOnUIThreadAsync(new ConcertTicketsViewModel(sender.Name));
        }
    }
}
