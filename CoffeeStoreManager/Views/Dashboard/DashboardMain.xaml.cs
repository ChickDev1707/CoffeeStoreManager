
using System.Windows;
using System.Windows.Controls;
using CoffeeStoreManager.ViewModels;

namespace CoffeeStoreManager.Views.Dashboard
{
    /// <summary>
    /// Interaction logic for DashboardMain.xaml
    /// </summary>
    public partial class DashboardMain : Page
    {
        public DashboardMain()
        {
            InitializeComponent();
            DashboardViewModel dbVm = new DashboardViewModel();
            this.DataContext = dbVm;
            Style = (Style)FindResource("WindowStyle");
        }
    }
}
