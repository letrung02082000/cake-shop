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
    /// Interaction logic for AllOrderPage.xaml
    /// </summary>
    public partial class AllOrderPage : Page
    {
        public ObservableCollection<OrderModel> OrderList { get; set; }
        public AllOrderPage(ObservableCollection<OrderModel> orderList)
        {
            InitializeComponent();
            OrderList = orderList;
            orderListView.ItemsSource = OrderList;
        }

        private void orderListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (orderListView.SelectedIndex >= 0)
            {
                OrderModel order = orderListView.SelectedItem as OrderModel;
                this.NavigationService.Navigate(new DetailOrderPage(order));
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            orderListView.SelectedIndex = -1;
        }
    }
}
