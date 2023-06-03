﻿using DataBase.Model;
using QualityRangeForSalesman.Commands.Base;
using QualityRangeForSalesman.View.Pages;
using QualityRangeForSalesman.View.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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


        private IEnumerable<Category> _categories;
        public IEnumerable<Category> Categories { get => _categories; set => Set(ref _categories, value); }


        private Client _clientDC;
        public Client ClientDC { get => _clientDC; set => Set(ref _clientDC, value); }


        private Visibility _visibilityBtn = Visibility.Visible;
        public Visibility VisibilityBtn { get => _visibilityBtn; set => Set(ref _visibilityBtn, value); }


        private Visibility _visibilityUserIcon = Visibility.Collapsed;
        public Visibility VisibilityUserIcon { get => _visibilityUserIcon; set => Set(ref _visibilityUserIcon, value); }


        private Visibility _userInfoPanel = Visibility.Collapsed;
        public Visibility UserInfoPanel { get => _userInfoPanel; set => Set(ref _userInfoPanel, value); }


        private int _countOrder;
        public int CountOrder { get => _countOrder; set => Set(ref _countOrder, value); }
        #endregion


        #region Commands
        public ICommand ShutdownApplication { get; }
        private bool CanShutdownApplicationExecute(object parameter) => true;
        private void OnShutdownApplicationExecute(object parameter) => App.Current.Shutdown();

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
        private void OnEditClientExecute(object parameter)
        {

        }

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

        public ICommand ShowOrderPage { get; }
        private void OnShowOrderPageExecute(object parameter)
        {

        }
        private bool CanShowOrderPageExecute(object parameter)
        {
            if (DataBase.ConnectionDataBase.client == null)
            {
                new AuthRegWindow().Show();
                return false;
            }
            return true;
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

        public ICommand CategoryPress { get; }
        private bool CanCategoryPressExecute(object parameter)
        {
            if (parameter as string == "Все категории")
            {
                CountProduct = DataBase.ConnectionDataBase.db.Product.Local.Count();
                return false;
            }
            return true;
        }
        private void OnCategoryPressExecute(object parameter)
        {
            var products = new ObservableCollection<Product>(DataBase.ConnectionDataBase.db.Product.Local.Where(p => p.Category.Name == parameter as string));
            CountProduct = products.Count();
        }
        #endregion


        public ICommand LogOut { get; }
        private bool CanLogOutExecute(object parameter) => true;
        private void OnLogOutExecute(object parameter)
        {
            DataBase.ConnectionDataBase.db.SaveChanges();
            DataBase.ConnectionDataBase.client = null;
            InitCountProductInBasket();
        }



        public MainWindowVM()
        {
            Instance = this;

            InitCountProductInBasket();

            Categories = new List<Category>(DataBase.ConnectionDataBase.db.Category.Local).Append(new Category { Name = "Все категории" });

            ShutdownApplication = new LambdaCommand(OnShutdownApplicationExecute, CanShutdownApplicationExecute);
            MinimazeWindow = new LambdaCommand(OnMinimazeWindowExecute, CanMinimazeWindowExecute);
            MaximizeWindow = new LambdaCommand(OnMaximizeWindowExecute, CanMaximizeWindowExecute);
            DragMoveWindow = new LambdaCommand(OnDragMoveWindowExecute, CanDragMoveWindowExecute);
            GoPage = new LambdaCommand(OnGoPageExecute, CanGoPageExecute);
            GoWindow = new LambdaCommand(OnGoWindowExecute, CanGoWindowExecute);
            ShowOrderPage = new LambdaCommand(OnShowOrderPageExecute, CanShowOrderPageExecute);
            CategoryPress = new LambdaCommand(OnCategoryPressExecute, CanCategoryPressExecute);
            UserInfo = new LambdaCommand(OnUserInfoExecute, CanUserInfoExecute);
            LogOut = new LambdaCommand(OnLogOutExecute, CanLogOutExecute);
            EditClient = new LambdaCommand(OnEditClientExecute, CanEditClientExecute);
            SearchProduct = new LambdaCommand(OnSearchProductExecute, CanSearchProductExecute);
        }

        public void InitCountProductInBasket()
        {
            if (DataBase.ConnectionDataBase.client != null)
            {
                VisibilityBtn = Visibility.Collapsed;
                VisibilityUserIcon = Visibility.Visible;
                ClientDC = DataBase.ConnectionDataBase.client;
            }
            else
            {
                VisibilityBtn = Visibility.Visible;
                VisibilityUserIcon = Visibility.Collapsed;
                UserInfoPanel = Visibility.Collapsed;
            }

            //CountOrder = вывод количества заказов
        }

        private void SearchProducts()
        {
            // переписать с учётом содержимого добавленных товаров
            var listProductSearched = DataBase.ConnectionDataBase.db.Product.Local.Where(p => p.Name.ToLower().StartsWith(MainWindow.Instance.SearchText.Text.ToLower()));
            CountProduct = listProductSearched.Count();
        }
    }
}
