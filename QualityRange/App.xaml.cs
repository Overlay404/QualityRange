using QualityRange.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Management.Instrumentation;
using System.Threading.Tasks;
using System.Windows;

namespace QualityRange
{
    public partial class App : Application
    {
        public static MarketplaceDatabaseEntities db = new MarketplaceDatabaseEntities();

        public static User user;

        public App()
        {
            db.Address.Load();
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
