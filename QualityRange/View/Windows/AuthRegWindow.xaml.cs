using QualityRangeForClient.View.Pages;
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
using System.Windows.Shapes;

namespace QualityRangeForClient.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для AuthRegWindow.xaml
    /// </summary>
    public partial class AuthRegWindow : Window
    {
        public static AuthRegWindow Instance { get; private set; }

        public AuthRegWindow()
        {
            InitializeComponent();
            Instance = this;
            RegAuthFrame.Navigate(new AutorizationPage());
        }
    }
}
