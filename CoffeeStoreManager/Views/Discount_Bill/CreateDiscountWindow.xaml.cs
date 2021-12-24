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

namespace CoffeeStoreManager.Views.Discount_Bill
{
    /// <summary>
    /// Interaction logic for CreateDiscountWindow.xaml
    /// </summary>
    public partial class CreateDiscountWindow : Window
    {
        public CreateDiscountWindow()
        {
            InitializeComponent();
            CreateDiscountViewModel createdicountvm = new CreateDiscountViewModel();
            this.DataContext = createdicountvm;
        }
    }
}
