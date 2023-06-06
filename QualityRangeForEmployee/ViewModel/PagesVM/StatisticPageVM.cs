using DataBase;
using LiveCharts;
using LiveCharts.Wpf;
using QualityRangeForEmployee.Commands.Base;
using QualityRangeForEmployee.View.Pages;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using QualityRangeForEmployee.View.Windows;
using QualityRangeForEmployee.ViewModel.WindowsVM;
using MessageBox = QualityRangeForEmployee.View.Windows.MessageBox;
using System.Collections.ObjectModel;

namespace QualityRangeForEmployee.ViewModel.PagesVM
{
    internal class StatisticPageVM : ViewModel.Base.ViewModel
    {
        public static StatisticPageVM Instance { get; private set; }

        #region Property
        private SeriesCollection _chartSeries = new SeriesCollection();
        public SeriesCollection ChartSeries { get => _chartSeries; set => Set(ref _chartSeries, value); }

        private List<string> _itemSelected = new List<string>() { "Продавец", "Товар", "Пункт выдачи" };
        public List<string> ItemSelected { get => _itemSelected; set => Set(ref _itemSelected, value); }

        private List<string> _typeChartSelectedCB = new List<string>() { "Строчная", "Столбчатая", "Линейная" };
        public List<string> TypeChartSelectedCB { get => _typeChartSelectedCB; set => Set(ref _typeChartSelectedCB, value); }

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
            var typeSelected = StatisticPage.Instance.TypeChartSelectedCB.SelectedIndex;

