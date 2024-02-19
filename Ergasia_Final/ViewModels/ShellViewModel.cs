using Caliburn.Micro;
using CommunityToolkit.Mvvm.Input;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace Ergasia_Final.ViewModels
{
    /// <summary>
    /// The "frame" for all other Windows to be shown, contains a back button + a menu strip 
    /// </summary>
    public class ShellViewModel : Conductor<object>, IHandle<object>, IHandle<string>
    {
        private readonly IEventAggregator _events;

		// Fields
		private Stack<object> _windowStack;
		private string _maximizeSymbol = "🗖";
		private bool _isDarkMode = true;
        private bool _helpOpen = false;
        private Visibility showBackButton = Visibility.Hidden;
        private Visibility djOpen = Visibility.Collapsed;

		// Properties
        public Visibility ShowBackButton
        {
            get => showBackButton;
            set
            {
                showBackButton = value;
                NotifyOfPropertyChange();
            }
        }
		public string MaximizeSymbol
		{
			get => _maximizeSymbol;
			set
			{
				_maximizeSymbol = value;
				NotifyOfPropertyChange();
			}
		}
        public Visibility DjOpen
        {
            get => djOpen;
            set
            {
                djOpen = value;
                NotifyOfPropertyChange();
            }
        }
        public ICommand OpenOnlineHelp { get; private set; }
        public ICommand CreateBackup { get; private set; }
        public ICommand ExitCommand { get; private set; }

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

            OpenOnlineHelp = new RelayCommand(ExecuteOpenOnlineHelp);
            CreateBackup = new RelayCommand(ExecuteCreateBackup);
            ExitCommand = new RelayCommand(CloseApp);
        }

        // Methods
        // (Implemented from IHandle) This method runs when an event is published in IEventAggregator
        public async Task HandleAsync(object message, CancellationToken cancellationToken)
        {
            if (message.Equals("epay to main"))
            {
                PreviousWindow();
                PreviousWindow();
                PreviousWindow();
            }
            else if (message is Screen)
            {
                ShowBackButton = Visibility.Visible;
                _windowStack.Push(message);
                await ActivateItemAsync(message);
            }
        }

        // Switch the ActiveItem in ContentControl 
        public void PreviousWindow()
        {
            try
            {
                Type activeViewModel = _windowStack.Peek().GetType();
                // If the current window is NOT MainMenuView, then go to previous window
                if (activeViewModel != typeof(MainMenuViewModel))
                {
                    if (activeViewModel == typeof(DJViewModel))
                    {
                        _events.PublishOnUIThreadAsync("DJ Exiting!");
                    } 
                    else if (activeViewModel == typeof(ExHallViewModel))
                    {
                        _events.PublishOnUIThreadAsync("ExHall Exiting!");
                    }
                    _windowStack.Pop();
                    var newWindow = _windowStack.Peek();
                    ActivateItemAsync(newWindow);

                    if (newWindow.GetType() == typeof(MainMenuViewModel))
                    {
                        ShowBackButton = Visibility.Hidden;
                    }
                }
            }
            catch 
            { 
                //do nothing
            }
        }

        public void ExecuteOpenOnlineHelp()
        {
            if (_helpOpen) return;

            try
            {
                string cwd = Environment.CurrentDirectory;
                string fileName = Directory.GetParent(cwd).Parent.Parent.FullName + "\\help\\help.html";

                var process = new Process();
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.FileName = fileName;
                process.Start();
            } 
            catch (Exception) { }
        }

        public void ExecuteCreateBackup()
        {
            MessageBox.Show("DJ has been backed up succesfully!");
        }

        // Replace color theme resource dictionaries to dynamically change app colors
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

        public void MinimizeView()
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        public void MaximizeView()
        {
            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
            {
                MaximizeSymbol = "🗗︎";
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            } 
            else
            {
                MaximizeSymbol = "🗖";
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
        }

        public void CloseApp()
        {
            Application.Current.Shutdown();
        }

        public void MoveWindow(MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Application.Current.MainWindow.DragMove();
            }
        }

		public Task HandleAsync(string message, CancellationToken cancellationToken)
		{
            DjOpen = message.Equals("DJ Opening!") ? Visibility.Visible : Visibility.Collapsed;
            return Task.CompletedTask;
		}
	}
}
