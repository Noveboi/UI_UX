using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Ergasia_Final.Views
{
    /// <summary>
    /// Interaction logic for DJView.xaml
    /// </summary>
    public partial class DJView : UserControl
    {
        public DJView()
        {
            InitializeComponent();
            
        }

        //private void SongQueue_Loaded(object sender, RoutedEventArgs e)
        //{
        //    foreach (var column in SongQueue.Columns)
        //    {
        //        column.MinWidth = column.ActualWidth;
        //        column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        //    }
        //}
    }
}
