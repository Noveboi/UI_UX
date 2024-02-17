using Caliburn.Micro;
using Ergasia_Final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergasia_Final.ViewModels
{
    public class CafeteriaViewModel : Screen
    {
        public BindableCollection<CatalogueItemModel> catalogueItems;

        public CafeteriaViewModel()
        {
            catalogueItems = new BindableCollection<CatalogueItemModel>();
        }
    }
}
