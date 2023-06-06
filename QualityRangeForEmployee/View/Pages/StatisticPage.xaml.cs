using QualityRangeForEmployee.View.Windows;
using QualityRangeForEmployee.ViewModel.PagesVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QualityRangeForEmployee.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для StatisticPage.xaml
    /// </summary>
    public partial class StatisticPage : Page
    {
        public static StatisticPage Instance { get; private set; }

        Dictionary<int, string> sortValue = new Dictionary<int, string>()
        {
            {0, "По заказам" },
            {1, "По проданным товарам" },
            {2, "По покупаемости" },
            {3, "По количеству" },
            {4, "По стоимости" }
        };

        public StatisticPage()
        {
            InitializeComponent();

            Instance = this;

            InitCB();

            ItemSelectedCB.SelectionChanged += (sender, e) =>
            {
                switch (ItemSelectedCB.SelectedIndex)
                {
                    case 0:
                        StatisticPageVM.Instance.SortItemSelected = new List<string>() { sortValue.Values.ElementAt(0), sortValue.Values.ElementAt(1) };
                        SortItemSelectedCB.SelectedIndex = 0;
                        break;
                    case 1:
                        StatisticPageVM.Instance.SortItemSelected = new List<string>() { sortValue.Values.ElementAt(2), sortValue.Values.ElementAt(3), sortValue.Values.ElementAt(4) };
                        SortItemSelectedCB.SelectedIndex = 0;
                        break;
                    case 2:
                        StatisticPageVM.Instance.SortItemSelected = new List<string>() { sortValue.Values.ElementAt(0) };
                        SortItemSelectedCB.SelectedIndex = 0;
                        break;
                }
            };
        }

        private void InitCB()
        {
            ItemSelectedCB.SelectedIndex = 0;
            StatisticPageVM.Instance.SortItemSelected = new List<string>() { sortValue.Values.ElementAt(0), sortValue.Values.ElementAt(1) };
            SortItemSelectedCB.SelectedIndex = 0;
        }
    }
}
