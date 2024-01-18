using Caliburn.Micro;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
                if (_windowStack.Peek().GetType().Name != typeof(MainMenuViewModel).Name)
                {
                    _windowStack.Pop();
                    var newWindow = _windowStack.Peek();
                    ActivateItemAsync(newWindow);
                }
            }
            catch { }
        }
    }
}
