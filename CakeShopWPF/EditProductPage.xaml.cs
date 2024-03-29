﻿using Microsoft.Win32;
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
    /// Interaction logic for EditProductPage.xaml
    /// </summary>
    public partial class EditProductPage : Page
    {
        public CakeModel Product { get; set; }
        public string FilePath { get; set; } = "";
        public string NewFilePath { get; set; } = "";
        public ObservableCollection<CategoryModel> CategoryList { get; set; }
        public EditProductPage(CakeModel cakeModel)
        {
            InitializeComponent();
            Product = cakeModel;
            this.DataContext = Product;
        }

        private void EditProductPage_Loaded(object sender, RoutedEventArgs e)
        {
            CategoryList = new ObservableCollection<CategoryModel>(DatabaseAccess.LoadAllCategories());
            cbbCakeCat.ItemsSource = CategoryList;
            CategoryModel category = CategoryList.Where(cate => cate.CateId == Product.CakeCat).First() as CategoryModel;
            int index = CategoryList.IndexOf(category);
            cbbCakeCat.SelectedIndex = index;
        }

        private void confirmUpdateProductBtn_Click(object sender, RoutedEventArgs e)
        {
            if(rbNewCategory.IsChecked == true)
            {
                int cateId = DatabaseAccess.SaveCategory(tbNewCategory.Text);
                Product.CakeCat = cateId;
            }
            else
            {
                Product.CakeCat = CategoryList[cbbCakeCat.SelectedIndex].CateId;
            }

            Product.CakeCode = txtCakeCode.Text;
            Product.CakeName = txtCakeName.Text;
            Product.CakeDesc = txtCakeDesc.Text;

            if(NewFilePath != "")
            {
                Product.CakeImage = NewFilePath.Split('\\').Last();
                File.Copy(FilePath, NewFilePath);
            }
            
            int producPrice = Product.CakePrice;
            int.TryParse(txtCakePrice.Text, out producPrice);
            Product.CakePrice = producPrice;
            int productQuantity = Product.CakeQuantity;
            int.TryParse(txtCakeQuantity.Text, out productQuantity);
            Product.CakeQuantity = productQuantity;

            DatabaseAccess.UpdateCake(Product);
            MessageBox.Show("Cập nhật thông tin bánh thành công!");
        }

        private void goBackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void cbbCakeCat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            rbNewCategory.IsChecked = false;
        }

        private void tbNewCategory_TextChanged(object sender, TextChangedEventArgs e)
        {
            rbNewCategory.IsChecked = true;
        }

        private void addImageBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            NewFilePath = "";

            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                NewFilePath = $"{baseDirectory}Image\\{Guid.NewGuid()}.{FilePath.Split('.')[1]}";

                if (!Directory.Exists($"{baseDirectory}Image"))
                {
                    Directory.CreateDirectory($"{baseDirectory}Image");
                }

                productImage.Source = new BitmapImage(new Uri(FilePath));
            }
        }
    }
}
