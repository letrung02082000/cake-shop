using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib
{
    public class CakeModel:INotifyPropertyChanged
    {
        public int CakeId { get; set; }
        public string CakeCode { get; set; }
        public string CakeName { get; set; }
        public string CakeName2 { get; set; }
        public int CakeCat { get; set; }
        public int CakePrice { get; set; }
        public string CakeDesc { get; set; }
        public string CakeImage { get; set; }
        public int CakeQuantity { get; set; }
        public string CateName { get; set; }

        private int _cartQuantity;
        public int CartQuantity
        {
            get => _cartQuantity;
            set
            {
                _cartQuantity = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("CartQuantity"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
