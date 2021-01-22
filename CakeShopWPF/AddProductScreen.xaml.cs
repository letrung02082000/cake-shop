using Microsoft.Win32;
using ModelLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
        public ObservableCollection<CategoryModel> CategoryList { get; set; }
        public AddProductScreen()
        {
            InitializeComponent();
        }

        private void AddProductScreen_Loaded(object sender, RoutedEventArgs e)
        {
            Product = new CakeModel();

            CategoryList = new ObservableCollection<CategoryModel>(DatabaseAccess.LoadAllCategories());
            cbbCategory.ItemsSource = CategoryList;
        }

        private void addProductBtn_Click(object sender, RoutedEventArgs e)
        {
            if (tbCode.Text.Trim().Length == 0 || tbName.Text.Trim().Length == 0 || tbDesc.Text.Trim().Length == 0 || tbPrice.Text.Trim().Length == 0 || tbQuantity.Text.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            Product.CakeCode = tbCode.Text;
            Product.CakeName = tbName.Text;
            Product.CakeDesc = tbDesc.Text;
            if(rbNewCategory.IsChecked == true)
            {
                if(tbNewCategory.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Vui lòng nhập thông tin loại sản phẩm!");
                    return;
                }

                int cateId;

                try
                {
                    cateId = DatabaseAccess.SaveCategory(tbNewCategory.Text);
                    Product.CakeCat = cateId;
                }
                catch(Exception error)
                {
                    if (error != null)
                    {
                        MessageBox.Show("Có lỗi xảy ra khi thêm loại sản phẩm. Hãy chắc chắn loại sản phẩm chưa tồn tại.");
                        return;
                    }
                }
                
            }
            else
            {
                if(cbbCategory.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn loại sản phẩm!");
                    return;
                }
                Product.CakeCat = CategoryList[cbbCategory.SelectedIndex].CateId;
            }

            int cakePrice = 0;
            if(!int.TryParse(tbPrice.Text, out cakePrice))
            {
                MessageBox.Show("Vui lòng nhập giá là 1 số!");
                return;
            }

            Product.CakePrice = cakePrice;

            int cakeQuantity = 0;

            if (!int.TryParse(tbQuantity.Text, out cakeQuantity))
            {
                MessageBox.Show("Vui lòng nhập số lượng là 1 số!");
                return;
            }

            Product.CakeQuantity = cakeQuantity;

            if(Product.CakeImage == null)
            {
                MessageBox.Show("Vui lòng thêm hình ảnh cho sản phẩm.");
                return;
            }
            else
            {
                //Do nothing
            }

            Product.CakeId = DatabaseAccess.SaveCake(Product);
            MessageBox.Show("Đã thêm sản phẩm thành công");
        }

        private void tbNewCategory_TextChanged(object sender, TextChangedEventArgs e)
        {
            rbNewCategory.IsChecked = true;
        }

        private void cbbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            rbNewCategory.IsChecked = false;
        }

        private void addImageBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string newFilePath = "";

            if(openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                newFilePath = $"{baseDirectory}Image\\{Guid.NewGuid()}.{filePath.Split('.')[1]}";

                if (!Directory.Exists($"{baseDirectory}Image"))
                {
                    Directory.CreateDirectory($"{baseDirectory}Image");
                }

                File.Copy(filePath, newFilePath);
                Product.CakeImage = newFilePath;
                productImage.Source = new BitmapImage(new Uri(newFilePath));
            }
        }
    }
}
