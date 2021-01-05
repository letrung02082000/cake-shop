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
    /// Interaction logic for OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        public ObservableCollection<CustomerModel> CustomerList { get; set; }
        public OrderPage()
        {
            InitializeComponent();
        }

        private void OrderPage_Loaded(object sender, RoutedEventArgs e)
        {
            CartListView.ItemsSource = Cart.CartList;
            CustomerList = new ObservableCollection<CustomerModel>(DatabaseAccess.LoadCustomer());
            CustomerComboBox.ItemsSource = CustomerList;
            CustomerListView.ItemsSource = CustomerList;

            if (Cart.HasData)
            {
                if (Cart.OldCustomer)
                {
                    CustomerModel customer = CustomerList.Single(c => c.CustomerId == Cart.Customer.CustomerId);
                    CustomerComboBox.SelectedIndex = CustomerList.IndexOf(customer);
                }
            }
        }

        private void CheckoutBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new CheckoutPage());
        }

        private void CustomerListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CustomerModel customer = CustomerListView.SelectedItem as CustomerModel;
            int index = CustomerList.IndexOf(customer);
            CustomerComboBox.SelectedIndex = index;
        }

        private void CustomerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Cart.Customer = new CustomerModel();
            Cart.Customer = CustomerComboBox.SelectedItem as CustomerModel;
            Cart.HasData = true;
            Cart.OldCustomer = true;
        }
    }
}
