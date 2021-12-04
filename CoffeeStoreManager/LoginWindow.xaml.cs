
using System.Windows;

namespace CoffeeStoreManager
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            Style = (Style)FindResource("WindowStyle");
        }
    }
}
