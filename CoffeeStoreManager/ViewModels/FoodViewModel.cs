using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CoffeeStoreManager.Models;
using CoffeeStoreManager.Views.ManageFood;
using MessageBox = System.Windows.Forms.MessageBox;

namespace CoffeeStoreManager.ViewModels
{
    public class FoodViewModel: BaseViewModel
    {
        public ObservableCollection<MonAn> FoodList { get => foodList; set { foodList = value; OnPropertyChanged(nameof(FoodList)); } }
        public MonAn SelectedFood { get => selectedFood; set { selectedFood = value; OnPropertyChanged(nameof(SelectedFood)); } }
        public string SearchKey { get => searchKey; set { searchKey = value; OnPropertyChanged(nameof(SearchKey)); } }
        
        private ObservableCollection<MonAn> foodList;
        private MonAn selectedFood;
        private string searchKey;
        
        public ICommand OpenAddFoodWindow { get; set; }
        public ICommand OpenUpdateFoodWindow { get; set; }
        public ICommand Search { get; set; }
        public ICommand DeleteFood { get; set; }
        public ICommand RefreshFoodList { get; set; }

        public FoodViewModel()
        {
            LoadFoodList();

            OpenAddFoodWindow = new RelayCommand<object>((p) => { return true; }, (p) => { openAddFoodWindow(p); });
            OpenUpdateFoodWindow = new RelayCommand<object>((p) => { return true; }, (p) => { openUpdateFoodWindow(p); });
            Search = new RelayCommand<object>((p) => { return true; }, (p) => { search(p); });
            DeleteFood = new RelayCommand<object>((p) => { return true; }, (p) => { deleteFood(p); });
            RefreshFoodList = new RelayCommand<object>((p) => { return true; }, (p) => { refreshFoodList(p); });
        }
        
        public void LoadFoodList()
        {
            var foodList = DataProvider.Ins.DB.MonAns.ToList();
            FoodList = new ObservableCollection<MonAn>(foodList);
        }

        void openAddFoodWindow(object p)
        {
            var window = new AddFoodWindow(this);
            window.ShowDialog();
        } 
        void openUpdateFoodWindow(object p)
        {
            if(SelectedFood != null)
            {
                var window = new UpdateFoodWindow(this);
                window.ShowDialog();
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn món ăn");
            }
        }
        private void refreshFoodList(object p)
        {
            LoadFoodList();
        }

        private void deleteFood(object p)
        {
            var selectedFoodId = SelectedFood.ma_mon_an;
            var selectedFood = DataProvider.Ins.DB.MonAns.Where(food => food.ma_mon_an == selectedFoodId).First();
            DataProvider.Ins.DB.MonAns.Remove(selectedFood);
            DataProvider.Ins.DB.SaveChanges();

            FoodList.Remove(FoodList.Where(item => item.ma_mon_an == selectedFoodId).Single());
        }

        private void search(object p)
        {
            var searchResult = DataProvider.Ins.DB.MonAns.Where(food => food.ten_mon_an.ToLower().Contains(SearchKey.ToLower())).ToList();
            FoodList = new ObservableCollection<MonAn>(searchResult);
        }

    }
}
