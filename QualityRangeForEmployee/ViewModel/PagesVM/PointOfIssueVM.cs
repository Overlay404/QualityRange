using DataBase;
using DataBase.Model;
using QualityRangeForEmployee.Commands.Base;
using QualityRangeForEmployee.View.Pages;
using QualityRangeForEmployee.View.Windows;
using QualityRangeForEmployee.ViewModel.WindowsVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QualityRangeForEmployee.ViewModel.PagesVM
{
    internal class PointOfIssueVM : ViewModel.Base.ViewModel
    {
        public static PointOfIssueVM Instance { get; private set; }

        public Regex LatRegex = new Regex(@"(^\d\,\d{1,14}$|^\d$)|(^[0-8]\d\,\d{1,14}$|^[0-8]\d$)|(^90\,[0]{1,14}|^90$)");
        public Regex LonRegex = new Regex(@"(^\d\,\d{1,14}$|^\d$)|(^\d{2}\,\d{1,14}$|^\d{2}$)|(^[1][0-7]\d\,\d{1,14}$|^[1][0-7]\d$)|(^180\,[0]{1,14}$|^180$)");

        #region Property
        private ObservableCollection<PointOfIssue> _pointOfIssueList = new ObservableCollection<PointOfIssue>(ConnectionDataBase.db.PointOfIssue.Local);
        public ObservableCollection<PointOfIssue> PointOfIssueList { get => _pointOfIssueList; set => Set(ref _pointOfIssueList, value); }

        private PointOfIssue _pointOfIssue = null;
        public PointOfIssue PointOfIssue { get => _pointOfIssue; set => Set(ref _pointOfIssue, value); }
        #endregion

        #region Commands
        public ICommand AddPointOfIssue { get; }
        private bool CanAddPointOfIssueExecute(object parameter) => true;
        private void OnAddPointOfIssueExecute(object parameter)
        {
            PointOfIssue = new PointOfIssue();
            ConnectionDataBase.db.PointOfIssue.Local.Add(PointOfIssue);
        }
        
        public ICommand EditPointOfIssue { get; }
        private bool CanEditPointOfIssueExecute(object parameter) => true;
        private void OnEditPointOfIssueExecute(object parameter)
        {
            PointOfIssue = parameter as PointOfIssue;
        }
        
        public ICommand ConfirmPoint { get; }
        private bool CanConfirmPointExecute(object parameter) => true;
        private void OnConfirmPointExecute(object parameter)
        {
            if(string.IsNullOrWhiteSpace(PointOfIssue.Name) || LatRegex.IsMatch(PointOfIssue.lat.ToString()) == false || LonRegex.IsMatch(PointOfIssue.lot.ToString()) == false)
            {
                new MessageBox().Show();
                MessageBoxVM.SetMessage($"Не соответствует формату");
                return;
            }

            ConnectionDataBase.db.SaveChanges();

            PointOfIssueList = new ObservableCollection<PointOfIssue>(ConnectionDataBase.db.PointOfIssue);
            PointOfIssuePage.Instance.AddingMarkerOnMap();
            PointOfIssuePage.Instance.ListViewPointOfIssie.Items.Refresh();

            PointOfIssue = null;
        }

        public ICommand ShowPosition { get; }
        private bool CanShowPositionExecute(object parameter) => true;
        private void OnShowPositionExecute(object parameter)
        {
            var itemPointOfIssue = parameter as PointOfIssue;
            PointOfIssuePage.Instance.ListViewPointOfIssie.SelectedItem = itemPointOfIssue;
            PointOfIssuePage.Instance.gMapControl1.Position = new GMap.NET.PointLatLng((double)itemPointOfIssue.lat, (double)itemPointOfIssue.lot);
        }
        #endregion

        public PointOfIssueVM()
        {
            Instance = this;

            ShowPosition = new LambdaCommand(OnShowPositionExecute, CanShowPositionExecute);
            AddPointOfIssue = new LambdaCommand(OnAddPointOfIssueExecute, CanAddPointOfIssueExecute);
            EditPointOfIssue = new LambdaCommand(OnEditPointOfIssueExecute, CanEditPointOfIssueExecute);
            ConfirmPoint = new LambdaCommand(OnConfirmPointExecute, CanConfirmPointExecute);
        }
    }
}
