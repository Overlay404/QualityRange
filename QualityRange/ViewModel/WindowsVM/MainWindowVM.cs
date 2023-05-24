using System;
using System.Collections.Generic;
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


        private IEnumerable<Category> _category = App.db.Category.Local;
        public IEnumerable<Category> Category { get => _category; set => Set(ref _category, value); }


        private int _countProductInBasket;
        public int CountProductInBasket { get => _countProductInBasket; set => Set(ref _countProductInBasket, value); }


        private Visibility _visibilityCountProductInBasket;
        public Visibility VisibilityCountProductInBasket { get => _visibilityCountProductInBasket; set => Set(ref _visibilityCountProductInBasket, value); }
        
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
        private bool CanMaximizeWindowExecute(object parameter) => true;
        private void OnMaximizeWindowExecute(object parameter)
        {
            if (MainWindow.Instance.WindowState == WindowState.Maximized)
            {
                MainWindow.Instance.WindowState = WindowState.Normal;
                return;
            }

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
            (parameter as Window).ShowDialog();
        }


        public ICommand CategoryPress { get; }
        private bool CanCategoryPressExecute(object parameter) => true;
        private void OnCategoryPressExecute(object parameter)
        {
            GridAndBarsViewProductPanelVM.Instance.Products = App.db.Product.Local.Where(p => p.Category.Name == parameter as string);
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
            CategoryPress = new LambdaCommand(OnCategoryPressExecute, CanCategoryPressExecute);
        }

        public void InitCountProductInBasket()
        {
            CountProductInBasket = (int)(App.db.ProductList.Local.Where(pl => pl.Basket.Client.User == App.user)?.Count());
            VisibilityCountProductInBasket = CountProductInBasket == 0 ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
