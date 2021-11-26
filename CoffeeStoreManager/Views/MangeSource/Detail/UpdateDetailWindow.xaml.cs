
using System.Windows;
using CoffeeStoreManager.ViewModels;

namespace CoffeeStoreManager.Views.MangeSource.Detail
{
    /// <summary>
    /// Interaction logic for UpdateDetailWindow.xaml
    /// </summary>
    public partial class UpdateDetailWindow : Window
    {
        public UpdateDetailWindow(SourceDetailViewModel vm)
        {
            InitializeComponent();
            this.DataContext = vm;
            Style = (Style)FindResource("WindowStyle");

        }
    }
}
