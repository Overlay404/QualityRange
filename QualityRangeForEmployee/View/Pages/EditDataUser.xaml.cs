﻿using QualityRangeForEmployee.SupportiveClasses;
using QualityRangeForEmployee.View.Windows;
using QualityRangeForEmployee.ViewModel.PagesVM;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace QualityRangeForEmployee.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditDataUser.xaml
    /// </summary>
    public partial class EditDataUser : Page
    {
        public static EditDataUser Instance { get; set; }

        public static bool IsConfirm { get; set; }

        public EditDataUser(bool isConfirm = false)
        {
            IsConfirm = isConfirm;
            InitializeComponent();
            Instance = this;

            NameClient.KeyDown += (sender, e) =>
            {
                HandledDigit(e);
            };

            SurnameClient.KeyDown += (sender, e) =>
            {
                HandledDigit(e);
            };

            PatronymicClient.KeyDown += (sender, e) =>
            {
                HandledDigit(e);
            };
        }

        #region Methods
        private static void HandledLetter(KeyEventArgs e)
        {
            if (new KeyConverter().ConvertToString(e.Key).All(letter => char.IsLetter(letter)))
                e.Handled = true;
        }

        private static void HandledDigit(KeyEventArgs e)
        {
            if (new KeyConverter().ConvertToString(e.Key).All(letter => char.IsDigit(letter)))
                e.Handled = true;
        }

        public static void UpdateDataContext()
        {
            var InsVM = new EditDataUserVM();
            Instance.DataContext = InsVM;
            Instance.GridRoot.DataContext = InsVM.EditingEmployee;
            Instance.ProfilePhotoImage.Source = ImageConverter.ConvertToImageSource(InsVM.EditingEmployee.ProfilePhoto);
            MainWindow.Instance.ProfilePhotoImage.Source = ImageConverter.ConvertToImageSource(InsVM.EditingEmployee.ProfilePhoto);
        } 
        #endregion
    }
}
