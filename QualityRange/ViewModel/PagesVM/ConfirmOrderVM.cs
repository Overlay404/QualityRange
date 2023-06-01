using QualityRange.Commands.Base;
using QualityRange.Model;
using QualityRange.View.Pages;
using QualityRange.View.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QualityRange.ViewModel.PagesVM
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
        private void OnReturnExecute(object parameter)
        {
            MainWindow.Instance.BasketFrame.GoBack();
        }

        public ICommand ConfirmOrder { get; }
        private bool CanConfirmOrderExecute(object parameter) => true;
        private void OnConfirmOrderExecute(object parameter)
        {
            
        }
        
        public ICommand ChangeNumberCard { get; }
        private bool CanChangeNumberCardExecute(object parameter) => true;
        private void OnChangeNumberCardExecute(object parameter)
        {
            
        }

        #endregion


        public ConfirmOrderVM()
        {
            Instance = this;

            Client = App.user.Client;

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
