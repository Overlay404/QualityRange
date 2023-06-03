using System.Linq;
using System.Windows;

namespace DataBase.Model
{
    partial class Product
    {
        public byte[] SourceFirstImage => ConnectionDataBase.db.PhotoProduct.Local.FirstOrDefault(p => p.ID_Product == ID)?.Photo ?? ConnectionDataBase.ImageNullebleProduct;

        public decimal CostWithDiscount => (decimal)(Cost - (Cost * Discount / 100));

        public int CountProductInBasket => ProductList.Where(pl => pl.Basket.ID_Client == ConnectionDataBase.client?.ID).FirstOrDefault()?.Count ?? 0;

        public Visibility IsAddedInProductList => CountProductInBasket == 0 ? Visibility.Collapsed : Visibility.Visible;

        public Visibility IsNotAddedInProductList => CountProductInBasket == 0 ? Visibility.Visible : Visibility.Collapsed;

        public bool IsSelected { get; set; }
    }
}
