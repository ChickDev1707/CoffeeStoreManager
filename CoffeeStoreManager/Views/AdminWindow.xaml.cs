
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CoffeeStoreManager.Views.ManageEmployee;
using CoffeeStoreManager.Views.ManageFood;
using CoffeeStoreManager.Views.MangeSource;
using CoffeeStoreManager.Views.Statistic;

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
        }
        private void FoodNavBtn_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = Pages.FoodPage;
        }

        private void EmployeeNavBtn_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = Pages.EmployeePage;

        }
        private void SourceNavBtn_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = Pages.SourcePage;

        }
        private void StatisticNavBtn_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = Pages.StatisticPage;

        }
    }
    static class Pages
    {
        public static List<Page> pages = new List<Page>();
        public static Page FoodPage { get => pages[0]; }
        public static Page EmployeePage { get => pages[1]; }
        public static Page SourcePage { get => pages[2]; }
        public static Page StatisticPage { get => pages[3]; }
        static Pages()
        {
            pages.Add(new ManageFoodMain());
            pages.Add(new ManageEmployeeMain());
            pages.Add(new ManageSourceMain());
            pages.Add(new StatisticMain());
        }

    }
}
