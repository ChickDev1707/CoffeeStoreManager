using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CoffeeStoreManager.Models;
using CoffeeStoreManager.Views.ManageFood;

namespace CoffeeStoreManager.ViewModels
{
    class ManageFoodViewModel: BaseViewModel
    {
        public ObservableCollection<ViewFood> FoodList { get => foodList; set { foodList = value; OnPropertyChanged(nameof(FoodList)); } }
        public ObservableCollection<LoaiMonAn> FoodTypeList { get => foodTypeList; set { foodTypeList = value; OnPropertyChanged(nameof(FoodTypeList)); } }
        //my views properties
        public string foodName { get => _foodName; set { _foodName = value; OnPropertyChanged(nameof(foodName)); } }
        public int foodPrice { get => _foodPrice; set { _foodPrice = value; OnPropertyChanged(nameof(foodPrice)); } }
        public int foodType { get => _foodType; set { _foodType = value; OnPropertyChanged(nameof(foodType)); } }
        public string foodTypeName { get => _foodTypeName; set { _foodTypeName = value; OnPropertyChanged(nameof(foodTypeName)); } }
        public ViewFood SelectedFood { get => selectedFood; set { selectedFood = value; OnPropertyChanged(nameof(SelectedFood)); } }


        private ObservableCollection<ViewFood> foodList;
        private ObservableCollection<LoaiMonAn> foodTypeList;
        private string _foodName;
        private int _foodPrice;
        private int _foodType;
        private string _foodTypeName;
        private ViewFood selectedFood;
        public ICommand AddFood { get; set; }
        public ICommand AddFoodType { get; set; }
        public ICommand OpenUpdateWindow { get; set; }

        public ManageFoodViewModel()
        {
            loadData();
            AddFood = new RelayCommand<object>((p) => { return true; }, (p) => { addFood(p); });
            AddFoodType = new RelayCommand<object>((p) => { return true; }, (p) => { addFoodType(p); });
            OpenUpdateWindow = new RelayCommand<object>((p) => { return true; }, (p) => { openUpdateWindow(p); });

        }
        private void openUpdateWindow(object p)
        {
            var updateWindow = new UpdateFoodWindow();
            updateWindow.ShowDialog();
        }

        private void addFoodType(object state)
        {
            ManageFoodViewModel vm = state as ManageFoodViewModel;
            LoaiMonAn newFoodType = new LoaiMonAn()
            {
                ten_loai_mon_an = vm.foodTypeName
            };
            DataProvider.Ins.DB.LoaiMonAns.Add(newFoodType);
            DataProvider.Ins.DB.SaveChanges();
            loadFoodTypeList();
            clearAddFoodTypeForm();
        }


        void addFood(object state)
        {
            ManageFoodViewModel vm = state as ManageFoodViewModel;
            MonAn newFood = new MonAn()
            {
                ten_mon_an = vm.foodName,
                ma_loai_mon_an = vm.foodType,
                gia_tien = vm.foodPrice
            };
            DataProvider.Ins.DB.MonAns.Add(newFood);
            DataProvider.Ins.DB.SaveChanges();
            loadFoodList();
            clearAddFoodForm();
        }
        public void loadData()
        {
            loadFoodList();
            loadFoodTypeList();
        }
        void clearAddFoodForm()
        {
            foodName = "";
            foodPrice = 0;
            foodType = 0;
        }
        private void clearAddFoodTypeForm()
        {
            foodTypeName = "";
        }
        void loadFoodList()
        {
            var foodListData = DataProvider.Ins.DB.MonAns.ToList();
            FoodList = new ObservableCollection<ViewFood>();
            int index = 1;
            foreach (var food in foodListData)
            {
                LoaiMonAn foodType = DataProvider.Ins.DB.LoaiMonAns.Where(p => p.ma_loai_mon_an == food.ma_loai_mon_an).FirstOrDefault<LoaiMonAn>();
                ViewFood viewFood = new ViewFood()
                {
                    STT = index,
                    ten_mon_an = food.ten_mon_an,
                    gia_tien = food.gia_tien,
                    loai_mon_an = foodType.ten_loai_mon_an,
                    ma_loai_mon_an = foodType.ma_loai_mon_an
                };
                index++;
                FoodList.Add(viewFood);
            }
        }
        void loadFoodTypeList()
        {
            var distTypeListData = DataProvider.Ins.DB.LoaiMonAns.ToList();
            FoodTypeList = new ObservableCollection<LoaiMonAn>(distTypeListData);
        }
    }
}
