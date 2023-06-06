using DataBase.Model;
using QualityRangeForEmployee.Commands.Base;
using QualityRangeForEmployee.View.Pages;
using QualityRangeForEmployee.ViewModel.WindowsVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Media;

namespace QualityRangeForEmployee.ViewModel.PagesVM
{
    internal class StatisticPageVM : ViewModel.Base.ViewModel
    {
        public static StatisticPageVM Instance { get; private set; }

        #region Property
        private SeriesCollection _chartSeries;
        public SeriesCollection ChartSeries { get => _chartSeries; set => Set(ref _chartSeries, value); }

        private List<string> _itemSelected = new List<string>() { "Продавец", "Товар", "Пункт выдачи" };
        public List<string> ItemSelected { get => _itemSelected; set => Set(ref _itemSelected, value); }

        private List<string> _sortItemSelected;
        public List<string> SortItemSelected { get => _sortItemSelected; set => Set(ref _sortItemSelected, value); }
        #endregion


        #region Commands
        public ICommand ChartUpdate { get; }
        private bool CanChartUpdateExecute(object parameter) => true;
        private void OnChartUpdateExecute(object parameter)
        {
            var itemSelected = StatisticPage.Instance.ItemSelectedCB.SelectedIndex;
            var sortItemSelected = StatisticPage.Instance.SortItemSelectedCB.SelectedIndex;

            #region DataSelected
            if (itemSelected == 0 || sortItemSelected == 0)
            {
                // продавец + заказы
            }
            else if (itemSelected == 0 || sortItemSelected == 1)
            {
                // продавец + товары
            }
            else if (itemSelected == 1 || sortItemSelected == 0)
            {
                // товар + покупаемость
            }
            else if (itemSelected == 1 || sortItemSelected == 1)
            {
                // товар + количество
            }
            else if (itemSelected == 1 || sortItemSelected == 2)
            {
                // товар + стоимость
            }
            else if (itemSelected == 2)
            {
                // точка выдачи + заказы
            } 
            #endregion
        }
        #endregion


        public StatisticPageVM()
        {
            Instance = this;

            ChartSeries = new SeriesCollection
            {
                new RowSeries
                {
                    Title = "My Column Series",
                    Values = new ChartValues<decimal> { 5, 6, 10, 7 }
                }
            };

            ChartUpdate = new LambdaCommand(OnChartUpdateExecute, CanChartUpdateExecute);
        }
    }
}
