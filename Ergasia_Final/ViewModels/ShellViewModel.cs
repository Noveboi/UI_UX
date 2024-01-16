using Caliburn.Micro;
using System;
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
        /// By default, request the event aggregator instance for inter-class communication
        /// </summary>
        /// <param name="eventAggregator"></param>
        public ShellViewModel(IEventAggregator eventAggregator)
        {
            _events = eventAggregator;
            _events.SubscribeOnUIThread(this);
            ActivateItemAsync(new MainMenuViewModel(eventAggregator));
        }

        public async Task HandleAsync(object message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(message);
        }
    }
}
