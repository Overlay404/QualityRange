using QualityRange.Commands.Base;
using QualityRange.Model;
using QualityRange.View.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace QualityRange.ViewModel.PagesVM
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
            MainWindow.Instance.BasketFrame.Navigate(null);
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
