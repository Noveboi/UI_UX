using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergasia_Final.ViewModels
{
    public class MainMenuViewModel : Screen
    {
        private readonly IEventAggregator _events;
        public MainMenuViewModel(IEventAggregator eventAggregator) 
        {
            _events = eventAggregator;
        }

        public void OpenTestView()
        {
            _events.PublishOnUIThreadAsync(new TestViewModel());
        }
    }
}
