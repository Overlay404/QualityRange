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

namespace QualityRangeForSalesman.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrderSalesman.xaml
    /// </summary>
    public partial class OrderSalesman : Page
    {
        public static OrderSalesman Instance { get; private set; }
        public OrderSalesman()
        {
            InitializeComponent();
            Instance = this;
        }
    }
}
