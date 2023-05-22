using QualityRange.Model;
using QualityRange.View.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace QualityRange.ViewModel
{
    internal class GridAndBarsViewProductPanelVM : ViewModel.Base.ViewModel
    {
        #region Property
        private IEnumerable<Product> _products;
        public IEnumerable<Product> Products { get => _products; set => Set(ref _products, value); }
        #endregion

        public GridAndBarsViewProductPanelVM()
        {
            Products = App.db.Product.ToList();
        }
    }
}
