using QualityRangeForClient.View.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для AutorizationPage.xaml
    /// </summary>
    public partial class AutorizationPage : Page
    {
        public static AutorizationPage Instance { get; private set; }
        public AutorizationPage()
        {
            InitializeComponent();
            Instance = this;

            RegText.MouseDown += (senser, e) => AuthRegWindow.Instance.RegAuthFrame.Navigate(new RegistrationPage());

            LoginTextBlock.MouseDown += (sender, e) => LoginTB.Focus();
            PasswordTextBlock.MouseDown += (sender, e) => PasswordTB.Focus();

            LoginTB.GotFocus += (sender, e) => CollapsedElement(LoginTextBlock);
            LoginTB.LostFocus += (sender, e) => VisibleElement(LoginTextBlock, LoginTB);

            PasswordTB.GotFocus += (sender, e) => CollapsedElement(PasswordTextBlock);
            PasswordTB.LostFocus += (sender, e) => VisibleElement(PasswordTextBlock, PasswordTB);
        }

        #region Methods
        private void VisibleElement(TextBlock textBlock, TextBox textBox)
        {
            if (String.IsNullOrEmpty(textBox.Text))
                textBlock.Visibility = Visibility.Visible;
        }

        private void CollapsedElement(TextBlock textBlock)
        {
            textBlock.Visibility = Visibility.Collapsed;
        }
        #endregion
    }
}
