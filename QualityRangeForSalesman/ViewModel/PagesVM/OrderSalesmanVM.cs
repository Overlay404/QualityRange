using DataBase.Model;
using DataBase;
using QualityRangeForSalesman.Commands.Base;
using QualityRangeForSalesman.SupportiveClasses;
using QualityRangeForSalesman.View.Pages;
using QualityRangeForSalesman.View.Windows;
using QualityRangeForSalesman.ViewModel.WindowsVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QualityRangeForSalesman.ViewModel.PagesVM
{
    internal class OrderSalesmanVM : ViewModel.Base.ViewModel
    {
        public static OrderSalesmanVM Instance { get; private set; }

        #region Property
        private ObservableCollection<IGrouping<Order, ProductListOrder>> _orderes = new ObservableCollection<IGrouping<Order, ProductListOrder>>(ConnectionDataBase.db.ProductListOrder.Local.Where(o => o.Product.Salesman == ConnectionDataBase.salesman).GroupBy(pr => pr.Order));
        public ObservableCollection<IGrouping<Order, ProductListOrder>> Orderes { get => _orderes; set => Set(ref _orderes, value); }
        #endregion

        #region Commands
        public ICommand ShowInfoOrder { get; }
        private bool CanShowInfoOrderExecute(object parameter) => true;
        private void OnShowInfoOrderExecute(object parameter)
        {
            OrderSalesman.Instance.ListProductGridView.SelectedItem = (parameter as IEnumerable<ProductListOrder>);
        }
        #endregion


        public OrderSalesmanVM()
        {
            Instance = this;

            ShowInfoOrder = new LambdaCommand(OnShowInfoOrderExecute, CanShowInfoOrderExecute);
            
        }
    }
}
