using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Model
{
    partial class Employee
    {

        public string Fullname => $"{Surname} {Name} {Patronymic}";

    }
}
