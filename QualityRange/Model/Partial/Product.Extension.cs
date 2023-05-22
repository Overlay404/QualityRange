using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace QualityRange.Model
{
    partial class Product
    {
        public ImageSource SourceFirstImage => SupportiveClasses.ImageConverter.ConvertToImageSource(App.db.PhotoProduct.FirstOrDefault(p => p.ID_Product == ID)?.Photo);
    }
}
