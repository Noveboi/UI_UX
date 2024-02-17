using Caliburn.Micro;
using Ergasia_Final.Models;
using Ergasia_Final.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Ergasia_Final.ViewModels
{
    public class CafeteriaViewModel : Screen
    {
        public List<CatalogueItemModel> CatalogueItems { get; }

        public CafeteriaViewModel()
        {
            CatalogueItems = new List<CatalogueItemModel>();
            PopulateUtility.AddCatalogueItems(CatalogueItems);
        }
    }
}
