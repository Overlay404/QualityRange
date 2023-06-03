using QualityRangeForClient.View.Windows;
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

namespace QualityRangeForClient.View.Pages
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
            SurnameTextBlock.MouseDown += (sender, e) => SurnameTB.Focus();
            PatronymicTextBlock.MouseDown += (sender, e) => PatronymicTB.Focus();
            LoginTextBlock.MouseDown += (sender, e) => LoginTB.Focus();
            PasswordTextBlock.MouseDown += (sender, e) => PasswordTB.Focus();

            NameTB.KeyDown += (sender, e) => HandledDigit(e);
            SurnameTB.KeyDown += (sender, e) => HandledDigit(e);
            PatronymicTB.KeyDown += (sender, e) => HandledDigit(e);

            NameTB.GotFocus += (sender, e) => CollapsedElement(NameTextBlock);
            NameTB.LostFocus += (sender, e) => VisibleElement(NameTextBlock, NameTB);

            SurnameTB.GotFocus += (sender, e) => CollapsedElement(SurnameTextBlock);
            SurnameTB.LostFocus += (sender, e) => VisibleElement(SurnameTextBlock, SurnameTB);

            PatronymicTB.GotFocus += (sender, e) => CollapsedElement(PatronymicTextBlock);
            PatronymicTB.LostFocus += (sender, e) => VisibleElement(PatronymicTextBlock, PatronymicTB);

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
