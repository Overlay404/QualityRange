using QualityRange.Commands.Base;
using QualityRange.Model;
using QualityRange.View.Pages;
using QualityRange.View.Windows;
using QualityRange.ViewModel.WindowsVM;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QualityRange.ViewModel.PagesVM
{
    internal class EditDataUserVM : ViewModel.Base.ViewModel
    {
        public static EditDataUserVM Instance { get; private set; }

        #region Property
        private Client _editingClient = App.user.Client;
        public Client EditingClient { get => _editingClient; set => Set(ref _editingClient, value); }
        #endregion

        #region Commands
        public ICommand Return { get; }
        private bool CanReturnExecute(object parameter) => true;
        private void OnReturnExecute(object parameter)
        {
            MainWindow.Instance.GlobalFrame.GoBack();
        }

        public ICommand ChangeImage { get; }
        private bool CanChangeImageExecute(object parameter) => true;
        private void OnChangeImageExecute(object parameter)
        {
            EditingClient.ProfilePhoto = SupportiveClasses.ImageConverter.OpenFileDialogSave();
        }


        public ICommand SaveChanges { get; }
        private bool CanSaveChangesExecute(object parameter)
        {
            return true;
        }
        private void OnSaveChangesExecute(object parameter)
        {
            MainWindow.Instance.GlobalFrame.GoBack();
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