            #region DataSelected
            if (itemSelected == 0 && sortItemSelected == 0)
            {
                ChartSeries.Clear();

                // продавец + заказы
                var values = new ChartValues<int>();
                foreach (var item in ConnectionDataBase.db.Salesman)
                {
                    switch (typeSelected)
                    {
                        case 0:
                            ChartSeries.Add(new RowSeries
                            {
                                Title = item.NameCompany,
                                Values = new ChartValues<int> { ConnectionDataBase.db.ProductListOrder.Local.Where(o => o.Product.Salesman == item).GroupBy(pr => pr.Order).Count() }
                            });
                            break;

                        case 1:
                            ChartSeries.Add(new ColumnSeries
                            {
                                Title = item.NameCompany,
                                Values = new ChartValues<int> { ConnectionDataBase.db.ProductListOrder.Local.Where(o => o.Product.Salesman == item).GroupBy(pr => pr.Order).Count() }
                            });
                            break;

                        case 2:
                            values.Add(ConnectionDataBase.db.ProductListOrder.Local.Where(o => o.Product.Salesman == item).GroupBy(pr => pr.Order).Count());
                            break;
                    }
                }
                if (values.Count == 0) return;

                ChartSeries.Add(new LineSeries
                {
                    Title = "Продавцы",
                    Values = values
                });
            }
            else if (itemSelected == 0 && sortItemSelected == 1)
            {
                ChartSeries.Clear();

                // продавец + товары
                var values = new ChartValues<int>();
                foreach (var item in ConnectionDataBase.db.Salesman)
                {
                    switch (typeSelected)
                    {
                        case 0:
                            ChartSeries.Add(new RowSeries
                            {
                                Title = item.NameCompany,
                                Values = new ChartValues<int> { ConnectionDataBase.db.ProductListOrder.Local.Where(o => o.Product.Salesman == item).Sum(plo => plo.Count) ?? 0 }
                            });
                            break;

                        case 1:
                            ChartSeries.Add(new ColumnSeries
                            {
                                Title = item.NameCompany,
                                Values = new ChartValues<int> { ConnectionDataBase.db.ProductListOrder.Local.Where(o => o.Product.Salesman == item).Sum(plo => plo.Count) ?? 0 }
                            });
                            break;

                        case 2:
                            values.Add(ConnectionDataBase.db.ProductListOrder.Local.Where(o => o.Product.Salesman == item).Sum(plo => plo.Count) ?? 0);
                            break;
                    }
                }
                if (values.Count == 0) return;

                ChartSeries.Add(new LineSeries
                {
                    Title = "Продавцы",
                    Values = values
                });
            }
            else if (itemSelected == 1 && sortItemSelected == 0)
            {
                ChartSeries.Clear();

                // товар + покупаемость
                var values = new ChartValues<int>();
                foreach (var item in ConnectionDataBase.db.Product)
                {
                    switch (typeSelected)
                    {
                        case 0:
                            ChartSeries.Add(new RowSeries
                            {
                                Title = item.Name,
                                Values = new ChartValues<int> { ConnectionDataBase.db.ProductListOrder.Local.Where(o => o.Product == item).Sum(plo => plo.Count) ?? 0 }
                            });
                            break;

                        case 1:
                            ChartSeries.Add(new ColumnSeries
                            {
                                Title = item.Name,
                                Values = new ChartValues<int> { ConnectionDataBase.db.ProductListOrder.Local.Where(o => o.Product == item).Sum(plo => plo.Count) ?? 0 }
                            });
                            break;

                        case 2:
                            values.Add(ConnectionDataBase.db.ProductListOrder.Local.Where(o => o.Product == item).Sum(plo => plo.Count) ?? 0);
                            break;
                    }
                }
                if (values.Count == 0) return;

                ChartSeries.Add(new LineSeries
                {
                    Title = "Продукты",
                    Values = values
                });
            }
            else if (itemSelected == 1 && sortItemSelected == 1)
            {
                ChartSeries.Clear();

                // товар + количество
                var values = new ChartValues<int>();
                foreach (var item in ConnectionDataBase.db.Product)
                {
                    switch (typeSelected)
                    {
                        case 0:
                            ChartSeries.Add(new RowSeries
                            {
                                Title= item.Name,
                                Values = new ChartValues<int> { item.Count ?? 0 }
                            });
                            break;

                        case 1:
                            ChartSeries.Add(new ColumnSeries
                            {
                                Title = item.Name,
                                Values = new ChartValues<int> { item.Count ?? 0 }
                            });
                            break;

                        case 2:
                            values.Add(item.Count ?? 0);
                            break;
                    }
                }
                if (values.Count == 0) return;

                ChartSeries.Add(new LineSeries
                {
                    Title = "Продукты",
                    Values = values
                });
            }
            else if (itemSelected == 1 && sortItemSelected == 2)
            {
                ChartSeries.Clear();
              
                // товар + стоимость
                var values = new ChartValues<decimal>();
                foreach (var item in ConnectionDataBase.db.Product)
                {
                    switch (typeSelected)
                    {
                        case 0:
                            ChartSeries.Add(new RowSeries
                            {
                                Title = item.Name,
                                Values = new ChartValues<decimal> { item.CostWithDiscount }
                            });
                            break;

                        case 1:
                            ChartSeries.Add(new ColumnSeries
                            {
                                Title = item.Name,
                                Values = new ChartValues<decimal> { item.CostWithDiscount }
                            });
                            break;

                        case 2:
                            values.Add(item.CostWithDiscount);
                            break;
                    }
                }

                if (values.Count == 0) return;

                ChartSeries.Add(new LineSeries
                {
                    Title = "Продукты",
                    Values = values
                });
            }
            else if (itemSelected == 2)
            {
                ChartSeries.Clear();

                // точка выдачи + заказы
                var values = new ChartValues<int>();
                foreach (var item in ConnectionDataBase.db.PointOfIssue)
                {
                    switch (typeSelected)
                    {
                        case 0:
                            ChartSeries.Add(new RowSeries
                            {
                                Title = item.Name,
                                Values = new ChartValues<int> { item.Order.Count() }
                            });
                            break;

                        case 1:
                            ChartSeries.Add(new ColumnSeries
                            {
                                Title = item.Name,
                                Values = new ChartValues<int> { item.Order.Count() }
                            });
                            break;

                        case 2:
                            values.Add(item.Order.Count());
                            break;
                    }
                }

                if (values.Count == 0) return;

                ChartSeries.Add(new LineSeries
                {
                    Title = "Точки выдачи",
                    Values = values
                });
            }
            #endregion
        }

        #endregion


        public StatisticPageVM()
        {
            Instance = this;

            foreach (var item in ConnectionDataBase.db.Salesman)
            {
                ChartSeries.Add(new RowSeries
                {
                    Title = item.NameCompany,
                    Values = new ChartValues<int> { ConnectionDataBase.db.ProductListOrder.Local.Where(o => o.Product.Salesman == item).GroupBy(pr => pr.Order).Count() }
                });
            }

            ChartUpdate = new LambdaCommand(OnChartUpdateExecute, CanChartUpdateExecute);
        }
    }
}
