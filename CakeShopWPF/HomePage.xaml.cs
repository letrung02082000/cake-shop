using ModelLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public partial class HomePage : Page, INotifyPropertyChanged
    {
        public ObservableCollection<CakeModel> CakeList { get; set; }
        public ObservableCollection<CakeModel> CurrentCakeList { get; set; }
        public ObservableCollection<CategoryModel> CategoryList { get; set; }
        public int TotalPage { get; set; }
        private int currentPage = 1;
        public int CurrentPage
        {
            get { return currentPage; }
            set
            {
                if (this.currentPage != value)
                {
                    this.currentPage = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentPage"));
                }
            }
        }
        public int RowPerPage { get; set; }
        public HomePage()
        {
            InitializeComponent();
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void HomePage_Loaded(object sender, RoutedEventArgs e)
        {
            CakeList = new ObservableCollection<CakeModel>(DatabaseAccess.LoadCake());
            CategoryList = new ObservableCollection<CategoryModel>(DatabaseAccess.LoadAllCategories());

            cbbFilter.ItemsSource = CategoryList;

            string rowPerPageStr = "5";
            int rowPerPageInt = 5;
            Setting.readSettingDB("row", ref rowPerPageStr);
            int.TryParse(rowPerPageStr, out rowPerPageInt);
            RowPerPage = rowPerPageInt;

            TotalPage = CakeList.Count / RowPerPage + (CakeList.Count % RowPerPage == 0 ? 0 : 1);
            CurrentCakeList = new ObservableCollection<CakeModel>(CakeList.Skip(RowPerPage * (CurrentPage-1)).Take(RowPerPage).ToList());
            CakeListView.ItemsSource = CurrentCakeList;
            this.DataContext = this;
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
                int cakeId = CurrentCakeList[selectedIndex].CakeId;
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

            CurrentCakeList[index].CartQuantity = 1;

            Cart.CartList.Add(CurrentCakeList[index]);
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            if(CurrentPage == 1)
            {
                return;
            } else
            {
                --CurrentPage;
                CurrentCakeList = new ObservableCollection<CakeModel>(CakeList.Skip(RowPerPage * (CurrentPage-1)).Take(RowPerPage).ToList());
                CakeListView.ItemsSource = CurrentCakeList;
            }
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage == TotalPage)
            {
                return;
            }
            else
            {
                ++CurrentPage;
                CurrentCakeList = new ObservableCollection<CakeModel>(CakeList.Skip(RowPerPage * (CurrentPage-1)).Take(RowPerPage).ToList());
                CakeListView.ItemsSource = CurrentCakeList;
            }
        }

        private void cbbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            int categoryId = 0;

            if (cbbFilter.SelectedIndex >=0 )
            {
                CategoryModel category = cbbFilter.SelectedItem as CategoryModel;
                categoryId = category.CateId;
            }
            else
            {
                return;
            }

            CurrentCakeList = new ObservableCollection<CakeModel>(DatabaseAccess.FindCakeByCategory(categoryId));
            CakeListView.ItemsSource = CurrentCakeList;
            
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchValue = (sender as TextBox).Text;
            MessageBox.Show(searchValue);
        }
    }
}
