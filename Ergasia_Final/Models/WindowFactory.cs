using Caliburn.Micro;
using Ergasia_Final.ViewModels;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ergasia_Final.Models
{
    /// <summary>
    /// Handles delivery of the singleton ViewModels of the application
    /// <para>
    ///     WARNING: Do not use for EVERY ViewModel. This class is suitable for ViewModels where the retention of properties, attributes & state
    ///     is desirable. If the retention of state is not desired then consider creating simple instances of the ViewModel.
    /// </para>
    /// </summary>
    public abstract class WindowFactory
    {
        // Contains all singleton instances
        private static readonly Dictionary<Type, IScreen> activeViewModels = new Dictionary<Type, IScreen>();

        /// <typeparam name="T">The type of the ViewModel to be requested</typeparam>
        /// <returns>The singleton instance of the requested ViewModel</returns>
        public static IScreen RequestViewModel<T>() where T : IScreen
        {
            IScreen viewModel;
            if (activeViewModels.TryGetValue(typeof(T), out viewModel))
            {
                return viewModel;
            }
            else
            {
                viewModel = (T)Activator.CreateInstance(typeof(T));
                activeViewModels[typeof(T)] = viewModel;
                return viewModel;
            }
        }

        public static IScreen RequestViewModel<T>(params object[] args) where T : IScreen
        {
            IScreen viewModel;
            if (activeViewModels.TryGetValue(typeof(T), out viewModel))
            {
                return viewModel;
            }
            else
            {
                viewModel = (T)Activator.CreateInstance(typeof(T), args);
                activeViewModels[typeof(T)] = viewModel;
                return viewModel;
            }
        }
    }
}
