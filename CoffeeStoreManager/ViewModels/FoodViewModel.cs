using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using CoffeeStoreManager.Models;
using CoffeeStoreManager.Resources.Utils;
using CoffeeStoreManager.Views.ManageFood;

namespace CoffeeStoreManager.ViewModels
{
    class FoodViewModel: BaseViewModel
    {
        public ObservableCollection<ViewFood> FoodList { get => foodList; set { foodList = value; OnPropertyChanged(nameof(FoodList)); } }
        public ObservableCollection<LoaiMonAn> FoodTypeList { get => foodTypeList; set { foodTypeList = value; OnPropertyChanged(nameof(FoodTypeList)); } }
        //my views properties
        public string AddFormFoodName { get => addFormFoodName; set { addFormFoodName = value; OnPropertyChanged(nameof(addFormFoodName)); } }
        public int AddFormFoodPrice { get => addFormFoodPrice; set { addFormFoodPrice = value; OnPropertyChanged(nameof(addFormFoodPrice)); } }
        public int AddFormFoodType { get => addFormFoodType; set { addFormFoodType = value; OnPropertyChanged(nameof(addFormFoodType)); } }
        public string AddFormFoodIngredient { get => addFormFoodIngredient; set { addFormFoodIngredient = value; OnPropertyChanged(nameof(addFormFoodIngredient)); } }
        public string AddFormFoodDescription { get => addFormFoodDescription; set { addFormFoodDescription = value; OnPropertyChanged(nameof(addFormFoodDescription)); } }
        public string AddFormFoodImage { get => addFormFoodImage; set { addFormFoodImage = value; OnPropertyChanged(nameof(addFormFoodImage)); } }
        public string AddFormFoodTypeName { get => addFormFoodTypeName; set { addFormFoodTypeName = value; OnPropertyChanged(nameof(addFormFoodTypeName)); } }
        public string FoodImagePath { get => foodImagePath; set { foodImagePath = value; OnPropertyChanged(nameof(foodImagePath)); } }
        public string UpdateFoodImagePath { get => updateFoodImagePath; set { updateFoodImagePath = value; OnPropertyChanged(nameof(updateFoodImagePath)); } }
        public ViewFood SelectedFood { get => selectedFood; set { selectedFood = value; OnPropertyChanged(nameof(SelectedFood)); } }

        public string SearchKey { get => searchKey; set { searchKey = value; OnPropertyChanged(nameof(SearchKey)); } }
        
        
        private ObservableCollection<ViewFood> foodList;
        private ObservableCollection<LoaiMonAn> foodTypeList;
        private string addFormFoodName;
        private int addFormFoodPrice;
        private int addFormFoodType;
        private string addFormFoodIngredient;
        private string addFormFoodDescription;
        private string addFormFoodImage;
        private string foodImagePath;
        private string updateFoodImagePath;


        private string addFormFoodTypeName;

        private ViewFood selectedFood;
        private string searchKey;
        public ICommand AddFood { get; set; }
        public ICommand OpenUpdateFoodWindow { get; set; }
        public ICommand OpenAddFoodWindow { get; set; }
        public ICommand UpdateSelectedFood { get; set; }
        public ICommand Search { get; set; }
        public ICommand DeleteFood { get; set; }
        public ICommand RefreshFoodList { get; set; }
        public ICommand UploadFoodImage { get; set; }
        public ICommand UpdateFormUpLoadImage { get; set; }

        public FoodViewModel()
        {
            loadData();
            AddFood = new RelayCommand<object>((p) => { return true; }, (p) => { addFood(p); });
            OpenAddFoodWindow = new RelayCommand<object>((p) => { return true; }, (p) => { openAddFoodWindow(p); });

            OpenUpdateFoodWindow = new RelayCommand<object>((p) => { return true; }, (p) => { openUpdateFoodWindow(p); });
            UpdateSelectedFood = new RelayCommand<object>((p) => { return true; }, (p) => { updateSlectedFood(); });
            Search = new RelayCommand<object>((p) => { return true; }, (p) => { search(p); });
            DeleteFood = new RelayCommand<object>((p) => { return true; }, (p) => { deleteFood(p); });
            RefreshFoodList = new RelayCommand<object>((p) => { return true; }, (p) => { refreshFoodList(p); });
            UploadFoodImage = new RelayCommand<object>((p) => { return true; }, (p) => { uploadFoodImage(p); });
            UpdateFormUpLoadImage = new RelayCommand<object>((p) => { return true; }, (p) => { updateFormUpLoadImage(p); });
        }

        private void updateFormUpLoadImage(object p)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Only(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                UpdateFoodImagePath = dialog.FileName;
            }
        }

