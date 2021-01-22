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
    /// Interaction logic for SettingScreen.xaml
    /// </summary>
    public partial class SettingScreen : UserControl
    {
        public SettingScreen()
        {
            InitializeComponent();
        }

        private void SettingScreen_Loaded(object sender, RoutedEventArgs e)
        {
            string rowPerPageStr = "5";
            Setting.readSettingDB("row", ref rowPerPageStr);
            tbRowPerPage.Text = rowPerPageStr;

            string startupStr = "1";
            Setting.readSettingDB("startup", ref startupStr);
            bool openStartup = startupStr == "1" ? true : false;

            if (openStartup)
            {
                cbSplashScreen.IsChecked = true;
            }
            else
            {
                cbSplashScreen.IsChecked = false;
            }
        }

        private void confirmRowBtn_Click(object sender, RoutedEventArgs e)
        {
            if(tbRowPerPage.Text.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng nhập vào 1 số!");
                return;
            }

            int rowPerPageInt = 5;
            if (!int.TryParse(tbRowPerPage.Text, out rowPerPageInt))
            {
                MessageBox.Show("Vui lòng nhập vào 1 số!");
                return;
            }

            Setting.SaveSetting("row", $"{rowPerPageInt}");
            MessageBox.Show("Đã lưu thành công!");
        }

        private void cbSplashScreen_Checked(object sender, RoutedEventArgs e)
        {
            Setting.SaveSetting("startup", "1");
        }

        private void cbSplashScreen_Unchecked(object sender, RoutedEventArgs e)
        {
            Setting.SaveSetting("startup", "0");
        }
    }
}
