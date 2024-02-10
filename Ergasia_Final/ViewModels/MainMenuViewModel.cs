using Caliburn.Micro;
using Ergasia_Final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergasia_Final.ViewModels
{
    public class MainMenuViewModel : Screen
    {
        private readonly IEventAggregator _eventAggregator;
        public MainMenuViewModel(IEventAggregator eventAggregator) 
        {
            _eventAggregator = eventAggregator;
        }

        // Methods for the 4 buttons 
        public void OpenUpCon()
        {
            _eventAggregator.PublishOnUIThreadAsync(WindowFactory.RequestViewModel<UpConViewModel>(_eventAggregator));
        }
        public void OpenExHall()
        {
            _eventAggregator.PublishOnUIThreadAsync(WindowFactory.RequestViewModel<ExHallViewModel>());
        }
        public void OpenVRoom()
        {
            _eventAggregator.PublishOnUIThreadAsync(WindowFactory.RequestViewModel<VRoomViewModel>(_eventAggregator));
        }
        public void OpenDJ()
        {
            _eventAggregator.PublishOnUIThreadAsync(WindowFactory.RequestViewModel<DJViewModel>());
        }
    }
}
