using DataBase.Model;
using QualityRangeForEmployee.Commands.Base;
using QualityRangeForEmployee.View.Pages;
using QualityRangeForEmployee.View.Windows;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QualityRangeForEmployee.ViewModel.WindowsVM
{
    internal class MainWindowVM : Base.ViewModel
    {
        public static MainWindowVM Instance { get; private set; }

        #region Property
        private Window _authRegWin = new AuthRegWindow();
        public Window AuthRegWin { get => _authRegWin; set => Set(ref _authRegWin, value); }

        private Page _requestProductPage = new RequestProductPage();
        public Page RequestProductPage { get => _requestProductPage; set => Set(ref _requestProductPage, value); }

        private Page _userPage = new UserPage();
        public Page UserPage { get => _userPage; set => Set(ref _userPage, value); }

        private Page _statisticPage = new StatisticPage();
        public Page StatisticPage { get => _statisticPage; set => Set(ref _statisticPage, value); }

        private Page _pointOfIssuePage = new PointOfIssuePage();
        public Page PointOfIssuePage { get => _pointOfIssuePage; set => Set(ref _pointOfIssuePage, value); }

        private Employee _salesmantDC;
        public Employee EmployeeDC { get => _salesmantDC; set => Set(ref _salesmantDC, value); }

        private Visibility _visibilityBtn = Visibility.Visible;
        public Visibility VisibilityBtn { get => _visibilityBtn; set => Set(ref _visibilityBtn, value); }

        private Visibility _visibilityUserIcon = Visibility.Collapsed;
        public Visibility VisibilityUserIcon { get => _visibilityUserIcon; set => Set(ref _visibilityUserIcon, value); }

        private Visibility _userInfoPanel = Visibility.Collapsed;
        public Visibility UserInfoPanel { get => _userInfoPanel; set => Set(ref _userInfoPanel, value); }
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

        public ICommand MaximizeWindow { get; }
        private bool CanMaximizeWindowExecute(object parameter) => true;
        private void OnMaximizeWindowExecute(object parameter) => MainWindow.Instance.WindowState = WindowState.Maximized;

        public ICommand ShutdownApplication { get; }
        private bool CanShutdownApplicationExecute(object parameter) => true;
        private void OnShutdownApplicationExecute(object parameter) => App.Current.Shutdown();

        public ICommand EditClient { get; }
        private bool CanEditClientExecute(object parameter) => true;
        private void OnEditClientExecute(object parameter)
        {
            MainWindow.Instance.GlobalFrame.Navigate(new EditDataUser());
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
        private bool CanLogOutExecute(object parameter) => true;
        private void OnLogOutExecute(object parameter)
        {
            DataBase.ConnectionDataBase.db.SaveChanges();
            DataBase.ConnectionDataBase.employee = null;
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
            UserInfo = new LambdaCommand(OnUserInfoExecute, CanUserInfoExecute);
            LogOut = new LambdaCommand(OnLogOutExecute, CanLogOutExecute);
            EditClient = new LambdaCommand(OnEditClientExecute, CanEditClientExecute);
        }

        #region Methods
        public void InitCountProductInBasket()
        {
           

            if (DataBase.ConnectionDataBase.employee != null)
            {
                VisibilityBtn = Visibility.Collapsed;
                VisibilityUserIcon = Visibility.Visible;
                EmployeeDC = DataBase.ConnectionDataBase.employee;
            }
            else
            {
                VisibilityBtn = Visibility.Visible;
                VisibilityUserIcon = Visibility.Collapsed;
                UserInfoPanel = Visibility.Collapsed;
            }
        }

        
        #endregion
    }
}
