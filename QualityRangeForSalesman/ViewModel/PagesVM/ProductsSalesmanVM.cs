using DataBase;
using DataBase.Model;
using QualityRangeForSalesman.Commands.Base;
using QualityRangeForSalesman.SupportiveClasses;
using QualityRangeForSalesman.View.Pages;
using QualityRangeForSalesman.View.Windows;
using QualityRangeForSalesman.ViewModel.WindowsVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QualityRangeForSalesman.ViewModel.PagesVM
{
    internal class ProductsSalesmanVM : ViewModel.Base.ViewModel
    {

        public static ProductsSalesmanVM Instance { get; private set; }

        Product newProduct;

        public static bool IsAddingNewProduct = false;

        readonly List<Product> DeletingProducts = new List<Product>();
        List<ProductList> DeletingProductListes = new List<ProductList>();
        List<ProductListOrder> DeletingProductListOrderes = new List<ProductListOrder>();
        List<PhotoProduct> DeletingPhotoProduct = new List<PhotoProduct>();

        #region Property
        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products { get => _products; set => Set(ref _products, value); }

        private int _countProducts = 0;
        public int CountProducts { get => _countProducts; set => Set(ref _countProducts, value); }

        private bool _isPressDeleteProducts = false;
        public bool IsPressDeleteProducts { get => _isPressDeleteProducts; set => Set(ref _isPressDeleteProducts, value); }

        private IEnumerable<Category> _categories = ConnectionDataBase.db.Category.Local;
        public IEnumerable<Category> Categories { get => _categories; set => Set(ref _categories, value); }
        #endregion

        #region Commands
        public ICommand AddNewProduct { get; }
        private bool CanAddNewProductExecute(object parameter)
        {
            if (IsAddingNewProduct == true)
            {
                return false;
            }
            return true;
        }
        private void OnAddNewProductExecute(object parameter)
        {
            if (ConnectionDataBase.salesman == null)
            {
                new AuthRegWindow().Show();
                return;
            }

            newProduct = new Product
            {
                Salesman = ConnectionDataBase.salesman,
                Discount = 0,
                Cost = 0,
                Count = 0,
                ID_Status = 2
            };

            Products.Add(newProduct);
            ConnectionDataBase.db.Product.Local.Add(newProduct);
            ProductsSalesman.Instance.ListProductGridView.SelectedItem = newProduct;

            IsAddingNewProduct = true;
        }

        public ICommand DeleteProducts { get; }
        private bool CanDeleteProductsExecute(object parameter)
        {
            if (IsAddingNewProduct == true)
            {
                new MessageBox().Show();
                MessageBoxVM.SetMessage("Вы в процессе добавления товара");
                return false;
            }
            return true;
        }
        private void OnDeleteProductsExecute(object parameter)
        {
            if (ConnectionDataBase.salesman == null)
            {
                new AuthRegWindow().Show();
                return;
            }

            if(IsPressDeleteProducts == true)
            {
                ClearChanges();
                IsPressDeleteProducts = false;
                return;
            }

            IsPressDeleteProducts = true;
            ProductsSalesman.Instance.ListProductGridView.SelectedItem = null;
        }
        
        public ICommand DeleteProduct { get; }
        private bool CanDeleteProductExecute(object parameter) => true;
        private void OnDeleteProductExecute(object parameter)
        {
            var searchProduct = ConnectionDataBase.db.Product.Local.FirstOrDefault(p => p.ID == (int)parameter);
            var searchProdList = ConnectionDataBase.db.ProductList.Local.Where(pl => pl.ID_Product == (int)parameter);
            var searchProdListOrder = ConnectionDataBase.db.ProductListOrder.Local.Where(pl => pl.ID_Product == (int)parameter);
            var searchPhotoProduct = ConnectionDataBase.db.PhotoProduct.Local.Where(pl => pl.ID_Product == (int)parameter);

            Products.Remove(searchProduct);

            DeletingProducts.Add(searchProduct);
            ProductsSalesman.Instance.ListProductGridView.Items.Refresh();

            if (searchProdList != null)
            {
                DeletingProductListes.AddRange(searchProdList);
            }

            if (searchProdListOrder != null)
            {
                DeletingProductListOrderes.AddRange(searchProdListOrder);
            }

            if (searchPhotoProduct != null)
            {
                DeletingPhotoProduct.AddRange(searchPhotoProduct);
            }
        }
        
        public ICommand EditProduct { get; }
        private bool CanEditProductExecute(object parameter)
        {
            if (IsPressDeleteProducts == true)
            {
                new MessageBox().Show();
                MessageBoxVM.SetMessage("Вы в процессе добавления товара");
                return false;
            }
            return true;
        }
        private void OnEditProductExecute(object parameter)
        {
            var selectedItemNow = parameter as Product;

            var selectedItemLater = ProductsSalesman.Instance.ListProductGridView.SelectedItem as Product;

            if(selectedItemLater == null)
            {
                ProductsSalesman.Instance.ListProductGridView.SelectedItem = selectedItemNow;
                return;
            }

            if(selectedItemLater.Category == null || string.IsNullOrWhiteSpace(selectedItemLater.Name) || string.IsNullOrWhiteSpace(selectedItemLater.Description)
                || string.IsNullOrWhiteSpace(selectedItemLater.Cost.ToString()) || string.IsNullOrWhiteSpace(selectedItemLater.Count.ToString())
                || string.IsNullOrWhiteSpace(selectedItemLater.Discount.ToString()))
            {
                new MessageBox().Show();
                MessageBoxVM.SetMessage("Не все обязательные поля заполнены");
                return;
            }

            if (selectedItemLater.Discount < 0 || selectedItemLater.Discount > 100)
            {
                new MessageBox().Show();
                MessageBoxVM.SetMessage("Поле скидка не может быть меньше 0 или больше 100");
                selectedItemLater.Discount = 0;
                ProductsSalesman.Instance.ListProductGridView.SelectedItem = selectedItemLater;
                return;
            }
            if (selectedItemLater.Cost < 0)
            {
                new MessageBox().Show();
                MessageBoxVM.SetMessage("Поле цена не может быть меньше 0");
                selectedItemLater.Cost = 0;
                ProductsSalesman.Instance.ListProductGridView.SelectedItem = selectedItemLater;
                return;
            }
            if (selectedItemLater.Count < 0)
            {
                new MessageBox().Show();
                MessageBoxVM.SetMessage("Поле количество не может быть меньше 0");
                selectedItemLater.Count = 0;
                ProductsSalesman.Instance.ListProductGridView.SelectedItem = selectedItemLater;
                return;
            }

            ProductsSalesman.Instance.ListProductGridView.SelectedItem = selectedItemNow;
            ProductsSalesman.Instance.ListProductGridView.Items.Refresh();

            IsAddingNewProduct = false;
        }
        
        public ICommand CancelDeletingItems { get; }
        private bool CanCancelDeletingItemsExecute(object parameter) => true;
        private void OnCancelDeletingItemsExecute(object parameter)
        {
            ClearChanges();

            IsPressDeleteProducts = false;
        }
        
        public ICommand ConfirmDeletingItems { get; }
        private bool CanConfirmDeletingItemsExecute(object parameter) => true;
        private void OnConfirmDeletingItemsExecute(object parameter)
        {
            ConnectionDataBase.db.ProductListOrder.RemoveRange(DeletingProductListOrderes);
            ConnectionDataBase.db.ProductList.RemoveRange(DeletingProductListes);
            ConnectionDataBase.db.Product.RemoveRange(DeletingProducts);
            ConnectionDataBase.db.PhotoProduct.RemoveRange(DeletingPhotoProduct);

            ConnectionDataBase.db.SaveChanges();

            ConnectionDataBase.db.Product.Load();
            ConnectionDataBase.db.ProductList.Load();
            ConnectionDataBase.db.ProductListOrder.Load();
            ConnectionDataBase.db.PhotoProduct.Load();

            IsPressDeleteProducts = false;
        }
        
        public ICommand AddPhoto { get; }
        private bool CanAddPhotoExecute(object parameter) => true;
        private void OnAddPhotoExecute(object parameter)
        {
            var image = ImageConverter.OpenFileDialogSave();

            if(image == null)
            {
                return;
            }

            ConnectionDataBase.db.PhotoProduct.Local.Add(new PhotoProduct
            {
                Product = parameter as Product,
                Photo = image
            });

            RefreshSelectedItem(parameter);
        }
        
        public ICommand DeletePhoto { get; }
        private bool CanDeletePhotoExecute(object parameter) => true;
        private void OnDeletePhotoExecute(object parameter)
        {
            ConnectionDataBase.db.PhotoProduct.Local.Remove(parameter as PhotoProduct);

            RefreshSelectedItem(parameter);
        }
        #endregion


        public ProductsSalesmanVM()
        {
            Instance = this;

            AddNewProduct = new LambdaCommand(OnAddNewProductExecute, CanAddNewProductExecute);
            DeleteProducts = new LambdaCommand(OnDeleteProductsExecute, CanDeleteProductsExecute);
            DeleteProduct = new LambdaCommand(OnDeleteProductExecute, CanDeleteProductExecute);
            EditProduct = new LambdaCommand(OnEditProductExecute, CanEditProductExecute);
            ConfirmDeletingItems = new LambdaCommand(OnConfirmDeletingItemsExecute, CanConfirmDeletingItemsExecute);
            CancelDeletingItems = new LambdaCommand(OnCancelDeletingItemsExecute, CanCancelDeletingItemsExecute);
            AddPhoto = new LambdaCommand(OnAddPhotoExecute, CanAddPhotoExecute);
            DeletePhoto = new LambdaCommand(OnDeletePhotoExecute, CanDeletePhotoExecute);
        }

        #region Methods
        private void ClearChanges()
        {
            DeletingProducts.Clear();
            DeletingProductListes.Clear();
            DeletingProductListOrderes.Clear();

            Products = new ObservableCollection<Product>(ConnectionDataBase.db.Product.Local.Where(p => p.Salesman == ConnectionDataBase.salesman));
        }

        private static void RefreshSelectedItem(object parameter)
        {
            ProductsSalesman.Instance.ListProductGridView.Items.Refresh();
            ProductsSalesman.Instance.PhotoList.Items.Refresh();
        } 
        #endregion
    }
}
