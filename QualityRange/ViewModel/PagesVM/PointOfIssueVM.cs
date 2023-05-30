using QualityRange.Commands.Base;
using QualityRange.Model;
using QualityRange.View.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QualityRange.ViewModel.PagesVM
{
    internal class PointOfIssueVM : ViewModel.Base.ViewModel
    {
        public static PointOfIssueVM Instance { get; private set; }

        #region Property
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
        #endregion


        public PointOfIssueVM()
        {
            Instance = this;

            Return = new LambdaCommand(OnReturnExecute, CanReturnExecute);
        }
    }
}
