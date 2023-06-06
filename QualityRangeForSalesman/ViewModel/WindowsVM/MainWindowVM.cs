using DataBase;
using DataBase.Model;
using QualityRangeForSalesman.Commands.Base;
using QualityRangeForSalesman.View.Pages;
using QualityRangeForSalesman.View.Windows;
using QualityRangeForSalesman.ViewModel.PagesVM;
using QualityRangeForSalesman.ViewModel.WindowsVM;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MessageBox = QualityRangeForSalesman.View.Windows.MessageBox;

namespace QualityRangeForSalesman.ViewModel
{
    internal class MainWindowVM : ViewModel.Base.ViewModel
    {
        public static MainWindowVM Instance { get; private set; }

        #region Property

        private Window _authRegWin = new AuthRegWindow();
        public Window AuthRegWin { get => _authRegWin; set => Set(ref _authRegWin, value); }


        private int _countProduct = DataBase.ConnectionDataBase.db.Product.Local.Count();
        public int CountProduct { get => _countProduct; set => Set(ref _countProduct, value); }


        private Salesman _salesmantDC;
        public Salesman SalesmanDC { get => _salesmantDC; set => Set(ref _salesmantDC, value); }


        private Visibility _visibilityBtn = Visibility.Visible;
        public Visibility VisibilityBtn { get => _visibilityBtn; set => Set(ref _visibilityBtn, value); }


        private Visibility _visibilityUserIcon = Visibility.Collapsed;
        public Visibility VisibilityUserIcon { get => _visibilityUserIcon; set => Set(ref _visibilityUserIcon, value); }


        private Visibility _userInfoPanel = Visibility.Collapsed;
        public Visibility UserInfoPanel { get => _userInfoPanel; set => Set(ref _userInfoPanel, value); }


        private int _countOrder;
        public int CountOrder { get => _countOrder; set => Set(ref _countOrder, value); }
        
        
        private bool _isPageOrder;
        public bool IsPageOrder { get => _isPageOrder; set => Set(ref _isPageOrder, value); }
        #endregion


        #region Commands

        public ICommand MinimazeWindow { get; }
        private bool CanMinimazeWindowExecute(object parameter) => true;
        private void OnMinimazeWindowExecute(object parameter) => MainWindow.Instance.WindowState = WindowState.Minimized;

        public ICommand GoPage { get; }
        private bool CanGoPageExecute(object parameter) => true;
        private void OnGoPageExecute(object parameter) => MainWindow.Instance.ProductListFrame.Navigate(parameter);

        public ICommand GoWindow { get; }
        private bool CanGoWindowExecute(object parameter) => true;
        private void OnGoWindowExecute(object parameter) => new AuthRegWindow().Show();

        public ICommand UserInfo { get; }
        private bool CanUserInfoExecute(object parameter) => true;
        private void OnUserInfoExecute(object parameter) => UserInfoPanel = UserInfoPanel == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

        public ICommand EditClient { get; }
        private bool CanEditClientExecute(object parameter) => true;
        private void OnEditClientExecute(object parameter) => MainWindow.Instance.GlobalFrame.Navigate(new EditDataUser());

        public ICommand SearchProduct { get; }
        private bool CanSearchProductExecute(object parameter) => true;
        private void OnSearchProductExecute(object parameter) => SearchProducts();

        public ICommand MaximizeWindow { get; }
        private void OnMaximizeWindowExecute(object parameter) => MainWindow.Instance.WindowState = WindowState.Maximized;
        private bool CanMaximizeWindowExecute(object parameter)
        {
            if (MainWindow.Instance.WindowState == WindowState.Maximized)
            {
                MainWindow.Instance.WindowState = WindowState.Normal;
                return false;
            };
            return true;
        }

        public ICommand ShutdownApplication { get; }
        private bool CanShutdownApplicationExecute(object parameter)
        {
            if (ProductsSalesmanVM.IsAddingNewProduct == true)
            {
                new MessageBox().Show();
                MessageBoxVM.SetMessage("Вы в процессе добавления товара");
                return false;
            }
            return true;
        }
        private void OnShutdownApplicationExecute(object parameter) => App.Current.Shutdown();

        public ICommand ShowOrderPage { get; }
        private bool CanShowOrderPageExecute(object parameter)
        {
            if (DataBase.ConnectionDataBase.salesman == null)
            {
                new AuthRegWindow().Show();
                return false;
            }

            if (ProductsSalesmanVM.IsAddingNewProduct == true)
            {
                new MessageBox().Show();
                MessageBoxVM.SetMessage("Вы в процессе добавления товара");
                return false;
            }

            return true;
        }
        private void OnShowOrderPageExecute(object parameter)
        {
            if(OrderSalesman.Instance == null)
            {
                new OrderSalesman();
                new OrderSalesmanVM();
            }
            MainWindow.Instance.ProductListFrame.Navigate(OrderSalesman.Instance);
            ProductsSalesman.Instance.ListProductGridView.SelectedItem = null;
            IsPageOrder = true;
        }  
        
