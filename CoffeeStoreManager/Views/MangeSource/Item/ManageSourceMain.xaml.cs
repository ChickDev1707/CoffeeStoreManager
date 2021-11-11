
using System.Windows;
using System.Windows.Controls;
using CoffeeStoreManager.Models;
using CoffeeStoreManager.Views.MangeSource.Detail;

namespace CoffeeStoreManager.Views.MangeSource.Item
{
    /// <summary>
    /// Interaction logic for ManageSourceMain.xaml
    /// </summary>
    public partial class ManageSourceMain : Page
    {
        public ManageSourceMain()
        {
            InitializeComponent();
        }
        private void Detail_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWindow = findAddMinWindow();
            if(adminWindow!= null && SourceList.SelectedItem!=null)
            {
                ViewSource sourceCard = (ViewSource)SourceList.SelectedItem;
                adminWindow.Main.Content = new SourceDetailPage(this, sourceCard.ma_phieu_nhap_hang);
            }
        }
        private AdminWindow findAddMinWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(AdminWindow))
                {
                    return (window as AdminWindow);
                }
            }
            return null;
        }
    }
}
