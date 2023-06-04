using DataBase.Model;
using DataBase.SupportiveClasses;
using System.Data.Entity;

namespace DataBase
{
    public class ConnectionDataBase
    {
        public static MarketShopEntities db = new MarketShopEntities();

        public static Client client;

        public static Salesman salesman;

        public static byte[] ImageNullebleProduct = ImageConverter.GetImageFromInternet(
            "https://w7.pngwing.com/pngs/685/826/png-transparent-computer-icons-cardboard-box-packaging-and-labeling-business-box-miscellaneous-angle-freight-transport-thumbnail.png");


        public static byte[] EmptyUserImage = ImageConverter.ConvertToByteCollection(@"Resourses\Image\EmptyUserPhoto.png");

        public ConnectionDataBase()
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
            db.ProductListOrder.Load();
        }
    }
}
