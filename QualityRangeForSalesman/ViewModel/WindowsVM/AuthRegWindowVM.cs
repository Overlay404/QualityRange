using DataBase;
using DataBase.Model;
using QualityRangeForSalesman.Commands.Base;
using QualityRangeForSalesman.View.Pages;
using QualityRangeForSalesman.View.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Threading;

namespace QualityRangeForSalesman.ViewModel.WindowsVM
{
    internal class AuthRegWindowVM : Base.ViewModel
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

            if (AutorizateUser(loginIntroduced, passwordIntroduced) == false)
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


            if (String.IsNullOrWhiteSpace(loginIntroduced) && String.IsNullOrWhiteSpace(passwordIntroduced) && String.IsNullOrWhiteSpace(nameIntroduced))
            {
                AddMessageInTextBlock("Поля не заполнены");
                return;
            }

            (List<string> messages, bool IsValid) = ValidatePassword(passwordIntroduced);

            if (IsValid == false)
            {
                AddMessageInTextBlock(messages.Aggregate((x, y) => x + "\n" + y));
                return;
            }


            if (ConnectionDataBase.db.User.Local.Select(u => u.Login).Contains(loginIntroduced) == true)
            {
                AddMessageInTextBlock("Такой логин уже используется");
                return;
            }

            var user = new User()
            {
                Login = loginIntroduced,
                Password = passwordIntroduced,
                Removed = false
            };

            DataBase.ConnectionDataBase.db.User.Local.Add(user);

            DataBase.ConnectionDataBase.db.Salesman.Local.Add(new Salesman()
            {
                NameCompany = nameIntroduced,
                DateOnMarketplace = DateTime.Today,
                User = user
            });


            DataBase.ConnectionDataBase.db.SaveChanges();

            if (AutorizateUser(loginIntroduced, passwordIntroduced) == false)
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
        }

        private static bool AutorizateUser(string loginIntroduced, string passwordIntroduced)
        {
            DataBase.ConnectionDataBase.salesman = DataBase.ConnectionDataBase.db.User.FirstOrDefault(u => u.Login == loginIntroduced && u.Password == passwordIntroduced)?.Salesman;

            if (DataBase.ConnectionDataBase.salesman == null)
            {
                AddMessageInTextBlock("Такого пользователя не существует");
                return false;
            }

            if (DataBase.ConnectionDataBase.salesman.User.Removed == true)
            {
                AddMessageInTextBlock("Аккаунт заблокирован");
                return false;
            }

            if (DataBase.ConnectionDataBase.salesman.ProfilePhoto == null)
            {
                DataBase.ConnectionDataBase.salesman.ProfilePhoto = DataBase.ConnectionDataBase.EmptyUserImage;
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


        public static (List<string>, bool) ValidatePassword(string password)
        {
            List<string> message = new List<string>();

            if (password.Length < 6)
                message.Add("Пароль должен быть не менее 6 символов");
            if (!password.Any(c => Char.IsUpper(c)))
                message.Add("В пароле должна быть хотя бы одна прописная буква");
            if (!Regex.IsMatch(password, @"\d"))
                message.Add("В пароле должна быть хотя бы одна цифра");
            if (!Regex.IsMatch(password, @"[!@#$%^]"))
                message.Add("В пароле должен быть хотя бы одний из символов ! @ # $ % ^");

            return message.Count == 0 ? (null, true) : (message, false);
        }
    }
}
