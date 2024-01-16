using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ergasia_Final.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<object>
    {
        private readonly IEventAggregator _events;
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
