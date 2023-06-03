using DataBase.Model;
using QualityRangeForClient.Commands.Base;
using QualityRangeForClient.View.Pages;
using QualityRangeForClient.View.Windows;
using QualityRangeForClient.ViewModel.PagesVM;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace QualityRangeForClient.ViewModel
{
    public class GridAndBarsViewProductPanelVM : ViewModel.Base.ViewModel
    {
        public static GridAndBarsViewProductPanelVM Instance { get; set; }

        ProductList ProdListItem;

        #region Property
        private ObservableCollection<Product> _products = new ObservableCollection<Product>(DataBase.ConnectionDataBase.db.Product.Local);
        public ObservableCollection<Product> Products { get => _products; set => Set(ref _products, value); }
        #endregion

        #region Commands
        public ICommand AddBasketProduct { get; }
        private bool CanAddBasketProductExecute(object parameter)
        {
            if (DataBase.ConnectionDataBase.client == null)
            {
                new AuthRegWindow().Show();
                return false;
            }
            return true;
        }
        private void OnAddBasketProductExecute(object parameter)
        {
            var basket = DataBase.ConnectionDataBase.db.Basket.Local.FirstOrDefault(b => b.Client == DataBase.ConnectionDataBase.client);
            if (basket == null)
            {
                basket = new Basket { Client = DataBase.ConnectionDataBase.db.Client.Local.FirstOrDefault(c => c == DataBase.ConnectionDataBase.client) };
                DataBase.ConnectionDataBase.db.Basket.Local.Add(basket);
            }

            var productList = DataBase.ConnectionDataBase.db.ProductList.Local.FirstOrDefault(p => p.ID_Product == (int)parameter);

            DataBase.ConnectionDataBase.db.ProductList.Local.Add(new ProductList
            {
                Basket = basket,
                ID_Product = (int)parameter,
                Count = 1
            });

            MainWindowVM.Instance.InitCountProductInBasket();
            InitProductList();
        }

        public ICommand RemoveProductInListProduct { get; }
        private bool CanRemoveProductInListProductExecute(object parameter)
        {
            ProdListItem = DataBase.ConnectionDataBase.db.ProductList.Local.FirstOrDefault(prodlist => prodlist.Basket.ID_Client == DataBase.ConnectionDataBase.client.ID && prodlist.Product.ID == (int)parameter);

            if (ProdListItem == null)
            {
                return false;
            }
            return true;
        }
        private void OnRemoveProductInListProductExecute(object parameter)
        {
            if (ProdListItem.Count == 1)
            {
                DataBase.ConnectionDataBase.db.ProductList.Local.Remove(ProdListItem);
            }
            else
            {
                ProdListItem.Count--;
            }

            MainWindowVM.Instance.InitCountProductInBasket();
            InitProductList();
        }

        public ICommand AddProductInListProduct { get; }
        private bool CanAddProductInListProductExecute(object parameter) => true;
        private void OnAddProductInListProductExecute(object parameter)
        {
            var prodListItem = DataBase.ConnectionDataBase.db.ProductList.Local.FirstOrDefault(prodlist => prodlist.Basket.ID_Client == DataBase.ConnectionDataBase.client.ID && prodlist.Product.ID == (int)parameter);
            prodListItem.Count++;

            MainWindowVM.Instance.InitCountProductInBasket();
            InitProductList();
        }


        public ICommand AdoutProductWidowShow { get; }
        private bool CanAdoutProductWidowShowExecute(object parameter) => true;
        private void OnAdoutProductWidowShowExecute(object parameter)
        {
            var aboutProduct = new AboutProduct();
            MainWindow.Instance.GlobalFrame.Navigate(aboutProduct);
            AboutProductVM.Instance.Product = DataBase.ConnectionDataBase.db.Product.Local.FirstOrDefault(p => p.ID == (int)parameter);
            AboutProductVM.Instance.Images = AboutProductVM.Instance.Product.PhotoProduct.Count() == 0 ? AboutProductVM.Instance.Product.PhotoProduct.Append(new PhotoProduct { Photo = DataBase.ConnectionDataBase.ImageNullebleProduct }) : AboutProductVM.Instance.Product.PhotoProduct;
            AboutProductVM.Instance.SelectedImage = AboutProductVM.Instance.Images.FirstOrDefault()?.Photo;
        }
        #endregion

        public GridAndBarsViewProductPanelVM()
        {
            if (Instance == null)
                Instance = this;

            InitProductList();

            AddBasketProduct = new LambdaCommand(OnAddBasketProductExecute, CanAddBasketProductExecute);
            RemoveProductInListProduct = new LambdaCommand(OnRemoveProductInListProductExecute, CanRemoveProductInListProductExecute);
            AddProductInListProduct = new LambdaCommand(OnAddProductInListProductExecute, CanAddProductInListProductExecute);
            AdoutProductWidowShow = new LambdaCommand(OnAdoutProductWidowShowExecute, CanAdoutProductWidowShowExecute);
        }

        public void InitProductList()
        {
            var classBars = BarsViewProductPage.Instance;
            var classGrid = GridViewProductPanel.Instance;
            if (classBars == null || classGrid == null)
            {
                return;
            }
            classBars.ListProductBarsView.Items.Refresh();
            classGrid.ListProductGridView.Items.Refresh();
        }
    }
}
