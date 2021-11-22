
using System.Windows;
using CoffeeStoreManager.ViewModels;

namespace CoffeeStoreManager.Views.MangeSource.Item
{
    /// <summary>
    /// Interaction logic for AddSourceWindow.xaml
    /// </summary>
    public partial class AddSourceWindow : Window
    {
        public AddSourceWindow()
        {
            InitializeComponent();
            Style = (Style)FindResource("WindowStyle");

        }
    }
}
