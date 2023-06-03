using DataBase.Model;
using QualityRangeForClient.Commands.Base;
using QualityRangeForClient.View.Pages;
using QualityRangeForClient.View.Windows;
using QualityRangeForClient.ViewModel.PagesVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;

namespace QualityRangeForClient.ViewModel.WindowsVM
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

            if (AutorizateClient(loginIntroduced, passwordIntroduced) == false)
            {
                return;
            }

            ClosingPageAuthReg();
        }


        public ICommand LogIn { get; }
        private bool CanLogInExecute(object parameter) => true;
        private void OnLogInExecute(object parameter)
        {
            var loginIntroduced = RegistrationPage.Instance.LoginTB.Text;
            var passwordIntroduced = RegistrationPage.Instance.PasswordTB.Text;
            var nameIntroduced = RegistrationPage.Instance.NameTB.Text;
            var surnameIntroduced = RegistrationPage.Instance.SurnameTB.Text;
            var patronymicIntroduced = RegistrationPage.Instance.PatronymicTB.Text;


            if (String.IsNullOrWhiteSpace(loginIntroduced) && String.IsNullOrWhiteSpace(passwordIntroduced)
                && String.IsNullOrWhiteSpace(nameIntroduced) && String.IsNullOrWhiteSpace(surnameIntroduced)
                && String.IsNullOrWhiteSpace(patronymicIntroduced))
            {
                AddMessageInTextBlock("Поля не заполнены");
                return;
            }

            (List<string> messages, bool IsValid) = EditDataUserVM.ValidatePassword(passwordIntroduced);

            if (IsValid == false)
            {
                AddMessageInTextBlock(messages.Aggregate((x, y) => x + "\n" + y));
                return;
            }

            var user = new User()
            {
                Login = loginIntroduced,
                Password = passwordIntroduced,
                Removed = false
            };

            DataBase.ConnectionDataBase.db.User.Local.Add(user);

            DataBase.ConnectionDataBase.db.Client.Local.Add(new Client()
            {
                Name = nameIntroduced,
                Surname = surnameIntroduced,
                Patronymic = patronymicIntroduced,
                User = user
            });

            DataBase.ConnectionDataBase.db.SaveChanges();

            if (AutorizateClient(loginIntroduced, passwordIntroduced) == false)
            {
                return;
            }

            ClosingPageAuthReg();
        }
        #endregion

        #region Methods
        private static void ClosingPageAuthReg()
        {
            AuthRegWindow.Instance.Close();
            MainWindowVM.Instance.InitCountProductInBasket();
            GridAndBarsViewProductPanelVM.Instance.InitProductList();
        }

        private static bool AutorizateClient(string loginIntroduced, string passwordIntroduced)
        {
            DataBase.ConnectionDataBase.client = DataBase.ConnectionDataBase.db.User.FirstOrDefault(u => u.Login.Equals(loginIntroduced.Trim()) && u.Password.Equals(passwordIntroduced.Trim()) && u.Removed == false).Client;

            if (DataBase.ConnectionDataBase.client == null)
            {
                AddMessageInTextBlock("Такого пользователя не существует");
                return false;
            }

            if (DataBase.ConnectionDataBase.client.ProfilePhoto == null)
            {
                DataBase.ConnectionDataBase.client.ProfilePhoto = DataBase.ConnectionDataBase.EmptyUserImage;
            }
            return true;
        }

        private static DispatcherTimer InitDispatcher()
        {
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(2500)
            };
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
        #endregion


        public AuthRegWindowVM()
        {
            ShutdownApplication = new LambdaCommand(OnShutdownApplicationExecute, CanShutdownApplicationExecute);
            DragMoveWindow = new LambdaCommand(OnDragMoveWindowExecute, CanDragMoveWindowExecute);
            SignIn = new LambdaCommand(OnSignInExecute, CanSignInExecute);
            LogIn = new LambdaCommand(OnLogInExecute, CanLogInExecute);
        }
    }
}
