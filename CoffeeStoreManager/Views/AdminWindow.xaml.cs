﻿
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using Syncfusion.UI.Xaml.NavigationDrawer;

namespace CoffeeStoreManager.Views
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
       
        public AdminWindow()
        {
            InitializeComponent();
            Style = (Style)FindResource("WindowStyle");
        }
        
        private void navigationDrawer_ItemClicked(object sender, Syncfusion.UI.Xaml.NavigationDrawer.NavigationItemClickedEventArgs e)
        {
            switch (e.Item.Name)
            {
                case "NavFoodList":
                    Main.Content = Pages.FoodListPage;
                    break;
                case "NavFoodType":
                    Main.Content = Pages.FoodTypePage;
                    break;
                case "NavEmployeeList":
                    Main.Content = Pages.EmployeePage;
                    break;
                case "NavEmployeeType":
                    Main.Content = Pages.EmployeeTypePage;
                    break;
                case "NavPartTimeScheduler":
                    Main.Content = Pages.PartTimeSchedulerPage;
                    break;
                case "NavSource":
                    Main.Content = Pages.SourcePage;
                    break;
                case "NavStatisticRevenue":
                    Main.Content = Pages.StatisticRevenuePage;
                    break;
                case "NavStatisticFoodType":
                    Main.Content = Pages.StatisticFoodTypePage;
                    break;
                case "NavRule":
                    Main.Content = Pages.RegulationPage;
                    break;
            }
        }

        private void AccountInfo_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = Pages.AccountPage;
        }


        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Window mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
    
}
