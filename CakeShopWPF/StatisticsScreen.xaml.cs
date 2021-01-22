using ModelLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
using LiveCharts;
using LiveCharts.Wpf;

namespace CakeShopWPF
{
    /// <summary>
    /// Interaction logic for StatisticsScreen.xaml
    /// </summary>
    public partial class StatisticsScreen : UserControl
    {
        public ObservableCollection<OrderModel> OrderList { get; set; }
        public ObservableCollection<TotalPerCategory> TotalPerCategoryList { get; set; }
        public string[] Labels { get; set; }
        public SeriesCollection SeriesCollection { get; set; } = new SeriesCollection();
        public StatisticsScreen()
        {
            InitializeComponent();
            OrderList = new ObservableCollection<OrderModel>(DatabaseAccess.LoadOrder());
            TotalPerCategoryList = new ObservableCollection<TotalPerCategory>(DatabaseAccess.LoadTotalByCategory());
        }

        private void StatisticsScreen_Loaded(object sender, RoutedEventArgs e)
        {
            CultureInfo culture = new CultureInfo("en-US");

            foreach (var order in OrderList)
            {
                DateTime tempDate = Convert.ToDateTime(order.OrderDate, culture);
                order.OrderDateTime = tempDate;
            }

            int month1 = 0;
            int month2 = 0;
            int month3 = 0;
            int month4 = 0;
            int month5 = 0;
            int month6 = 0;
            int month7 = 0;
            int month8 = 0;
            int month9 = 0;
            int month10 = 0;
            int month11 = 0;
            int month12 = 0;


            foreach (var order in OrderList)
            {
                if(order.OrderDateTime.Year == DateTime.Now.Year)
                {
                    if (order.OrderDateTime.Month == 1)
                    {
                        month1 += order.TotalPrice;
                    }
                    else if(order.OrderDateTime.Month == 2)
                    {
                        month2 += order.TotalPrice;
                    }
                    else if (order.OrderDateTime.Month == 3)
                    {
                        month3 += order.TotalPrice;
                    }
                    else if (order.OrderDateTime.Month == 4)
                    {
                        month4 += order.TotalPrice;
                    }
                    else if (order.OrderDateTime.Month == 5)
                    {
                        month5 += order.TotalPrice;
                    }
                    else if (order.OrderDateTime.Month == 6)
                    {
                        month6 += order.TotalPrice;
                    }
                    else if (order.OrderDateTime.Month == 7)
                    {
                        month7 += order.TotalPrice;
                    }
                    else if (order.OrderDateTime.Month == 8)
                    {
                        month8 += order.TotalPrice;
                    }
                    else if (order.OrderDateTime.Month == 9)
                    {
                        month9 += order.TotalPrice;
                    }
                    else if (order.OrderDateTime.Month == 10)
                    {
                        month10 += order.TotalPrice;
                    }
                    else if (order.OrderDateTime.Month == 11)
                    {
                        month11 += order.TotalPrice;
                    }
                    else if (order.OrderDateTime.Month == 12)
                    {
                        month12 += order.TotalPrice;
                    }

                }
            }

            ColumnSeries columnSeries = new ColumnSeries
            {
                Title = "Doanh thu theo tháng",
                Values = new ChartValues<int> { month1, month2, month3, month4, month5, month6, month7, month8, month9, month10, month11, month12 }
            };

             Labels = new[]
            {
                "Tháng 1",
                "Tháng 2",
                "Tháng 3",
                "Tháng 4",
                "Tháng 5",
                "Tháng 6",
                "Tháng 7",
                "Tháng 8",
                "Tháng 9",
                "Tháng 10",
                "Tháng 11",
                "Tháng 12",
            };


            PerMonthChart.Series.Add(columnSeries);
            chartGrid.DataContext = Labels;

            foreach(var total in TotalPerCategoryList)
            {
                PieSeries pieSeries = new PieSeries
                {
                    Title = total.CateName,
                    Values = new ChartValues<int> { total.TotalCate }
                };

                pieChart.Series.Add(pieSeries);
            }

            

        }
        
    }
}
