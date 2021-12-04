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
using System.Windows.Shapes;

namespace CoffeeStoreManager.Views.ManageEmployee
{
    /// <summary>
    /// Interaction logic for UpdateEmployeeWindow.xaml
    /// </summary>
    public partial class UpdateEmployeeWindow : Window
    {
        public UpdateEmployeeWindow()
        {
            InitializeComponent();
            Style = (Style)FindResource("WindowStyle");

        }
    }
}
