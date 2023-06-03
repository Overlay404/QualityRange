using DataBase.Model;
using QualityRangeForClient.Commands.Base;
using QualityRangeForClient.View.Windows;
using System.Collections.Generic;
using System.Windows.Input;

namespace QualityRangeForClient.ViewModel.PagesVM
{
    internal class AboutProductVM : ViewModel.Base.ViewModel
    {
        public static AboutProductVM Instance { get; private set; }

        #region Property
        private Product _product;
        public Product Product { get => _product; set => Set(ref _product, value); }

        private byte[] _selectedImage;
        public byte[] SelectedImage { get => _selectedImage; set => Set(ref _selectedImage, value); }

        private IEnumerable<PhotoProduct> _images;
        public IEnumerable<PhotoProduct> Images { get => _images; set => Set(ref _images, value); }
        #endregion

        #region Commands
        public ICommand Return { get; }
        private bool CanReturnExecute(object parameter) => true;
        private void OnReturnExecute(object parameter)
        {
            MainWindow.Instance.GlobalFrame.Navigate(null);
        }

        public ICommand SelectImage { get; }
        private bool CanSelectImageExecute(object parameter) => true;
        private void OnSelectImageExecute(object parameter)
        {
            SelectedImage = (byte[])parameter;
        }
        #endregion


        public AboutProductVM()
        {
            Instance = this;

            SelectImage = new LambdaCommand(OnSelectImageExecute, CanSelectImageExecute);
            Return = new LambdaCommand(OnReturnExecute, CanReturnExecute);
        }
    }
}
