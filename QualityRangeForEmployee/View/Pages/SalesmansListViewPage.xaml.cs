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
    /// Логика взаимодействия для SalesmansListViewPage.xaml
    /// </summary>
    public partial class SalesmansListViewPage : Page
    {
        public static SalesmansListViewPage Instance { get; private set; }
        public SalesmansListViewPage()
        {
            InitializeComponent();

            Instance = this;
        }
    }
}
