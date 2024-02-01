using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergasia_Final.ViewModels
{
    public class EPayServiceViewModel
    {
        public EPayServiceViewModel(double price)
        {
            _price = "€" + price.ToString();
        }

        // Fields
        private string _price;

        // Properties
        public string Price
        {
            get => _price;
        }
    }
}
