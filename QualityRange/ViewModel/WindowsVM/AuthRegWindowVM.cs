using QualityRange.View.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using QualityRange.Commands.Base;
using QualityRange.View.Pages;
using QualityRange.Model;
using System.Windows.Controls;
using System.Windows.Threading;

namespace QualityRange.ViewModel.WindowsVM
{
    internal class AuthRegWindowVM : ViewModel.Base.ViewModel
    {
        public static DispatcherTimer timer;

        #region Commands
        public ICommand ShutdownApplication { get; }
        private bool CanShutdownApplicationExecute(object parameter) => true;
        private void OnShutdownApplicationExecute(object parameter)
        {
            AuthRegWindow.Instance.Hide();
        }

        public ICommand DragMoveWindow { get; }
        private bool CanDragMoveWindowExecute(object parameter) => true;
        private void OnDragMoveWindowExecute(object parameter)
        {
            AuthRegWindow.Instance.DragMove();
        }


        public ICommand SignIn { get; }
        private bool CanSignInExecute(object parameter) => true;
        private void OnSignInExecute(object parameter)
        {
            ClearMessageInTextBlock(null, null);

            var loginIntroduced = AutorizationPage.Instance.LoginTB.Text;
            var passwordIntroduced = AutorizationPage.Instance.PasswordTB.Text;

            if (String.IsNullOrWhiteSpace(loginIntroduced) && String.IsNullOrWhiteSpace(passwordIntroduced))
            {
                AddMessageInTextBlock("Поля не заполнены");
                return;
            }

            App.user = App.db.User.FirstOrDefault(u => u.Login.Equals(loginIntroduced.Trim()) && u.Password.Equals(passwordIntroduced.Trim()) && u.Removed == false);

            if (App.user == null)
            {
                AddMessageInTextBlock("Такого пользователя не существует");
                return;
            }

            AuthRegWindow.Instance.Close();
            MainWindowVM.Instance.InitCountProductInBasket();
            GridAndBarsViewProductPanelVM.Instance.InitProductList();
        }

        public ICommand LogIn { get; }
        private bool CanLogInExecute(object parameter) => true;
        private void OnLogInExecute(object parameter)
        {

        }
        #endregion


        private static DispatcherTimer InitDispatcher()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(2500);
            timer.Tick += ClearMessageInTextBlock;
            return timer;
        }

        private static void AddMessageInTextBlock(string text)
        {
            timer = timer ?? InitDispatcher();
            timer.Start();

            AuthRegWindow.Instance.ShowMessageTextBlock.Text = text;
        }

        private static void ClearMessageInTextBlock(object sender, EventArgs e)
        {
            AuthRegWindow.Instance.ShowMessageTextBlock.Text = String.Empty;
        }


        public AuthRegWindowVM()
        {
            ShutdownApplication = new LambdaCommand(OnShutdownApplicationExecute, CanShutdownApplicationExecute);
            DragMoveWindow = new LambdaCommand(OnDragMoveWindowExecute, CanDragMoveWindowExecute);
            SignIn = new LambdaCommand(OnSignInExecute, CanSignInExecute);
            LogIn = new LambdaCommand(OnLogInExecute, CanLogInExecute);
        }
    }
}
