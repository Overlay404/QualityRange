using QualityRange.Commands.Base;
using QualityRange.Model;
using QualityRange.View.Pages;
using QualityRange.View.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QualityRange.ViewModel
{
    public class GridAndBarsViewProductPanelVM : ViewModel.Base.ViewModel
    {
        public static GridAndBarsViewProductPanelVM Instance { get; set; }

        #region Property
        private IEnumerable<Product> _products;
        public IEnumerable<Product> Products { get => _products; set => Set(ref _products, value); }
        #endregion

        #region Commands
        public ICommand AddBasketProduct { get; }
        private bool CanAddBasketProductExecute(object parameter) => true;
        private void OnAddBasketProductExecute(object parameter)
        {
            if (App.user == null)
            {
                new AuthRegWindow().Show();
                return;
            }

            var basket = App.db.Basket.Local.FirstOrDefault(b => b.Client.User == App.user);
            if (basket == null)
            {
                basket = new Basket { Client = App.db.Client.Local.FirstOrDefault(c => c.User == App.user) };
                App.db.Basket.Local.Add(basket);
            }

            var productList = App.db.ProductList.Local.FirstOrDefault(p => p.ID_Product == (int)parameter);

            App.db.ProductList.Local.Add(new ProductList
            {
                Basket = basket,
                ID_Product = (int)parameter,
                Count = 1
            });

            MainWindowVM.Instance.InitCountProductInBasket();
            InitProductList();
        }

        public ICommand RemoveProductInListProduct { get; }
        private bool CanRemoveProductInListProductExecute(object parameter) => true;
        private void OnRemoveProductInListProductExecute(object parameter) 
        {
            var prodListItem = App.db.ProductList.Local.FirstOrDefault(prodlist => prodlist.Basket.ID_Client == App.user.ID && prodlist.Product.ID == (int)parameter);

            if(prodListItem.Count == 1) 
            {
                App.db.ProductList.Local.Remove(prodListItem);
            }
            else
            {
                prodListItem.Count--;
            }

            MainWindowVM.Instance.InitCountProductInBasket();
            InitProductList();
        }

        public ICommand AddProductInListProduct { get; }
        private bool CanAddProductInListProductExecute(object parameter) => true;
        private void OnAddProductInListProductExecute(object parameter)
        {
            var prodListItem = App.db.ProductList.Local.FirstOrDefault(prodlist => prodlist.Basket.ID_Client == App.user.ID && prodlist.Product.ID == (int)parameter);

            prodListItem.Count++;

            MainWindowVM.Instance.InitCountProductInBasket();
            InitProductList();
        }
        #endregion

        public GridAndBarsViewProductPanelVM()
        {
            if (Instance == null)
                Instance = this;

            InitProductList();

            AddBasketProduct = new LambdaCommand(OnAddBasketProductExecute, CanAddBasketProductExecute);
            RemoveProductInListProduct = new LambdaCommand(OnRemoveProductInListProductExecute, CanRemoveProductInListProductExecute);
            AddProductInListProduct = new LambdaCommand(OnAddProductInListProductExecute, CanAddProductInListProductExecute);
        }

        public void InitProductList()
        {
            Products = App.db.Product.Local.ToList();
        }
    }
}
