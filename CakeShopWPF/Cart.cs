using ModelLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeShopWPF
{
    public class Cart
    {
        public static ObservableCollection<CakeModel> CartList { get; set; }
        public static CustomerModel Customer { get; set; }
        public static bool OldCustomer { get; set; } = false;
        public static bool HasData { get; set; } = false;
    }
}
