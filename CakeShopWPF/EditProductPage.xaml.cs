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
    /// Interaction logic for EditProductPage.xaml
    /// </summary>
    public partial class EditProductPage : Page
    {
        private CakeModel Product { get; set; }
        public EditProductPage(CakeModel cakeModel)
        {
            InitializeComponent();
            Product = cakeModel;
            this.DataContext = Product;
        }

        private void EditProductPage_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void confirmUpdateProductBtn_Click(object sender, RoutedEventArgs e)
        {
            Product.CakeCode = txtCakeCode.Text;
            Product.CakeName = txtCakeName.Text;
            Product.CakeImage = txtCakeImage.Text;
            Product.CakeDesc = txtCakeDesc.Text;
            Product.CakeCat = txtCakeCat.Text;
            int producPrice = Product.CakePrice;
            int.TryParse(txtCakePrice.Text, out producPrice);
            Product.CakePrice = producPrice;
            int productQuantity = Product.CakeQuantity;
            int.TryParse(txtCakePrice.Text, out productQuantity);
            Product.CakeQuantity = productQuantity;

            DatabaseAccess.UpdateCake(Product);
        }

        private void goBackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
