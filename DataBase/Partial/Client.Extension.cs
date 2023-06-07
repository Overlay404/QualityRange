using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Model
{
    partial class Client
    {
        public string FormatingNumberCard => string.Format("{0} {1}", "**** **** ****", NumberOfCreditCard?.Substring(12));

        public string NumberCard => FormatingNumberCard.Equals("**** **** **** ") ? "Нет карты" : FormatingNumberCard;

        public string Fullname => $"{Surname} {Name} {Patronymic}";

        public string StatusFormat => User.Removed == true ? "Заблокирован" : "Не заблокирован";
    }
}
