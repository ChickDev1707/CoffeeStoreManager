
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using CoffeeStoreManager.ViewModels;
using CoffeeStoreManager.Views.MangeSource.Item;

namespace CoffeeStoreManager.Views.MangeSource.Detail
{
    /// <summary>
    /// Interaction logic for SourceDetailPage.xaml
    /// </summary>
    public partial class SourceDetailPage : Page
    {
        private ManageSourceMain sourceMainRef;

        SourceDetailViewModel detailVm;
        public SourceDetailPage(ManageSourceMain sourceMain, int sourceItemKey)
        {
            InitializeComponent();
            sourceMainRef = sourceMain;
            detailVm = new SourceDetailViewModel(sourceItemKey);
            this.DataContext = detailVm;
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(sourceMainRef);
        }

        private void OpenUpdateWindow(object sender, RoutedEventArgs e)
        {
            UpdateDetailWindow updateWindow = new UpdateDetailWindow(detailVm);
            updateWindow.ShowDialog();
        }
        private void OpenAddWindow(object sender, RoutedEventArgs e)
        {
            AddDetailWindow addWindow = new AddDetailWindow(detailVm);
            addWindow.ShowDialog();
        }

    }
}
