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
        private double orderCost = 0;
        private readonly IEventAggregator _eventAggregator;

        public double OrderCost
        {
            get => orderCost;
            set
            {
                orderCost = Math.Round(value, 2);
                CanPay = orderCost > 0;
                NotifyOfPropertyChange("CanPay");
                NotifyOfPropertyChange();
            }
        }
        public bool CanPay { get; set; }
        public List<CatalogueItemModel> CatalogueItems { get; }
        public BindableCollection<CatalogueItemModel> Cart { get; }


        public CafeteriaViewModel(IEventAggregator eventAggregator)
        {
            CatalogueItems = new List<CatalogueItemModel>();
            Cart = new BindableCollection<CatalogueItemModel>();
            PopulateUtility.AddCatalogueItems(CatalogueItems);

            _eventAggregator = eventAggregator;
        }

        public void AddItemToCart(CatalogueItemModel item)
        {
            Cart.Add(item);
            OrderCost += item.Price;
        }
        public void RemoveItemFromCart(string itemName)
        {
            var item = Cart.Where(item => item.Name == itemName).First();
            Cart.Remove(item);
            OrderCost -= item.Price;
        }

        public void GoToEPay()
        {
            _eventAggregator.PublishOnUIThreadAsync(new EPayServiceViewModel(OrderCost, _eventAggregator));
        }

	}
}
