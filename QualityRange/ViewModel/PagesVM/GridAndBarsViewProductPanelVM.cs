using QualityRange.Model;
using QualityRange.View.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace QualityRange.ViewModel
{
    public class GridAndBarsViewProductPanelVM : ViewModel.Base.ViewModel
    {
        public static GridAndBarsViewProductPanelVM Instance { get; set; }

        #region Property
        private IEnumerable<Product> _products = App.db.Product.Local.ToList();
        public IEnumerable<Product> Products { get => _products; set => Set(ref _products, value); }
        #endregion

        public GridAndBarsViewProductPanelVM()
        {
            if(Instance == null)
                Instance = this;
        }
    }
}
