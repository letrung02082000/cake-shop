using ModelLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CakeShopWPF
{
    /// <summary>
    /// Interaction logic for CheckoutPage.xaml
    /// </summary>
    public partial class CheckoutPage : Page
    {
        public int DirectTotalPrice { get; set; }
        public int DeliveryTotalPrice { get; set; }
        public CheckoutPage()
        {
            InitializeComponent();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Checkout_Loaded(object sender, RoutedEventArgs e)
        {
            

            int totalPrice = 0;

            foreach(var cake in Cart.CartList)
            {
                totalPrice += cake.CakePrice*cake.CartQuantity;
            }

            DirectTotalPrice = totalPrice;
            DeliveryTotalPrice = totalPrice + 30000;

            tbDirectTotalPrice.Text = totalPrice.ToString();
            tbShippingFee.Text = "30000";
            tbDeliveryTotalPrice.Text = $"{DeliveryTotalPrice}";

            DirectBtn.IsChecked = true;
        }

        private void tbCash_TextChanged(object sender, TextChangedEventArgs e)
        {
            DirectBtn.IsChecked = true;
            DeliveryBtn.IsChecked = false;

            int cash = 0;
            int totalPrice = 0;
            int.TryParse(tbDirectTotalPrice.Text, out totalPrice);
            int.TryParse(tbCash.Text, out cash);
            tbReturnCash.Text = (cash - totalPrice).ToString();
        }

        private void DeliveryBtn_Checked(object sender, RoutedEventArgs e)
        {
            DirectBtn.IsChecked = false;
        }

        private void DirectBtn_Checked(object sender, RoutedEventArgs e)
        {
            DeliveryBtn.IsChecked = false;
        }

        private void tbDirectTotalPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            DirectBtn.IsChecked = true;
            DeliveryBtn.IsChecked = false;
        }

        private void tbReturnCash_TextChanged(object sender, TextChangedEventArgs e)
        {
            DirectBtn.IsChecked = true;
            DeliveryBtn.IsChecked = false;
        }

        private void tbShippingFee_TextChanged(object sender, TextChangedEventArgs e)
        {
            DirectBtn.IsChecked = false;
            DeliveryBtn.IsChecked = true;

            int shippingFee = 30000;
            int.TryParse(tbShippingFee.Text, out shippingFee);
            tbDeliveryTotalPrice.Text = $"{DirectTotalPrice + shippingFee}";
        }

        private void tbDeliveryTotalPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            DirectBtn.IsChecked = false;
            DeliveryBtn.IsChecked = true;
        }

        private void ConfirmCheckoutBtn_Click(object sender, RoutedEventArgs e)
        {
            OrderModel order = new OrderModel();

            int totalPrice = 0;

            if (DirectBtn.IsChecked == true)
            {
                if (!int.TryParse(tbDirectTotalPrice.Text, out totalPrice))
                {
                    MessageBox.Show("Vui lòng nhập tổng giá là 1 số!");
                    return;
                }

                order.OrderStatus = 1;
                order.ShippingFee = 0;
                order.TotalPrice = totalPrice;
                order.IsDirect = 1;
            }
            else if (DeliveryBtn.IsChecked == true)
            {
                
                int shippingFee = 30000;
                
                if(!int.TryParse(tbShippingFee.Text, out shippingFee))
                {
                    MessageBox.Show("Vui lòng nhập phí giao hàng là 1 số!");
                    return;
                }

                if(tbShippingAddress.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Vui lòng nhập địa chỉ giao hàng!");
                    return;
                }

                if(!int.TryParse(tbDirectTotalPrice.Text, out totalPrice))
                {
                    MessageBox.Show("Vui lòng nhập tổng thu là 1 số!");
                    return;
                }

                order.ShippingAddress = tbShippingAddress.Text;
                order.ShippingFee = shippingFee;
                order.OrderStatus = 0;
                order.TotalPrice = totalPrice+shippingFee;
                order.IsDirect = 0;
            }

            order.OrderDate = DateTime.Now.ToString();

            if (Cart.IsOldCustomer)
            {
                int customerId = Cart.OldCustomer.CustomerId;
                
                order.CustomerId = customerId;
            }
            else
            {
                int customerId = DatabaseAccess.SaveCustomer(Cart.NewCustomer);
                Cart.NewCustomer.CustomerId = customerId;

                order.CustomerId = customerId;
            }

            foreach (var cake in Cart.CartList)
            {
                CakeModel cakeModel = DatabaseAccess.FindCakeById(cake.CakeId);
                int cakeQuantity = cakeModel.CakeQuantity;
                cakeModel.CakeQuantity = cakeModel.CakeQuantity - cake.CartQuantity;

                if (cakeModel.CakeQuantity < 0)
                {
                    cakeModel.CakeQuantity = cakeQuantity;
                    MessageBox.Show("Không đủ số lượng bánh trong kho");
                    return;
                }

                DatabaseAccess.UpdateCake(cakeModel);
            }

            int orderId = DatabaseAccess.SaveOrder(order);

            foreach (var cake in Cart.CartList)
            {
                DatabaseAccess.SaveOrderItem(orderId, cake.CakeId, cake.CartQuantity);
            }

            Cart.CartList.Clear();
            Cart.IsOldCustomer = true;
            Cart.NewCustomer = null;
            Cart.OldCustomer = null;
            Cart.HasData = false;
            DirectBtn.IsChecked = true;
            tbDirectTotalPrice.Text = "";
            tbDirectTotalPrice.Text = "";
            tbShippingAddress.Text = "";

            MessageBox.Show("Tạo đơn hàng thành công");
        }

        private void tbShippingAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            DirectBtn.IsChecked = false;
            DeliveryBtn.IsChecked = true;
        }
    }
}
