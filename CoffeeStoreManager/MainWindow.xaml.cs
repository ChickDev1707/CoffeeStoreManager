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
using CoffeeStoreManager.Views.ManageFood;
using CoffeeStoreManager.Views.MangeSource;

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

        private void OpenManageFoodWindow(object sender, RoutedEventArgs e)
        {
            Window manageFoodWindow = new ManageFoodWindow();
            manageFoodWindow.Show();
        }
        private void OpenManageSourceWindow(object sender, RoutedEventArgs e)
        {
            Window mangeSourceWindow = new ManageSourceWindow();
            mangeSourceWindow.Show();
        }
    }
}
