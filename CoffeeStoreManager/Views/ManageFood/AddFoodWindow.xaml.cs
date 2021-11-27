
using System.Windows;
using CoffeeStoreManager.ViewModels;

namespace CoffeeStoreManager.Views.ManageFood
{
    /// <summary>
    /// Interaction logic for AddFoodWindow.xaml
    /// </summary>
    public partial class AddFoodWindow : Window
    {
        public AddFoodWindow(FoodViewModel foodVm)
        {
            InitializeComponent();

            AddFoodViewModel addFoodVm = new AddFoodViewModel(foodVm);
            this.DataContext = addFoodVm;
        }
    }
}
