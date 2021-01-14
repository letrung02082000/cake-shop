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
        public CustomerModel Customer { get; set; }
        public OrderPage()
        {
            InitializeComponent();
        }

        private void OrderPage_Loaded(object sender, RoutedEventArgs e)
        {
            if(Cart.CartList.Count == 0)
            {
                txtNotification.Visibility = Visibility.Visible;
                CartListView.Visibility = Visibility.Hidden;
            }

            CartListView.ItemsSource = Cart.CartList;
            CustomerList = new ObservableCollection<CustomerModel>(DatabaseAccess.LoadCustomer());
            CustomerComboBox.ItemsSource = CustomerList;
            CustomerListView.ItemsSource = CustomerList;

            OldCustomerBtn.IsChecked = true;

            if (Cart.OldCustomer != null)
            {
                CustomerModel customer = CustomerList.Single(c => c.CustomerId == Cart.OldCustomer.CustomerId);
                CustomerComboBox.SelectedIndex = CustomerList.IndexOf(customer);
            }

            if (Cart.NewCustomer != null)
            {
                tbCustomerName.Text = Cart.NewCustomer.CustomerName;
                tbCustomerTel.Text = Cart.NewCustomer.CustomerTel;
            }

            if (Cart.IsOldCustomer == false)
            {
                OldCustomerBtn.IsChecked = false;
                NewCustomerBtn.IsChecked = true;
            }
            else
            {
                OldCustomerBtn.IsChecked = true;
                NewCustomerBtn.IsChecked = false;
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
            OldCustomerBtn.IsChecked = true;
            NewCustomerBtn.IsChecked = false;
        }

        private void CustomerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Cart.OldCustomer = new CustomerModel();
            Cart.OldCustomer = CustomerComboBox.SelectedItem as CustomerModel;
            Cart.HasData = true;
            OldCustomerBtn.IsChecked = true;
            NewCustomerBtn.IsChecked = false;
        }

        private void NewCustomerBtn_Checked(object sender, RoutedEventArgs e)
        {
            OldCustomerBtn.IsChecked = false;
        }

        private void OldCustomerBtn_Checked(object sender, RoutedEventArgs e)
        {
            NewCustomerBtn.IsChecked = false;
        }

        private void tbCustomerName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(Cart.NewCustomer == null)
            {
                Cart.NewCustomer = new CustomerModel();
            }

            NewCustomerBtn.IsChecked = true;
            OldCustomerBtn.IsChecked = false;
            Cart.NewCustomer.CustomerName = tbCustomerName.Text;
        }

        private void tbCustomerTel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Cart.NewCustomer == null)
            {
                Cart.NewCustomer = new CustomerModel();
            }

            NewCustomerBtn.IsChecked = true;
            OldCustomerBtn.IsChecked = false;
            Cart.NewCustomer.CustomerTel = tbCustomerTel.Text;
        }

        private void OrderPage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (NewCustomerBtn.IsChecked == true)
            {
                Cart.IsOldCustomer = false;
            }
            else
            {
                Cart.IsOldCustomer = true;
            }
        }

        private void IncreaseQuantityBtn_Click(object sender, RoutedEventArgs e)
        {
            int index = CartListView.Items.IndexOf((sender as FrameworkElement).DataContext);
            Cart.CartList[index].CartQuantity += 1;
        }

        private void DecreaseQuantityBtn_Click(object sender, RoutedEventArgs e)
        {
           
            int index = CartListView.Items.IndexOf((sender as FrameworkElement).DataContext);

            if (Cart.CartList[index].CartQuantity == 1)
            {
                return;
            }

            Cart.CartList[index].CartQuantity -= 1;
        }

        private void tbQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            int index = CartListView.Items.IndexOf((sender as FrameworkElement).DataContext);
            int quantity = 1;
            int.TryParse((sender as TextBox).Text, out quantity);
            Cart.CartList[index].CartQuantity = quantity;
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            int index = CartListView.Items.IndexOf((sender as FrameworkElement).DataContext);
            Cart.CartList.RemoveAt(index);
        }

        private void addCartBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new HomePage());
        }
    }
}
