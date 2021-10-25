using System;
using System.Windows;
using CoffeeStoreManager.Views;

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
        }

        private void OpenAdminWindow(object sender, RoutedEventArgs e)
        {
            Window adminWindow = new AdminWindow();
            adminWindow.Show();
        }
    }
}
