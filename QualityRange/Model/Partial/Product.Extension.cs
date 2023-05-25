using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace QualityRange.Model
{
    partial class Product
    {
        public ImageSource SourceFirstImage => SupportiveClasses.ImageConverter.ConvertToImageSource(App.db.PhotoProduct.Local.FirstOrDefault(p => p.ID_Product == ID)?.Photo);

        public decimal CostWithDiscount => (decimal)(Cost - (Cost * Discount / 100));

        public int CountProductInBasket => ProductList.Where(pl => pl.Basket.ID_Client == App.user?.ID).FirstOrDefault()?.Count ?? 0;

        public Visibility IsAddedInProductList => CountProductInBasket == 0 ? Visibility.Collapsed : Visibility.Visible;

        public Visibility IsNotAddedInProductList => CountProductInBasket == 0 ? Visibility.Visible : Visibility.Collapsed;
    }
}
