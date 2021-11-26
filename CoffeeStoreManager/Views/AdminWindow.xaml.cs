
using System.Windows;


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
        private void AccountNavBtn_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = Pages.AccountPage;
        }
        private void PartTimeSchedulerNavBtn_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = Pages.PartTimeSchedulerPage;

        }

        private void RegulationNavBtn_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = Pages.RegulationPage;
        private void MonthReportNavBtn_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = Pages.MonthReportPage;

        }
    }
    
}
