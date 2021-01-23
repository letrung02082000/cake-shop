using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib
{
    public class OrderItem
    {
        public int OrderId { get; set; }
        public int CakeId { get; set; }
        public string CakeName { get; set; }
        public string CakeCode { get; set; }
        public int CartQuantity { get; set; }
        public string CateName { get; set; }
    }
}
