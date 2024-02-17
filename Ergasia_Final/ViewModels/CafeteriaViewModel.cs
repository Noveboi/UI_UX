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
        private List<CatalogueItemModel> _catalogueItems;

        public BindableCollection<CatalogueItemModel> CoffeeItems { get; private set; }
        public BindableCollection<CatalogueItemModel> TeaItems { get; private set; }
        public BindableCollection<CatalogueItemModel> SoftDrinkItems { get; private set; }
        public BindableCollection<CatalogueItemModel> SnackItems { get; private set; }
        public BindableCollection<CatalogueItemModel> SweetItems { get; private set; }

        public CafeteriaViewModel()
        {
            _catalogueItems = new List<CatalogueItemModel>();
            PopulateUtility.AddCatalogueItems(_catalogueItems);
            CategoricalSplitList();
        }

        private void CategoricalSplitList()
        {
            CoffeeItems = new BindableCollection<CatalogueItemModel>
                (_catalogueItems.Where(item => item.Type == CatalogueItemTypes.Coffee));
            TeaItems = new BindableCollection<CatalogueItemModel>
                (_catalogueItems.Where(item => item.Type == CatalogueItemTypes.Tea));
			SoftDrinkItems = new BindableCollection<CatalogueItemModel>
				(_catalogueItems.Where(item => item.Type == CatalogueItemTypes.SoftDrink));
			SnackItems = new BindableCollection<CatalogueItemModel>
				(_catalogueItems.Where(item => item.Type == CatalogueItemTypes.Snack));
			SweetItems = new BindableCollection<CatalogueItemModel>
				(_catalogueItems.Where(item => item.Type == CatalogueItemTypes.Sweet));
		}
    }
}
