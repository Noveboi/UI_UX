using CommunityToolkit.Mvvm.Input;
using Ergasia_Final.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ergasia_Final.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Ergasia_Final.Utilities"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Ergasia_Final.Utilities;assembly=Ergasia_Final.Utilities"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:MonthPicker/>
    ///
    /// </summary>
    public class MonthPicker : Control
    {
        public static readonly DependencyProperty SelectedYearProperty = DependencyProperty.Register("SelectedYear", typeof(int), typeof(MonthPicker));
        public static readonly DependencyProperty SelectedMonthProperty = DependencyProperty.Register("SelectedMonth", typeof(int), typeof(MonthPicker));
        public event EventHandler<MonthPickerSelectionChangedEventArgs> SelectionChanged;
        public ICommand PickMonth { get; private set; }
        public ICommand PreviousYear { get; private set; }
        public ICommand NextYear { get; private set; }
        public int SelectedYear
        {
            get => (int)GetValue(SelectedYearProperty);
            set => SetValue(SelectedYearProperty, value);
        }
        public int SelectedMonth
        {
            get => (int)GetValue(SelectedMonthProperty);
            set => SetValue(SelectedMonthProperty, value);
        }

        public ObservableCollection<MonthModel> Months { get; private set; } = new();
        static MonthPicker()
        {
			DefaultStyleKeyProperty.OverrideMetadata(typeof(MonthPicker), new FrameworkPropertyMetadata(typeof(MonthPicker)));   
		}

        public MonthPicker()
        {
			string[] monthNames = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
			for (int i = 0; i < 12; i++)
			{
				Months.Add(new MonthModel(monthNames[i], i + 1));
			}
			PickMonth = new RelayCommand<int>(ExecutePickMonth);
			PreviousYear = new RelayCommand(() => SelectedYear--);
			NextYear = new RelayCommand(() => SelectedYear++);

			SelectedYear = DateTime.Now.Year;
			SelectedMonth = DateTime.Now.Month;
		}

        private void ExecutePickMonth(int month)
        {
            Months[SelectedMonth - 1].Checked = false;
            Months[month - 1].Checked = true;
            SelectedMonth = month;

            SelectionChanged?.Invoke(this, new MonthPickerSelectionChangedEventArgs()
            {
                Month = month,
                Year = SelectedYear
            });
        }
    }

    public class MonthModel : INotifyPropertyChanged
    {
        private bool _checked = false;
        public string Name { get; }
        public int Index { get; }
        public bool Checked
        {
            get => _checked;
            set
            {
                _checked = value;
                NotifyPropertyChanged();
            }
        }

        public MonthModel(string name, int index)
        {
            Name = name;
            Index = index;
        }

		public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
	}
}
