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
    /// Interaction logic for AddProductScreen.xaml
    /// </summary>
    public partial class AddProductScreen : UserControl
    {
        public CakeModel Product { get; set; }
        public AddProductScreen()
        {
            InitializeComponent();
        }

        private void AddProductScreen_Loaded(object sender, RoutedEventArgs e)
        {
            Product = new CakeModel();
        }

        private void addProductBtn_Click(object sender, RoutedEventArgs e)
        {
            Product.CakeCode = tbCode.Text;
            Product.CakeName = tbName.Text;
            Product.CakeDesc = tbDesc.Text;
            Product.CakeCat = tbCategory.Text;

            int cakePrice = 0;
            int.TryParse(tbPrice.Text, out cakePrice);

            Product.CakePrice = cakePrice;

            int cakeQuantity = 0;
            int.TryParse(tbQuantity.Text, out cakeQuantity);

            Product.CakeQuantity = cakeQuantity;

            Product.CakeId = DatabaseAccess.SaveCake(Product);
            MessageBox.Show(Product.CakeId.ToString());
        }
    }
}
