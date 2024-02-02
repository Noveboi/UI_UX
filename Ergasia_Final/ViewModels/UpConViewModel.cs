﻿using Caliburn.Micro;
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
            TextBlock? priceMult = sender.Template.FindName("priceMultiplier", sender) as TextBlock;
            if (priceMult is not null)
            {
                int multiplier = priceMult.Text.Length;
                _eventAggregator.PublishOnUIThreadAsync(new ConcertTicketsViewModel(_eventAggregator, multiplier));
            }
            else 
            { 
                _eventAggregator.PublishOnUIThreadAsync(new ConcertTicketsViewModel(_eventAggregator, 1.0));
            }
        }
    }
}
