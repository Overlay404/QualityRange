using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QualityRange.Model
{
    partial class Client
    {
        public string FormatingNumberCard => string.Format("{0} {1}", 
                                             "**** **** ****",    
                                             NumberOfCreditCard.Substring(12));

        public string Fullname => $"{Surname} {Name} {Patronymic}";

    }
}
