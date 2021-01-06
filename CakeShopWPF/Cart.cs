using ModelLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeShopWPF
{
    public class Cart:INotifyPropertyChanged
    {
        public static ObservableCollection<CakeModel> CartList { get; set; }
        public static CustomerModel NewCustomer { get; set; }
        public static CustomerModel OldCustomer { get; set; }
        public static bool IsOldCustomer { get; set; } = true;
        public static bool HasData { get; set; } = false;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
