
using System.Windows;
using CoffeeStoreManager.ViewModels;

namespace CoffeeStoreManager.Views.MangeSource.Detail
{
    /// <summary>
    /// Interaction logic for AddDetailWindow.xaml
    /// </summary>
    public partial class AddDetailWindow : Window
    {
        public AddDetailWindow(SourceDetailViewModel vm)
        {
            InitializeComponent();
            this.DataContext = vm;
        }
    }
}
