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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Cart.CartList = new System.Collections.ObjectModel.ObservableCollection<ModelLib.CakeModel>();
        }

        private void ChangeMenuPointer(int index)
        {
            TransitionCursor.OnApplyTemplate();
            MenuPointer.Margin = new Thickness(0, 100 + 60 * index, 0, 0);
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            GridContent.Children.Clear();
            ChangeMenuPointer(0);
            ListViewMenu.SelectedIndex = 0;
            HomeScreen homeScreen = new HomeScreen();
            GridContent.Children.Add(homeScreen);
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            GridContent.Children.Clear();
            ChangeMenuPointer(1);
            ListViewMenu.SelectedIndex = 1;
            AddProductScreen addProductScreen = new AddProductScreen();
            GridContent.Children.Add(addProductScreen);
        }

        private void CreateOrder_Click(object sender, RoutedEventArgs e)
        {
            GridContent.Children.Clear();
            ChangeMenuPointer(2);
            ListViewMenu.SelectedIndex = 2;
            OrderScreen orderScreen = new OrderScreen();
            GridContent.Children.Add(orderScreen);
        }

        private void Statistics_Click(object sender, RoutedEventArgs e)
        {
            GridContent.Children.Clear();
            ChangeMenuPointer(3);
            ListViewMenu.SelectedIndex = 3;
            StatisticsScreen statisticsScreen = new StatisticsScreen();
            GridContent.Children.Add(statisticsScreen);
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            GridContent.Children.Clear();
            ChangeMenuPointer(4);
            ListViewMenu.SelectedIndex = 4;
            AboutScreen aboutScreen = new AboutScreen();
            GridContent.Children.Add(aboutScreen);
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            GridContent.Children.Clear();
            ChangeMenuPointer(5);
            ListViewMenu.SelectedIndex = 5;
            SettingScreen settingScreen = new SettingScreen();
            GridContent.Children.Add(settingScreen);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HomeScreen homeScreen = new HomeScreen();
            GridContent.Children.Add(homeScreen);
        }
    }
}
