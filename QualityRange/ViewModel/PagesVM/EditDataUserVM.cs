using DataBase.Model;
using QualityRangeForClient.Commands.Base;
using QualityRangeForClient.View.Pages;
using QualityRangeForClient.View.Windows;
using QualityRangeForClient.ViewModel.WindowsVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace QualityRangeForClient.ViewModel.PagesVM
{
    internal class EditDataUserVM : ViewModel.Base.ViewModel
    {
        public static EditDataUserVM Instance { get; private set; }

        #region Property
        private Client _editingClient = DataBase.ConnectionDataBase.client;
        public Client EditingClient { get => _editingClient; set => Set(ref _editingClient, value); }
        #endregion

        #region Commands
        public ICommand Return { get; }
        private bool CanReturnExecute(object parameter) => true;
        private void OnReturnExecute(object parameter)
        {
            BackPage();
        }

        public ICommand ChangeImage { get; }
        private bool CanChangeImageExecute(object parameter) => true;
        private void OnChangeImageExecute(object parameter)
        {
            var imageFromExplorer = SupportiveClasses.ImageConverter.OpenFileDialogSave();
            if (imageFromExplorer == null)
            {
                return;
            }
            EditingClient.ProfilePhoto = imageFromExplorer;
            EditDataUser.UpdateDataContext();
        }


        public ICommand SaveChanges { get; }
        private bool CanSaveChangesExecute(object parameter)
        {
            (List<string> messages, bool isValid) = ValidatePassword(EditingClient.User.Password);
            if (isValid == false)
            {
                new MessageBox().Show();
                MessageBoxVM.SetMessage(messages.Aggregate((x, y) => x + "\n" + y));
                MessageBoxVM.SetFontSize(10);
                return false;
            }
            if (String.IsNullOrEmpty(EditingClient.Name) || String.IsNullOrEmpty(EditingClient.Surname) || String.IsNullOrEmpty(EditingClient.Patronymic))
            {
                new MessageBox().Show();
                MessageBoxVM.SetMessage("Не заполнено ФИО");
                return false;
            }
            return true;
        }
        private void OnSaveChangesExecute(object parameter)
        {
            BackPage();
            DataBase.ConnectionDataBase.db.SaveChanges();
        }

        private static void BackPage()
        {
            if (EditDataUser.IsConfirm == false)
            {
                MainWindow.Instance.GlobalFrame.Navigate(null);
            }
            else
            {
                MainWindow.Instance.GlobalFrame.Navigate(new ConfirmOrderPage() { DataContext = new ConfirmOrderVM() });
            }
        }
        #endregion

        public EditDataUserVM()
        {
            Instance = this;


            Return = new LambdaCommand(OnReturnExecute, CanReturnExecute);
            ChangeImage = new LambdaCommand(OnChangeImageExecute, CanChangeImageExecute);
            SaveChanges = new LambdaCommand(OnSaveChangesExecute, CanSaveChangesExecute);
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
