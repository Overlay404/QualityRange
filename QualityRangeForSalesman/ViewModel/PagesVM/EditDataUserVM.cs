using DataBase.Model;
using QualityRangeForSalesman.Commands.Base;
using QualityRangeForSalesman.View.Pages;
using QualityRangeForSalesman.View.Windows;
using QualityRangeForSalesman.ViewModel.WindowsVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace QualityRangeForSalesman.ViewModel.PagesVM
{
    internal class EditDataUserVM : ViewModel.Base.ViewModel
    {
        public static EditDataUserVM Instance { get; private set; }

        #region Property
        private Salesman _editingSalesman = DataBase.ConnectionDataBase.salesman;
        public Salesman EditingSalesman { get => _editingSalesman; set => Set(ref _editingSalesman, value); }
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
            EditingSalesman.ProfilePhoto = imageFromExplorer;
            EditDataUser.UpdateDataContext();
        }


        public ICommand SaveChanges { get; }
        private bool CanSaveChangesExecute(object parameter)
        {
            (List<string> messages, bool isValid) = AuthRegWindowVM.ValidatePassword(EditingSalesman.User.Password);
            if (isValid == false)
            {
                new MessageBox().Show();
                MessageBoxVM.SetMessage(messages.Aggregate((x, y) => x + "\n" + y));
                MessageBoxVM.SetFontSize(10);
                return false;
            }
            if (String.IsNullOrEmpty(EditingSalesman.NameCompany))
            {
                new MessageBox().Show();
                MessageBoxVM.SetMessage("Не введено название огранизации");
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
            MainWindow.Instance.GlobalFrame.Navigate(null);
        }
        #endregion

        public EditDataUserVM()
        {
            Instance = this;

            Return = new LambdaCommand(OnReturnExecute, CanReturnExecute);
            ChangeImage = new LambdaCommand(OnChangeImageExecute, CanChangeImageExecute);
            SaveChanges = new LambdaCommand(OnSaveChangesExecute, CanSaveChangesExecute);
        }
    }
}
