using Caliburn.Micro;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Ergasia_Final.ViewModels
{
    /// <summary>
    /// The "frame" for all other Windows to be shown, contains a back button + a menu strip 
    /// </summary>
    public class ShellViewModel : Conductor<object>, IHandle<object>
    {
        private readonly IEventAggregator _events;
        /// <summary>
        /// By default, request the event aggregator instance for class cross-communication
        /// </summary>
        public ShellViewModel(IEventAggregator eventAggregator)
        {
            _events = eventAggregator;
            _events.SubscribeOnUIThread(this);

            var mainMenu = new MainMenuViewModel(eventAggregator);

            ActivateItemAsync(mainMenu);

            _windowStack = new Stack<object>();
            _windowStack.Push(mainMenu);
        }

        // Fields
        /// <summary>
        /// Remember which windows have been opened in order to go back to previous ones
        /// </summary>
        private Stack<object> _windowStack;
        private bool _isDarkMode = true;

        // Methods
        /// <summary>
        /// (Implemented from IHandle) This method runs when an event is published in IEventAggregator
        /// </summary>
        public async Task HandleAsync(object message, CancellationToken cancellationToken)
        {
            _windowStack.Push(message);
            await ActivateItemAsync(message);
        }
        public void PreviousWindow()
        {
            try
            {
                // If the current window is NOT MainMenuView, then go to previous window
                if (_windowStack.Peek().GetType().Name != typeof(MainMenuViewModel).Name)
                {
                    _windowStack.Pop();
                    var newWindow = _windowStack.Peek();
                    ActivateItemAsync(newWindow);
                }
            }
            catch 
            { 
                //do nothing
            }
        }

        public void ToggleColorTheme()
        {
            // Clear the application dictionary for the colors
            // IMPORTANT: do not change the ORDER of the resource dictionaries in App.xaml!!!
            Application.Current.Resources.MergedDictionaries.RemoveAt(2);

            string colorThemePath = _isDarkMode ? "Resources/LightColors.xaml" : "Resources/DarkColors.xaml";

            // Create the ResourceDictionary and add it to the application scope
            ResourceDictionary newTheme = new ResourceDictionary
            {
                Source = new Uri(colorThemePath, UriKind.Relative)
            };
            Application.Current.Resources.MergedDictionaries.Add(newTheme);

            _isDarkMode = !_isDarkMode;
        }
    }
}
