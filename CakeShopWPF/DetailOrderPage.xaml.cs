using ModelLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for DetailOrderPage.xaml
    /// </summary>
    public partial class DetailOrderPage : Page
    {
        public OrderModel Order { get; set; }
        public ObservableCollection<OrderItem> OrderItemList { get; set; }
        public DetailOrderPage(OrderModel order)
        {
            InitializeComponent();
            Order = order;
            OrderItemList = new ObservableCollection<OrderItem>(DatabaseAccess.LoadOrderItem(order.OrderId));
        }   

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            OrderStackPanel.DataContext = Order;
            OrderItemListView.ItemsSource = OrderItemList;

            if (Order.IsDirect == 1)
            {
                dpShippingAddress.Visibility = Visibility.Hidden;
                dpShippingFee.Visibility = Visibility.Hidden;
            }

            if(Order.OrderStatus == 1)
            {
                txtStatus.Text = "Đã thanh toán";
                completeBtn.Visibility = Visibility.Hidden;
            }
            else
            {
                txtStatus.Text = "Chưa thanh toán";
            }
        }

        private void goBackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void completeBtn_Click(object sender, RoutedEventArgs e)
        {
            DatabaseAccess.CompleteOrder(Order.OrderId);
            completeBtn.Visibility = Visibility.Hidden;
            txtStatus.Text = "Đã thanh toán";
            MessageBox.Show("Đã đánh dấu hoàn tất đơn hàng.");
        }
    }
}
