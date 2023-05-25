using QualityRange.View.Windows;
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

namespace QualityRange.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public static RegistrationPage Instance { get; private set; }
        public RegistrationPage()
        {
            InitializeComponent();

            Instance = this;

            SignText.MouseDown += (sender, e) => AuthRegWindow.Instance.RegAuthFrame.Navigate(new AutorizationPage());

            NameTextBlock.MouseDown += (sender, e) => NameTB.Focus();
            LoginTextBlock.MouseDown += (sender, e) => LoginTB.Focus();
            PasswordTextBlock.MouseDown += (sender, e) => PasswordTB.Focus();

            NameTB.GotFocus += (sender, e) => CollapsedElement(NameTextBlock);
            NameTB.LostFocus += (sender, e) => VisibleElement(NameTextBlock, NameTB);

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
