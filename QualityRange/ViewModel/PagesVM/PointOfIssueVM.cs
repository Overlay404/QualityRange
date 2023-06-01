using QualityRange.Commands.Base;
using QualityRange.Model;
using QualityRange.View.Pages;
using QualityRange.View.Windows;
using QualityRange.ViewModel.WindowsVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MessageBox = QualityRange.View.Windows.MessageBox;

namespace QualityRange.ViewModel.PagesVM
{
    internal class PointOfIssueVM : ViewModel.Base.ViewModel
    {
        public static PointOfIssueVM Instance { get; private set; }

        #region Property
        private IEnumerable<PhotoProduct> _images;
        public IEnumerable<PhotoProduct> Images { get => _images; set => Set(ref _images, value); }
        
        private IEnumerable<PointOfIssue> _pointOfIssueList;
        public IEnumerable<PointOfIssue> PointOfIssueList { get => _pointOfIssueList; set => Set(ref _pointOfIssueList, value); }
        #endregion

        #region Commands
        public ICommand Return { get; }
        private bool CanReturnExecute(object parameter) => true;
        private void OnReturnExecute(object parameter)
        {
            MainWindow.Instance.GlobalFrame.GoBack();
        }
        
        public ICommand ShowPosition { get; }
        private bool CanShowPositionExecute(object parameter) => true;
        private void OnShowPositionExecute(object parameter)
        {
            var itemPointOfIssue = App.db.PointOfIssue.Local.FirstOrDefault(p => p.ID == (int)parameter);
            PointOfIssuePage.Instance.ListViewPointOfIssie.SelectedItem = itemPointOfIssue;
            PointOfIssuePage.Instance.gMapControl1.Position = new GMap.NET.PointLatLng((double)itemPointOfIssue.lat, (double)itemPointOfIssue.lot);
        } 
        
        
        public ICommand NextStage { get; }
        private bool CanNextStageExecute(object parameter)
        {
            if (PointOfIssuePage.Instance.ListViewPointOfIssie.SelectedItem == null)
            {
                new MessageBox().Show();
                MessageBoxVM.SetMessage("Не выбран не один пункт выдачи");
                return false;
            }
            return true;
        }
        private void OnNextStageExecute(object parameter)
        {
            MainWindow.Instance.GlobalFrame.Navigate(new ConfirmOrderPage());
        }
        #endregion


        public PointOfIssueVM()
        {
            Instance = this;

            PointOfIssueList = App.db.PointOfIssue.Local;

            Return = new LambdaCommand(OnReturnExecute, CanReturnExecute);
            ShowPosition = new LambdaCommand(OnShowPositionExecute, CanShowPositionExecute);
            NextStage = new LambdaCommand(OnNextStageExecute, CanNextStageExecute);
        }
    }
}
