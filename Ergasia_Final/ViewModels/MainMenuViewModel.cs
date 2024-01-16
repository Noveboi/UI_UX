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
        private readonly IEventAggregator _eventAggregator;
        public MainMenuViewModel(IEventAggregator eventAggregator) 
        {
            _eventAggregator = eventAggregator;
        }

        public void OpenUpCon()
        {
            _eventAggregator.PublishOnUIThreadAsync(new UpConViewModel(_eventAggregator));
        }
        public void OpenExHall()
        {
            _eventAggregator.PublishOnUIThreadAsync(new ExHallViewModel());
        }
        public void OpenVRoom()
        {
            _eventAggregator.PublishOnUIThreadAsync(new VRoomViewModel(_eventAggregator));
        }
        public void OpenDJ()
        {
            _eventAggregator.PublishOnUIThreadAsync(new DJViewModel());
        }
    }
}
