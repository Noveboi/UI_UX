using Caliburn.Micro;
using Ergasia_Final.Models;
using System.Windows.Controls;

namespace Ergasia_Final.ViewModels
{
    public class VRoomViewModel : Screen
    {
        private readonly IEventAggregator _eventAggregator;
        public VRoomViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void OpenFoodAndDrink()
        {
            // Keep a singleton instance of CafeteriaViewModel in order to maintain the user's order state
            _eventAggregator.PublishOnUIThreadAsync(WindowFactory.RequestViewModel<CafeteriaViewModel>(_eventAggregator));
        }

        public void GoToArtistHall(Button source)
        {
            _eventAggregator.PublishOnUIThreadAsync(new ArtistHallViewModel(source.Name));
        }

	}
}
