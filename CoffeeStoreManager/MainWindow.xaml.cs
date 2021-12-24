using System;
using System.Windows;
using CoffeeStoreManager.Views;
using System.Windows;
using CoffeeStoreManager.ViewModels;
using CoffeeStoreManager.Views.Discount_Bill;


namespace CoffeeStoreManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainViewModel mainViewModel = new MainViewModel();
            this.DataContext = mainViewModel;
        }

        private void OpenAdminWindow(object sender, RoutedEventArgs e)
        {
            Window adminWindow = new AdminWindow();
            adminWindow.Show();
        }

        /*private void OpenDiscountWindow(object sender, RoutedEventArgs e)
        {
            MainViewModel mainVM = new MainViewModel();
            Window discountWindow = new DiscountWindow(mainVM);
            discountWindow.Show();
        }*/
    }
}
