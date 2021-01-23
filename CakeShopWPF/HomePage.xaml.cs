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
        public ObservableCollection<CakeModel> DisplayCakeList { get; set; }
        public ObservableCollection<CategoryModel> CategoryList { get; set; }
        private int totalPage;
        public int TotalPage
        {
            get { return totalPage; }
            set
            {
                if (this.totalPage != value)
                {
                    this.totalPage = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TotalPage"));
                }
            }
        }
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
            CategoryList.Insert(0, new CategoryModel { CateId = -1, CateName = "Tất cả" });
            cbbFilter.ItemsSource = CategoryList;

            string rowPerPageStr = "5";
            int rowPerPageInt = 5;
            Setting.readSettingDB("row", ref rowPerPageStr);
            int.TryParse(rowPerPageStr, out rowPerPageInt);
            RowPerPage = rowPerPageInt;

            CurrentCakeList = CakeList;

            CurrentPage = 1;
            TotalPage = CurrentCakeList.Count / RowPerPage + (CurrentCakeList.Count % RowPerPage == 0 ? 0 : 1);

            if (TotalPage == 0)
            {
                CurrentPage = 0;
            }

            DisplayCakeList = new ObservableCollection<CakeModel>(CurrentCakeList.Skip(RowPerPage * (CurrentPage - 1)).Take(RowPerPage).ToList());
            CakeListView.ItemsSource = DisplayCakeList;
            this.DataContext = this;
            cbbFilter.SelectedIndex = 0;
        }

        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
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
            if(CurrentPage <= 1)
            {
                return;
            } else
            {
                --CurrentPage;
                TotalPage = CurrentCakeList.Count / RowPerPage + (CurrentCakeList.Count % RowPerPage == 0 ? 0 : 1);
                DisplayCakeList = new ObservableCollection<CakeModel>(CurrentCakeList.Skip(RowPerPage * (CurrentPage - 1)).Take(RowPerPage).ToList());
                CakeListView.ItemsSource = DisplayCakeList;
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
                TotalPage = CurrentCakeList.Count / RowPerPage + (CurrentCakeList.Count % RowPerPage == 0 ? 0 : 1);
                DisplayCakeList = new ObservableCollection<CakeModel>(CurrentCakeList.Skip(RowPerPage * (CurrentPage - 1)).Take(RowPerPage).ToList());
                CakeListView.ItemsSource = DisplayCakeList;
            }
        }

        private void cbbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            int categoryId = 0;

            if(this.IsLoaded)
            {
                string searchValue = SearchTextBox.Text.Trim();
                searchValue = DatabaseAccess.ConvertToUnSign(searchValue);

                if (cbbFilter.SelectedIndex == 0)
                {
                    var searchResult = from cake in CakeList
                                       where cake.CakeName2.Contains(searchValue)
                                       select cake;
                    CurrentCakeList = new ObservableCollection<CakeModel>(searchResult.ToList());
                    CurrentPage = 1;
                    TotalPage = CurrentCakeList.Count / RowPerPage + (CurrentCakeList.Count % RowPerPage == 0 ? 0 : 1);

                    if (TotalPage == 0)
                    {
                        CurrentPage = 0;
                    }

                    DisplayCakeList = new ObservableCollection<CakeModel>(CurrentCakeList.Skip(RowPerPage * (CurrentPage - 1)).Take(RowPerPage).ToList());
                    CakeListView.ItemsSource = DisplayCakeList;
                }

                if (cbbFilter.SelectedIndex > 0)
                {
                    CategoryModel category = cbbFilter.SelectedItem as CategoryModel;
                    categoryId = category.CateId;

                    var filterResult = from cake in CakeList
                                       where cake.CakeCat == categoryId
                                       select cake;

                    CurrentCakeList = new ObservableCollection<CakeModel>(filterResult.ToList());
                    var searchResult = from cake in CurrentCakeList
                                       where cake.CakeName2.Contains(searchValue)
                                       select cake;
                    CurrentCakeList = new ObservableCollection<CakeModel>(searchResult.ToList());
                    CurrentPage = 1;
                    TotalPage = CurrentCakeList.Count / RowPerPage + (CurrentCakeList.Count % RowPerPage == 0 ? 0 : 1);

                    if (TotalPage == 0)
                    {
                        CurrentPage = 0;
                    }

                    DisplayCakeList = new ObservableCollection<CakeModel>(CurrentCakeList.Skip(RowPerPage * (CurrentPage - 1)).Take(RowPerPage).ToList());
                    CakeListView.ItemsSource = DisplayCakeList;
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int categoryId = 0;
            string searchValue = (sender as TextBox).Text.Trim();
            searchValue = DatabaseAccess.ConvertToUnSign(searchValue);

            if (cbbFilter.SelectedIndex == 0)
            {
                var searchResult = from cake in CakeList
                                   where cake.CakeName2.Contains(searchValue)
                                   select cake;
                CurrentCakeList = new ObservableCollection<CakeModel>(searchResult.ToList());
                CurrentPage = 1;
                TotalPage = CurrentCakeList.Count / RowPerPage + (CurrentCakeList.Count % RowPerPage == 0 ? 0 : 1);

                if (TotalPage == 0)
                {
                    CurrentPage = 0;
                }

                DisplayCakeList = new ObservableCollection<CakeModel>(CurrentCakeList.Skip(RowPerPage * (CurrentPage - 1)).Take(RowPerPage).ToList());
                CakeListView.ItemsSource = DisplayCakeList;
            }

            if (cbbFilter.SelectedIndex > 0)
            {
                CategoryModel category = cbbFilter.SelectedItem as CategoryModel;
                categoryId = category.CateId;

                var filterResult = from cake in CakeList
                                   where cake.CakeCat == categoryId
                                   select cake;

                CurrentCakeList = new ObservableCollection<CakeModel>(filterResult.ToList());
                var searchResult = from cake in CurrentCakeList
                                   where cake.CakeName2.Contains(searchValue)
                                   select cake;
                CurrentCakeList = new ObservableCollection<CakeModel>(searchResult.ToList());
                CurrentPage = 1;
                TotalPage = CurrentCakeList.Count / RowPerPage + (CurrentCakeList.Count % RowPerPage == 0 ? 0 : 1);

                if (TotalPage == 0)
                {
                    CurrentPage = 0;
                }

                DisplayCakeList = new ObservableCollection<CakeModel>(CurrentCakeList.Skip(RowPerPage * (CurrentPage - 1)).Take(RowPerPage).ToList());
                CakeListView.ItemsSource = DisplayCakeList;
            }
            else
            {
                return;
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            cbbFilter.SelectedIndex = 0;
            SearchTextBox.Text = "";
        }
    }
}
