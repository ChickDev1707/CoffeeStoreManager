
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
                case "NavPartTimeScheduler":
                    Main.Content = Pages.PartTimeSchedulerPage;
                    break;
                case "NavSource":
                    Main.Content = Pages.SourcePage;
                    break;
                case "NavStatisticFoodType":
                    Main.Content = Pages.StatisticFoodTypePage;
                    break;
                case "NavRule":
                    Main.Content = Pages.RegulationPage;
                    break;
            }
        }

        private void AccountNavBtn_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = Pages.AccountPage;
        }
    }
    
}
