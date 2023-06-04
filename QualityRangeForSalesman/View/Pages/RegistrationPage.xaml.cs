using QualityRangeForSalesman.View.Windows;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QualityRangeForSalesman.View.Pages
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

            NameTB.KeyDown += (sender, e) => HandledDigit(e);
            
            NameTB.GotFocus += (sender, e) => CollapsedElement(NameTextBlock);
            NameTB.LostFocus += (sender, e) => VisibleElement(NameTextBlock, NameTB);

            LoginTB.GotFocus += (sender, e) => CollapsedElement(LoginTextBlock);
            LoginTB.LostFocus += (sender, e) => VisibleElement(LoginTextBlock, LoginTB);

            PasswordTB.GotFocus += (sender, e) => CollapsedElement(PasswordTextBlock);
            PasswordTB.LostFocus += (sender, e) => VisibleElement(PasswordTextBlock, PasswordTB);
        }
        #region Methods
        private static void HandledDigit(KeyEventArgs e)
        {
            if (new KeyConverter().ConvertToString(e.Key).All(letter => char.IsDigit(letter)))
                e.Handled = true;
        }

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
