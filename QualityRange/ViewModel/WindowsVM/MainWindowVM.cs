﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using QualityRange.Commands.Base;
using QualityRange.Model;
using QualityRange.View.Pages;
using QualityRange.View.Windows;
using QualityRange.ViewModel.Base;

namespace QualityRange.ViewModel
{
    internal class MainWindowVM : ViewModel.Base.ViewModel
    {
        public static MainWindowVM Instance { get; private set; }

        #region Property
        private Page _barsViewPage = new BarsViewProductPage() { DataContext = GridAndBarsViewProductPanelVM.Instance };
        public Page BarsViewPage { get => _barsViewPage; set => Set(ref _barsViewPage, value); }


        private Page _gridViewPage = new GridViewProductPanel() { DataContext = GridAndBarsViewProductPanelVM.Instance };
        public Page GridViewPage { get => _gridViewPage; set => Set(ref _gridViewPage, value); }


        private Window _authRegWin = new AuthRegWindow();
        public Window AuthRegWin { get => _authRegWin; set => Set(ref _authRegWin, value); }


        private int _countProduct = App.db.Product.Local.Count();
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


        private int _countProductInBasket;
        public int CountProductInBasket { get => _countProductInBasket; set => Set(ref _countProductInBasket, value); }
        #endregion


        #region Commands
        public ICommand ShutdownApplication { get; }
        private bool CanShutdownApplicationExecute(object parameter) => true;
        private void OnShutdownApplicationExecute(object parameter)
        {
            App.Current.Shutdown();
        }


        public ICommand MinimazeWindow { get; }
        private bool CanMinimazeWindowExecute(object parameter) => true;
        private void OnMinimazeWindowExecute(object parameter)
        {
            MainWindow.Instance.WindowState = WindowState.Minimized;
        }


        public ICommand MaximizeWindow { get; }
        private bool CanMaximizeWindowExecute(object parameter)
        {
            if (MainWindow.Instance.WindowState == WindowState.Maximized)
            {
                MainWindow.Instance.WindowState = WindowState.Normal;
                return false;
            };
            return true;
        }
        private void OnMaximizeWindowExecute(object parameter)
        {
            MainWindow.Instance.WindowState = WindowState.Maximized;
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


        public ICommand GoPage { get; }
        private bool CanGoPageExecute(object parameter) => true;
        private void OnGoPageExecute(object parameter)
        {
            MainWindow.Instance.ProductListFrame.Navigate(parameter);
        }


        public ICommand GoWindow { get; }
        private bool CanGoWindowExecute(object parameter) => true;
        private void OnGoWindowExecute(object parameter)
        {
            new AuthRegWindow().Show();
        }
        
        
        public ICommand ShowBasketPage { get; }
        private bool CanShowBasketPageExecute(object parameter)
        {
            if (App.user == null)
            {
                new AuthRegWindow().Show();
                return false;
            }
            return true;
        }
        private void OnShowBasketPageExecute(object parameter)
        {
            MainWindow.Instance.BasketFrame.Navigate(new BasketPage());
        }


        public ICommand CategoryPress { get; }
        private bool CanCategoryPressExecute(object parameter)
        {
            if (parameter as string == "Все категории")
            {
                GridAndBarsViewProductPanelVM.Instance.Products = App.db.Product.Local;
                CountProduct = App.db.Product.Local.Count();
                return false;
            }
            return true;
        }
        private void OnCategoryPressExecute(object parameter)
        {
            var products = new ObservableCollection<Product>(App.db.Product.Local.Where(p => p.Category.Name == parameter as string));
            GridAndBarsViewProductPanelVM.Instance.Products = products;
            CountProduct = products.Count();
        }
        
        
        public ICommand UserInfo { get; }
        private bool CanUserInfoExecute(object parameter) => true;
        private void OnUserInfoExecute(object parameter)
        {
            UserInfoPanel = UserInfoPanel ==  Visibility.Visible? Visibility.Collapsed : Visibility.Visible;
        }
        
        
        public ICommand LogOut { get; }
        private bool CanLogOutExecute(object parameter) => true;
        private void OnLogOutExecute(object parameter)
        {
            App.user = null;
            InitCountProductInBasket();
            GridAndBarsViewProductPanelVM.Instance.Products = new ObservableCollection<Product>(App.db.Product.Local);
            GridAndBarsViewProductPanelVM.Instance.InitProductList();
        }
        #endregion


        public MainWindowVM()
        {
            Instance = this;

            InitCountProductInBasket();

            Categories = new List<Category>(App.db.Category.Local).Append(new Category { Name = "Все категории" });

            ShutdownApplication = new LambdaCommand(OnShutdownApplicationExecute, CanShutdownApplicationExecute);
            MinimazeWindow = new LambdaCommand(OnMinimazeWindowExecute, CanMinimazeWindowExecute);
            MaximizeWindow = new LambdaCommand(OnMaximizeWindowExecute, CanMaximizeWindowExecute);
            DragMoveWindow = new LambdaCommand(OnDragMoveWindowExecute, CanDragMoveWindowExecute);
            GoPage = new LambdaCommand(OnGoPageExecute, CanGoPageExecute);
            GoWindow = new LambdaCommand(OnGoWindowExecute, CanGoWindowExecute);
            ShowBasketPage = new LambdaCommand(OnShowBasketPageExecute, CanShowBasketPageExecute);
            CategoryPress = new LambdaCommand(OnCategoryPressExecute, CanCategoryPressExecute);
            UserInfo = new LambdaCommand(OnUserInfoExecute, CanUserInfoExecute);
            LogOut = new LambdaCommand(OnLogOutExecute, CanLogOutExecute);
        }

        public void InitCountProductInBasket()
        {
            if(App.user != null)
            {
                VisibilityBtn = Visibility.Collapsed;
                VisibilityUserIcon = Visibility.Visible;
                ClientDC = App.user.Client;
            }
            else
            {
                VisibilityBtn = Visibility.Visible;
                VisibilityUserIcon = Visibility.Collapsed;
                UserInfoPanel = Visibility.Collapsed;
            }

            CountProductInBasket = (int)(App.db.ProductList.Local.Where(pl => pl.Basket.Client.User == App.user)?.Count());
        }
    }
}
