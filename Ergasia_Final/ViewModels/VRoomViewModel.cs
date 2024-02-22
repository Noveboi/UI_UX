using Caliburn.Micro;
using Ergasia_Final.Models;
using Ergasia_Final.Views;
using System;
using System.Security.Policy;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Ergasia_Final.ViewModels
{
    public class VRoomViewModel : Screen
    {
        private readonly IEventAggregator _eventAggregator;
        private SolidColorBrush _mainBgColor;
        private SolidColorBrush _windowBackground;
        private double _temperature = 20;
        private bool _lightsOn = false;

        public bool LightsOn
        {
            get => _lightsOn;
            set
            {
                _lightsOn = value;
                NotifyOfPropertyChange();
                AdjustBackgroundBrightness();
            }
        }
        public SolidColorBrush WindowBackground
        {
            get => _windowBackground;
            set
            {
                _windowBackground = value;
                NotifyOfPropertyChange();
            }
        }
        public double Temperature
        {
            get => _temperature;
            set
            {
                _temperature = Math.Round(value, 1);
                NotifyOfPropertyChange();
                TintBackground();
            }
        }
        public VRoomViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void OnWindowLoaded()
        {
			_mainBgColor = (SolidColorBrush)((VRoomView)GetView()).FindResource("MainBackground");
            WindowBackground = _mainBgColor;
		}

        private void TintBackground()
        {
            int warmness = 2*((int)_temperature - 20);
            var newColor = new SolidColorBrush(Color.FromRgb(
                (byte)(_mainBgColor.Color.R + warmness),
				_mainBgColor.Color.G,
				_mainBgColor.Color.B));
            WindowBackground = newColor;
        }

        public void AdjustBackgroundBrightness()
        {
            int dim_brighten = LightsOn ? 15 : -15;

            var newColor = new SolidColorBrush(Color.FromRgb(
				NonNegativeX(WindowBackground.Color.R + dim_brighten),
                NonNegativeX(WindowBackground.Color.G + dim_brighten),
                NonNegativeX(WindowBackground.Color.B + dim_brighten)));
            _mainBgColor = new SolidColorBrush(Color.FromRgb(
				NonNegativeX(_mainBgColor.Color.R + dim_brighten),
				NonNegativeX(_mainBgColor.Color.G + dim_brighten),
				NonNegativeX(_mainBgColor.Color.B + dim_brighten)));
			WindowBackground = newColor;
        }

        private byte NonNegativeX(int X)
        {
            return (byte)(X > 0 ? X : 0);
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
