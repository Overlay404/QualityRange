using QualityRange.Model;
using QualityRange.SupportiveClasses;
using System.Data.Entity;
using System.Windows;

namespace QualityRange
{
    public partial class App : Application
    {
        public static MarketplaceDatabaseEntities db = new MarketplaceDatabaseEntities();

        public static User user;

        public static byte[] ImageNullebleProduct = ImageConverter.GetImageFromInternet(
            "https://w7.pngwing.com/pngs/685/826/png-transparent-computer-icons-cardboard-box-packaging-and-labeling-business-box-miscellaneous-angle-freight-transport-thumbnail.png");


        public static byte[] EmptyUserImage = ImageConverter.ConvertToByteCollection(@"Resourses\Image\EmptyUserPhoto.png");

        public App()
        {
            db.PointOfIssue.Load();
            db.Basket.Load();
            db.Category.Load();
            db.PhotoProduct.Load();
            db.Client.Load();
            db.Employee.Load();
            db.Order.Load();
            db.Product.Load();
            db.Salesman.Load();
            db.ProductList.Load();
        }
    }
}
