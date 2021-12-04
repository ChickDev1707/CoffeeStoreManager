
using System.Windows;
using CoffeeStoreManager.ViewModels;

namespace CoffeeStoreManager.Views.ManageFood
{
    /// <summary>
    /// Interaction logic for UpdateFoodWindow.xaml
    /// </summary>
    public partial class UpdateFoodWindow : Window
    {
        public UpdateFoodWindow(FoodViewModel foodVm)
        {
            InitializeComponent();
            UpdateFoodViewModel updateFoodVm = new UpdateFoodViewModel(foodVm);
            this.DataContext = updateFoodVm;
            Style = (Style)FindResource("WindowStyle");
        }
    }
}
