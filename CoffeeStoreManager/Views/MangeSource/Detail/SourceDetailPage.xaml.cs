
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

        SourceViewModel sourceVm;
        public SourceDetailPage(ManageSourceMain sourceMain, int sourceItemKey, SourceViewModel sourceViewModel)
        {
            InitializeComponent();
            sourceMainRef = sourceMain;
            SourceDetailViewModel detailVm = new SourceDetailViewModel(sourceItemKey);
            sourceVm = sourceViewModel;

            this.DataContext = detailVm;
            Style = (Style)FindResource("WindowStyle");

        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.sourceVm.LoadSourceList();
            NavigationService.Navigate(sourceMainRef);
        }
    }
}
