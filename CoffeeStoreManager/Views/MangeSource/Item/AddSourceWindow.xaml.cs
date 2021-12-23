
using System.Windows;
using CoffeeStoreManager.ViewModels;

namespace CoffeeStoreManager.Views.MangeSource.Item
{
    /// <summary>
    /// Interaction logic for AddSourceWindow.xaml
    /// </summary>
    public partial class AddSourceWindow : Window
    {
        public AddSourceWindow(SourceViewModel sourceViewModel)
        {
            InitializeComponent();
            AddSourceViewModel addSourceVM = new AddSourceViewModel(sourceViewModel);

            this.DataContext = addSourceVM;
            Style = (Style)FindResource("WindowStyle");
        }
    }
}
