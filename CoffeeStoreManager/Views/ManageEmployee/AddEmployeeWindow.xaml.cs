using CoffeeStoreManager.ViewModels;
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
    /// Interaction logic for AddEmployeeWindow.xaml
    /// </summary>
    public partial class AddEmployeeWindow : Window
    {
        public AddEmployeeWindow(EmployeeViewModel eVM)
        {
            InitializeComponent();
            AddEmployeeViewModel addVM = new AddEmployeeViewModel(eVM);
            this.DataContext = addVM;
            Style = (Style)FindResource("WindowStyle");

        }
    }
}
