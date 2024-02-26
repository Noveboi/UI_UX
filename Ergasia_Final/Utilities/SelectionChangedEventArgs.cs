using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergasia_Final.Utilities
{
    public class MonthPickerSelectionChangedEventArgs : EventArgs
    {
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
