using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Model
{
    partial class Salesman
    {

        public string StatusFormat => User.Removed == true ? "Заблокирован" : "Не заблокирован" ;

    }
}
