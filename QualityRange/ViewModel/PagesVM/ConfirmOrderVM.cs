﻿using DataBase;
using DataBase.Model;
using QualityRangeForClient.Commands.Base;
using QualityRangeForClient.View.Pages;
using QualityRangeForClient.View.Windows;
using QualityRangeForClient.ViewModel.WindowsVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace QualityRangeForClient.ViewModel.PagesVM
{
    internal class ConfirmOrderVM : ViewModel.Base.ViewModel
    {
        public static ConfirmOrderVM Instance { get; private set; }

        #region Property
        private Client _client;
        public Client Client { get => _client; set => Set(ref _client, value); }


        private Order _order;
        public Order Order { get => _order; set => Set(ref _order, value); }


        private List<Product> _productListOrder;
        public List<Product> ProductListOrder { get => _productListOrder; set => Set(ref _productListOrder, value); }
        #endregion

        #region Commands
        public ICommand Return { get; }
        private bool CanReturnExecute(object parameter) => true;
        private void OnReturnExecute(object parameter) => MainWindow.Instance.GlobalFrame.GoBack();

        public ICommand ConfirmOrder { get; }
        private bool CanConfirmOrderExecute(object parameter)
        {
            if (String.IsNullOrEmpty(Client.NumberOfCreditCard) || Order.PointOfIssue == null || ProductListOrder.Count == 0)
            {
                new MessageBox().Show();
                MessageBoxVM.SetMessage("Для оформления заказа укажите номер карты. Это можно сделать при редактировании профиля.");
                return false;
            }
            return true;
        }
        private void OnConfirmOrderExecute(object parameter)
        {
            DataBase.ConnectionDataBase.db.Order.Local.Add(Order);
            foreach (var item in ProductListOrder)
            {
                DataBase.ConnectionDataBase.db.ProductListOrder.Local.Add(new ProductListOrder
                {
                    Order = Order,
                    Product = item,
                    Count = item.CountProductInBasket
                });

                item.Count -= item.CountProductInBasket;
            }
            new MessageBox().Show();
            MessageBoxVM.SetMessage($"Ваш заказ оформлен");



            DataBase.ConnectionDataBase.db.SaveChanges();

            MainWindow.Instance.GlobalFrame.Navigate(null);
        }

        public ICommand ChangeNumberCard { get; }
        private bool CanChangeNumberCardExecute(object parameter) => true;
        private void OnChangeNumberCardExecute(object parameter) => MainWindow.Instance.GlobalFrame.Navigate(new EditDataUser(true));
        #endregion


        public ConfirmOrderVM()
        {
            Instance = this;

            Client = DataBase.ConnectionDataBase.client;

            Order = new Order()
            {
                Client = Client,
                PointOfIssue = PointOfIssuePage.Instance.ListViewPointOfIssie.SelectedItem as PointOfIssue,
            };

            ProductListOrder = BasketPageVM.Instance.ProductListInBasket.Where(p => p.IsSelected == true).ToList();

            Return = new LambdaCommand(OnReturnExecute, CanReturnExecute);
            ConfirmOrder = new LambdaCommand(OnConfirmOrderExecute, CanConfirmOrderExecute);
            ChangeNumberCard = new LambdaCommand(OnChangeNumberCardExecute, CanChangeNumberCardExecute);
        }
    }
}
