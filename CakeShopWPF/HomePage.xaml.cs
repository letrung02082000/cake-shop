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
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public ObservableCollection<CakeModel> CakeList { get; set; }
        public HomePage()
        {
            InitializeComponent();
            CakeList = new ObservableCollection<CakeModel>(DatabaseAccess.LoadCake());
        }

        private void HomePage_Loaded(object sender, RoutedEventArgs e)
        {
            
            CakeListView.ItemsSource = CakeList;
        }

        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void CakeListView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int selectedIndex = CakeListView.SelectedIndex;

            if(selectedIndex >= 0)
            {
                int cakeId = CakeList[selectedIndex].CakeId;
                DetailProductPage detailProductPage = new DetailProductPage(cakeId);
                this.NavigationService.Navigate(detailProductPage);
            }

        }

        private void addToCartBtn_Click(object sender, RoutedEventArgs e)
        {
            var index = CakeListView.Items.IndexOf((sender as FrameworkElement).DataContext);

            foreach (var cartCake in Cart.CartList)
            {
                if (cartCake.CakeId == CakeList[index].CakeId)
                {
                    MessageBox.Show("San pham da duoc them vao don hang");
                    return;
                }
            }

            CakeList[index].CartQuantity = 1;

            Cart.CartList.Add(CakeList[index]);
        }
    }
}
