using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CoffeeStoreManager.Views.ManageFood
{
    /// <summary>
    /// Interaction logic for FoodTypePage.xaml
    /// </summary>
    public partial class FoodTypePage : Page
    {
        public FoodTypePage()
        {
            InitializeComponent();
            Style = (Style)FindResource("WindowStyle");

        }
    }
}
