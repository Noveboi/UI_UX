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
        /// <summary>
        /// Redirects to <see cref="ConcertTicketsViewModel"/> and passes the following data:
        /// <list type="bullet">
        /// <item>The price multiplier depending on the artist</item>
        /// <item>The artist's name</item>
        /// </list>
        /// </summary>
        /// <param name="sender"></param>
        public void GoToTickets(Button sender)
        {
            TextBlock? priceMult = sender.Template.FindName("priceMultiplier", sender) as TextBlock;
            TextBlock? artistName = sender.Template.FindName("artist", sender) as TextBlock;
            if (priceMult is not null && artistName is not null)
            {
                int multiplier = priceMult.Text.Length;
                _eventAggregator.PublishOnUIThreadAsync(new ConcertTicketsViewModel(_eventAggregator, multiplier, artistName.Text));
            }
            else 
            { 
                _eventAggregator.PublishOnUIThreadAsync(new ConcertTicketsViewModel(_eventAggregator, 1.0, "Stage"));
            }
        }
    }
}
