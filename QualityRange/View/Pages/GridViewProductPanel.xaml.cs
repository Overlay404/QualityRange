using QualityRangeForClient.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace QualityRangeForClient.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для GridViewProductPanel.xaml
    /// </summary>
    public partial class GridViewProductPanel : Page
    {
        public static GridViewProductPanel Instance { get; private set; }
        public GridViewProductPanel()
        {
            InitializeComponent();
            Instance = this;

            ListProductGridView.SelectionChanged += (sender, e) => ListProductGridView.SelectedIndex = -1;
        }
    }
}