        private void uploadFoodImage(object p)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Only(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                FoodImagePath = dialog.FileName;
            }
        }

        private void refreshFoodList(object p)
        {
            loadFoodList();
        }

        private void deleteFood(object p)
        {
            var selectedFoodId = SelectedFood.ma_mon_an;
            var selectedFood = DataProvider.Ins.DB.MonAns.Where(food => food.ma_mon_an == selectedFoodId).First();
            DataProvider.Ins.DB.MonAns.Remove(selectedFood);
            DataProvider.Ins.DB.SaveChanges();

            //FoodList.Remove(FoodList.Where(item => item.ma_mon_an == selectedFoodId).Single());
            loadFoodList();
        }

        private void search(object p)
        {
            var foodList = DataProvider.Ins.DB.MonAns.Where(food => food.ten_mon_an.ToLower().Contains(SearchKey.ToLower())).ToList();
            var searchResult = getViewFoodFromList(foodList);
            FoodList = searchResult;
        }

        void loadData()
        {
            loadFoodList();
            loadFoodTypeList();
        }
        /// <summary>
        /// MANAGE FOOD
        /// </summary>
        void loadFoodTypeList()
        {
            var distTypeListData = DataProvider.Ins.DB.LoaiMonAns.ToList();
            FoodTypeList = new ObservableCollection<LoaiMonAn>(distTypeListData);

        }
        void loadFoodList()
        {
            var foodListData = DataProvider.Ins.DB.MonAns.ToList();
            var viewFoodCollection = getViewFoodFromList(foodListData);
            FoodList = viewFoodCollection;
        }

        private ObservableCollection<ViewFood> getViewFoodFromList(List<MonAn> foodListData)
        {
            var viewFoodCollection = new ObservableCollection<ViewFood>();
            int index = 1;
            foreach (var food in foodListData)
            {
                LoaiMonAn foodType = DataProvider.Ins.DB.LoaiMonAns.Where(p => p.ma_loai_mon_an == food.ma_loai_mon_an).FirstOrDefault<LoaiMonAn>();
                ViewFood viewFood = new ViewFood()
                {
                    STT = index,
                    ma_mon_an = food.ma_mon_an,
                    ten_mon_an = food.ten_mon_an,
                    gia_tien = food.gia_tien,
                    loai_mon_an = foodType.ten_loai_mon_an,
                    ma_loai_mon_an = foodType.ma_loai_mon_an,
                    nguyen_lieu = food.nguyen_lieu,
                    mo_ta = food.mo_ta,
                    anh = food.anh
                };
                viewFoodCollection.Add(viewFood);
                index++;
            }
            return viewFoodCollection;
        }
        //ADD
        void openAddFoodWindow(object p)
        {
            var window = new AddFoodWindow();
            window.ShowDialog();
        }
        void addFood(object state)
        {
            FoodViewModel vm = state as FoodViewModel;
            Image foodImg = Image.FromFile(FoodImagePath);
            
            MonAn newFood = new MonAn()
            {
                ten_mon_an = vm.AddFormFoodName,
                ma_loai_mon_an = vm.AddFormFoodType,
                gia_tien = vm.AddFormFoodPrice,
                nguyen_lieu = vm.AddFormFoodIngredient,
                mo_ta = vm.AddFormFoodDescription,
                anh = ImageConverterUtil.ImageToByteArray(foodImg)
            };
            DataProvider.Ins.DB.MonAns.Add(newFood);
            DataProvider.Ins.DB.SaveChanges();
            loadFoodList();
            clearAddFoodForm();
        }
        void clearAddFoodForm()
        {
            AddFormFoodName = "";
            AddFormFoodPrice = 0;
            AddFormFoodType = 0;
        }
        //UPDATE
        void openUpdateFoodWindow(object p)
        {
            var window = new UpdateFoodWindow();
            window.ShowDialog();
        }
        private void updateSlectedFood()
        {
            Image foodImg = Image.FromFile(UpdateFoodImagePath);
            var dbSelectedFood = DataProvider.Ins.DB.MonAns.SingleOrDefault(food => food.ma_mon_an == SelectedFood.ma_mon_an);
            dbSelectedFood.ten_mon_an = SelectedFood.ten_mon_an;
            dbSelectedFood.ma_loai_mon_an = SelectedFood.ma_loai_mon_an;
            dbSelectedFood.gia_tien = SelectedFood.gia_tien;
            dbSelectedFood.nguyen_lieu = SelectedFood.nguyen_lieu;
            dbSelectedFood.mo_ta = SelectedFood.mo_ta;
            dbSelectedFood.anh = ImageConverterUtil.ImageToByteArray(foodImg);
            DataProvider.Ins.DB.SaveChanges();
            loadFoodList();
        }

    }
}
