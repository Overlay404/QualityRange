using QualityRange.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Management.Instrumentation;
using System.Threading.Tasks;
using System.Windows;

namespace QualityRange
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static MarketplaceDatabaseEntities db = new MarketplaceDatabaseEntities();
    }
}
