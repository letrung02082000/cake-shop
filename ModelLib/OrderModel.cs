using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib
{
    public class OrderModel
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int OrderStatus { get;set; }
        public int ShippingFee { get; set; }
        public int TotalPrice { get; set; }
        public string OrderDate { get; set; }
        public string ShippingAddress { get; set; }
        public int IsDirect { get; set; }
        public string CustomerName { get; set; }
        public string CustomerTel { get; set; }
        public DateTime OrderDateTime { get; set; }
        public string OrderStatusStr { get; set; }
    }
}
