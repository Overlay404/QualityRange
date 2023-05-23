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
        #region Property
        private Page _barsViewPage = new BarsViewProductPage();
        public Page BarsViewPage { get => _barsViewPage; set => Set(ref _barsViewPage, value); }


        private Page _gridViewPage = new GridViewProductPanel();
        public Page GridViewPage { get => _gridViewPage; set => Set(ref _gridViewPage, value); }

        private int _countProduct = App.db.Product.Local.Count();
        public int CountProduct { get => _countProduct; set => Set(ref _countProduct, value); }

        private IEnumerable<Category> _category = App.db.Category.Local;
        public IEnumerable<Category> Category { get => _category; set => Set(ref _category, value); }
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


        public ICommand CategoryPress { get; }
        private bool CanCategoryPressExecute(object parameter) => true;
        private void OnCategoryPressExecute(object parameter)
        {
            MessageBox.Show($"asd{parameter as string}");
        }
        #endregion


        public MainWindowVM()
        {
            ShutdownApplication = new LambdaCommand(OnShutdownApplicationExecute, CanShutdownApplicationExecute);
            MinimazeWindow = new LambdaCommand(OnMinimazeWindowExecute, CanMinimazeWindowExecute);
            MaximizeWindow = new LambdaCommand(OnMaximizeWindowExecute, CanMaximizeWindowExecute);
            DragMoveWindow = new LambdaCommand(OnDragMoveWindowExecute, CanDragMoveWindowExecute);
            GoPage = new LambdaCommand(OnGoPageExecute, CanGoPageExecute);
            CategoryPress = new LambdaCommand(OnCategoryPressExecute, CanCategoryPressExecute);
        }
    }
}
