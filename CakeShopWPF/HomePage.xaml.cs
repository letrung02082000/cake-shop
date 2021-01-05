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
        }

        private void HomePage_Loaded(object sender, RoutedEventArgs e)
        {
            CakeList = new ObservableCollection<CakeModel>(DatabaseAccess.LoadCake());
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
            int cakeId = CakeList[selectedIndex].CakeId;
            DetailProductPage detailProductPage = new DetailProductPage(cakeId);
            this.NavigationService.Navigate(detailProductPage);
        }
    }
}
