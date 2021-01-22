using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CakeShopWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (!File.Exists("CakeShop.db"))
            {
                MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu. Vui lòng không chỉnh sửa các File hệ thống.");
                return;
            }

            string startupSetting = "1";
            bool openSplashWindow = true;

            Setting.readSettingDB("startup", ref startupSetting);
            openSplashWindow = startupSetting == "1" ? true : false;

            //int rowPerPageInt = 5;
            //string rowPerPage = "5";
            //Setting.readSettingDB("row", ref rowPerPage);
            
            //if(int.TryParse(rowPerPage, out rowPerPageInt))
            //{
            //    Setting.RowPerPage = rowPerPageInt;
            //}
            //else
            //{
            //    Setting.RowPerPage = 5;
            //}


            if (openSplashWindow)
            {
                SplashScreen splashScreen = new SplashScreen();
                splashScreen.Show();
            }
            else
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
        }
    }
}
