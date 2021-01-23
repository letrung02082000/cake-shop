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
using System.Windows.Shapes;

namespace CakeShopWPF
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        private Random rsg = new Random();
        public ObservableCollection<CakeModel> CakeList { get; set; }
        public SplashScreen()
        {
            InitializeComponent();
            CakeList = new ObservableCollection<CakeModel>(DatabaseAccess.LoadCake());
            int index = rsg.Next(CakeList.Count);
            this.DataContext = CakeList[index];
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Setting.SaveSetting("startup", "0");
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Setting.SaveSetting("startup", "1");
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
