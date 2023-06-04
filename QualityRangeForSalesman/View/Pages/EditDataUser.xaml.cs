using QualityRangeForSalesman.SupportiveClasses;
using QualityRangeForSalesman.View.Windows;
using QualityRangeForSalesman.ViewModel.PagesVM;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace QualityRangeForSalesman.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditDataUser.xaml
    /// </summary>
    public partial class EditDataUser : Page
    {
        public static EditDataUser Instance { get; set; }

        public static bool IsConfirm { get; set; }

        public EditDataUser(bool isConfirm = false)
        {
            IsConfirm = isConfirm;
            InitializeComponent();
            Instance = this;

            NameCompany.KeyDown += (sender, e) =>
            {
                HandledDigit(e);
            };

            Description.KeyDown += (sender, e) =>
            {
                HandledDigit(e);
            };
        }

        #region Methods
        private static void HandledDigit(KeyEventArgs e)
        {
            if (new KeyConverter().ConvertToString(e.Key).All(letter => char.IsDigit(letter)))
                e.Handled = true;
        }

        public static void UpdateDataContext()
        {
            var InsVM = new EditDataUserVM();
            Instance.DataContext = InsVM;
            Instance.GridRoot.DataContext = InsVM.EditingSalesman;
            Instance.ProfilePhotoImage.Source = ImageConverter.ConvertToImageSource(InsVM.EditingSalesman.ProfilePhoto);
            MainWindow.Instance.ProfilePhotoImage.Source = ImageConverter.ConvertToImageSource(InsVM.EditingSalesman.ProfilePhoto);
        } 
        #endregion
    }
}