        public ICommand ShowProductPage { get; }
        private bool CanShowProductPageExecute(object parameter)
        {
            if (DataBase.ConnectionDataBase.salesman == null)
            {
                new AuthRegWindow().Show();
                return false;
            }

            if (ProductsSalesmanVM.IsAddingNewProduct == true)
            {
                new MessageBox().Show();
                MessageBoxVM.SetMessage("Вы в процессе добавления товара");
                return false;
            }

            return true;
        }
        private void OnShowProductPageExecute(object parameter)
        {
            MainWindow.Instance.ProductListFrame.Navigate(ProductsSalesman.Instance);
            OrderSalesman.Instance.ListProductGridView.SelectedItem = null;
            IsPageOrder = false;
        }

        public ICommand DragMoveWindow { get; }
        private bool CanDragMoveWindowExecute(object parameter) => true;
        private void OnDragMoveWindowExecute(object parameter)
        {
            if (MainWindow.Instance.WindowState == WindowState.Maximized)
            {
                var positionCursorX = MainWindow.Instance.PointToScreen(new Point(Mouse.GetPosition(null).X, Mouse.GetPosition(null).Y)).X;
                MainWindow.Instance.WindowState = WindowState.Normal;
                MainWindow.Instance.Top = 0;
                MainWindow.Instance.Left = positionCursorX - MainWindow.Instance.Width / 2;
            }
            MainWindow.Instance.DragMove();
        }

        public ICommand LogOut { get; }
        private bool CanLogOutExecute(object parameter)
        {
            if(ProductsSalesmanVM.IsAddingNewProduct == true)
            {
                new MessageBox().Show();
                MessageBoxVM.SetMessage("Вы в процессе добавления товара");
                return false;
            }
            return true;
        }
        private void OnLogOutExecute(object parameter)
        {
            DataBase.ConnectionDataBase.db.SaveChanges();
            DataBase.ConnectionDataBase.salesman = null;
            InitCountProductInBasket();
        }
        #endregion



        public MainWindowVM()
        {
            Instance = this;

            InitCountProductInBasket();

            ShutdownApplication = new LambdaCommand(OnShutdownApplicationExecute, CanShutdownApplicationExecute);
            MinimazeWindow = new LambdaCommand(OnMinimazeWindowExecute, CanMinimazeWindowExecute);
            MaximizeWindow = new LambdaCommand(OnMaximizeWindowExecute, CanMaximizeWindowExecute);
            DragMoveWindow = new LambdaCommand(OnDragMoveWindowExecute, CanDragMoveWindowExecute);
            GoPage = new LambdaCommand(OnGoPageExecute, CanGoPageExecute);
            GoWindow = new LambdaCommand(OnGoWindowExecute, CanGoWindowExecute);
            ShowOrderPage = new LambdaCommand(OnShowOrderPageExecute, CanShowOrderPageExecute);
            ShowProductPage = new LambdaCommand(OnShowProductPageExecute, CanShowProductPageExecute);
            UserInfo = new LambdaCommand(OnUserInfoExecute, CanUserInfoExecute);
            LogOut = new LambdaCommand(OnLogOutExecute, CanLogOutExecute);
            EditClient = new LambdaCommand(OnEditClientExecute, CanEditClientExecute);
            SearchProduct = new LambdaCommand(OnSearchProductExecute, CanSearchProductExecute);
        }

        #region Methods
        public void InitCountProductInBasket()
        {
            if (ProductsSalesmanVM.Instance == null) return;

            if (DataBase.ConnectionDataBase.salesman != null)
            {
                VisibilityBtn = Visibility.Collapsed;
                VisibilityUserIcon = Visibility.Visible;
                SalesmanDC = DataBase.ConnectionDataBase.salesman;
                ProductsSalesmanVM.Instance.Products = new ObservableCollection<Product>(ConnectionDataBase.db.Product.Local.Where(p => p.Salesman == ConnectionDataBase.salesman));
                ProductsSalesmanVM.Instance.CountProducts = ProductsSalesmanVM.Instance.Products.Count();
                CountOrder = ConnectionDataBase.db.ProductListOrder.Local.Where(o => o.Product.Salesman == ConnectionDataBase.salesman).GroupBy(pr => pr.Order).Count();
            }
            else
            {
                VisibilityBtn = Visibility.Visible;
                VisibilityUserIcon = Visibility.Collapsed;
                UserInfoPanel = Visibility.Collapsed;
                ProductsSalesmanVM.Instance.Products = new ObservableCollection<Product>();
                ProductsSalesmanVM.Instance.CountProducts = 0;
                CountOrder = 0;
            }
        }

        private void SearchProducts()
        {
            // переписать с учётом содержимого добавленных товаров
            var listProductSearched = DataBase.ConnectionDataBase.db.Product.Local.Where(p => p.Name.ToLower().StartsWith(MainWindow.Instance.SearchText.Text.ToLower()));
            CountProduct = listProductSearched.Count();
        }
        #endregion
    }
}
