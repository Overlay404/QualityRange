﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace QualityRangeForEmployee
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        DataBase.ConnectionDataBase InstanceDataBase = new DataBase.ConnectionDataBase();
    }
}
