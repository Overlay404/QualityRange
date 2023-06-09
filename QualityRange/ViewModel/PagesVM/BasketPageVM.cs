﻿using DataBase.Model;
using QualityRangeForClient.Commands.Base;
using QualityRangeForClient.View.Pages;
using QualityRangeForClient.View.Windows;
using QualityRangeForClient.ViewModel.WindowsVM;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace QualityRangeForClient.ViewModel.PagesVM
{
    internal class BasketPageVM : ViewModel.Base.ViewModel
    {
        public static BasketPageVM Instance { get; private set; }

        #region Property
        private BindingList<Product> _productListInBasket;
        public BindingList<Product> ProductListInBasket { get => _productListInBasket; set => Set(ref _productListInBasket, value); }

        private int _countElementInBasket;
        public int CountElementInBasket { get => _countElementInBasket; set => Set(ref _countElementInBasket, value); }

        private int _countSelectedProductInBasket;
        public int CountSelectedProductInBasket { get => _countSelectedProductInBasket; set => Set(ref _countSelectedProductInBasket, value); }

        private decimal _costAllProductInBasket;
        public decimal CostAllProductInBasket { get => _costAllProductInBasket; set => Set(ref _costAllProductInBasket, value); }

        private IEnumerable<PhotoProduct> _images;
        public IEnumerable<PhotoProduct> Images { get => _images; set => Set(ref _images, value); }
        #endregion

        #region Commands
        public ICommand Return { get; }
        private bool CanReturnExecute(object parameter) => true;
        private void OnReturnExecute(object parameter)
        {
            MainWindow.Instance.GlobalFrame.Navigate(null);
        }

        public ICommand ShowPointIfIssuePage { get; }
        private bool CanShowPointIfIssuePageExecute(object parameter)
        {
            if (CountSelectedProductInBasket == 0)
            {
                new MessageBox().Show();
                MessageBoxVM.SetMessage("Не выбран ни один товар");
                return false;
            }
            return true;
        }
        private void OnShowPointIfIssuePageExecute(object parameter)
        {
            MainWindow.Instance.GlobalFrame.Navigate(new PointOfIssuePage());
        }
        #endregion


        public BasketPageVM()
        {
            Instance = this;

            var userBasket = DataBase.ConnectionDataBase.client.Basket.FirstOrDefault()?.ID;
            var listProduct = DataBase.ConnectionDataBase.db.ProductList.Local.Where(pl => pl.ID_Basket == userBasket).Select(p => p.Product).ToList();

            ProductListInBasket = new BindingList<Product>(listProduct);
            CountElementInBasket = listProduct.Count;

            UpdateInfo();

            Return = new LambdaCommand(OnReturnExecute, CanReturnExecute);
            ShowPointIfIssuePage = new LambdaCommand(OnShowPointIfIssuePageExecute, CanShowPointIfIssuePageExecute);
        }

        public void UpdateInfo()
        {
            var selectedItemList = ProductListInBasket.Where(p => p.IsSelected == true);

            CountSelectedProductInBasket = (int)selectedItemList.Sum(si => si.CountProductInBasket);
            CostAllProductInBasket = selectedItemList.Sum(pl => pl.CostWithDiscount * pl.CountProductInBasket);
        }
    }
}
