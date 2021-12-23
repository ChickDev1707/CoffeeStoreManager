
using System.Windows;
using CoffeeStoreManager.ViewModels;

namespace CoffeeStoreManager.Views.MangeSource.Item
{
    /// <summary>
    /// Interaction logic for UpdateSourceWindow.xaml
    /// </summary>
    public partial class UpdateSourceWindow : Window
    {
        public UpdateSourceWindow(SourceViewModel sourceViewModel)
        {
            InitializeComponent();
            UpdateSourceViewModel updateSourceVM = new UpdateSourceViewModel(sourceViewModel);

            this.DataContext = updateSourceVM;
            Style = (Style)FindResource("WindowStyle");

        }
    }
}
