using DataBase.Model;
using QualityRangeForEmployee.Commands.Base;
using QualityRangeForEmployee.View.Pages;
using QualityRangeForEmployee.ViewModel.WindowsVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using DataBase;
using System.Collections.ObjectModel;

namespace QualityRangeForEmployee.ViewModel.PagesVM
{
    internal class RequestProductPageVM : ViewModel.Base.ViewModel
    {
        public static RequestProductPageVM Instance { get; private set; }

        #region Property
        private ObservableCollection<Product> _productList = new ObservableCollection<Product>(ConnectionDataBase.db.Product.Local);
        public ObservableCollection<Product> ProductList { get => _productList; set => Set(ref _productList, value); }
        #endregion


        #region Commands
        public ICommand Approved { get; }
        private bool CanApprovedExecute(object parameter) => true;
        private void OnApprovedExecute(object parameter)
        {
            var product = (Product)parameter;

            product.ID_Status = 1;

            RequestProductPage.Instance.ListProduct.Items.Refresh();
        }
        
        
        public ICommand NotApproved { get; }
        private bool CanNotApprovedExecute(object parameter) => true;
        private void OnNotApprovedExecute(object parameter)
        {
            var product = (Product)parameter;

            product.ID_Status = 3;

            RequestProductPage.Instance.ListProduct.Items.Refresh();
        }
        #endregion


        public RequestProductPageVM()
        {
            Instance = this;

            Approved = new LambdaCommand(OnApprovedExecute, CanApprovedExecute);
            NotApproved = new LambdaCommand(OnNotApprovedExecute, CanNotApprovedExecute);
        }
    }
}
