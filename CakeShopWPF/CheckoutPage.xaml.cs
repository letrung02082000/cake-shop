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
            DirectBtn.IsChecked = true;

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

        private void tbShippingAdress_TextChanged(object sender, TextChangedEventArgs e)
        {
            DirectBtn.IsChecked = false;
            DeliveryBtn.IsChecked = true;
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

            if (Cart.IsOldCustomer)
            {
                int customerId = Cart.OldCustomer.CustomerId;
                
                order.CustomerId = customerId;            }
            else
            {
                int customerId = DatabaseAccess.SaveCustomer(Cart.NewCustomer);
                Cart.NewCustomer.CustomerId = customerId;

                order.CustomerId = customerId;
            }

            int totalPrice = 0;

            if (DirectBtn.IsChecked == true)
            {
                order.OrderStatus = 1;
                order.ShippingFee = 0;
                int.TryParse(tbDirectTotalPrice.Text, out totalPrice);
                order.TotalPrice = DirectTotalPrice;
            }
            else if (DeliveryBtn.IsChecked == true)
            {
                order.OrderStatus = 0;
                int shippingFee = 30000;
                int.TryParse(tbShippingFee.Text, out shippingFee);
                order.ShippingFee = shippingFee;
                order.ShippingAddress = tbShippingAdress.Text;
                order.TotalPrice = DeliveryTotalPrice;
            }

            int orderId = DatabaseAccess.SaveOrder(order);

            foreach(var cake in Cart.CartList)
            {
                DatabaseAccess.SaveOrderItem(orderId, cake.CakeId);
            }
        }
    }
}
