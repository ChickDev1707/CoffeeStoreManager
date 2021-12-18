using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using CoffeeStoreManager.Models;
using CoffeeStoreManager.Resources.Utils;
using MaterialDesignThemes.Wpf;

namespace CoffeeStoreManager.ViewModels
{
    class FoodTypeViewModel:BaseViewModel
    {
        public ObservableCollection<LoaiMonAn> FoodTypeList { get => foodTypeList; set { foodTypeList = value; OnPropertyChanged(nameof(FoodTypeList)); } }

        private ObservableCollection<LoaiMonAn> foodTypeList;
        public ICommand AddType { get; set; }
        public ICommand UpdateType { get; set; }
        public ICommand DeleteType { get; set; }

        public string NewFoodTypeName { get => newFoodTypeName; set { newFoodTypeName = value; OnPropertyChanged(nameof(NewFoodTypeName)); } }
        private string newFoodTypeName;

        public string SelectedUpdateFoodTypeName { get => selectedUpdateFoodTypeName; set { selectedUpdateFoodTypeName = value; OnPropertyChanged(nameof(SelectedUpdateFoodTypeName)); } }
        private string selectedUpdateFoodTypeName;
        public SnackbarMessageQueue MyMessageQueue { get => myMessageQueue; set { myMessageQueue = value; OnPropertyChanged(nameof(MyMessageQueue)); } }
        private SnackbarMessageQueue myMessageQueue;
        public LoaiMonAn SelectedType { get => selectedType; set 
            { 
                selectedType = value;
                if(value!= null) SelectedUpdateFoodTypeName = value.ten_loai_mon_an;
            } 
        }
        private LoaiMonAn selectedType;

        public FoodTypeViewModel()
        {
            loadFoodTypeList();
            AddType = new RelayCommand<StackPanel>((p) => { return true; }, (p) => { addType(p); });
            UpdateType = new RelayCommand<StackPanel>((p) => { return true; }, (p) => { updateType(p); });
            DeleteType = new RelayCommand<object>((p) => { return true; }, (p) => { deleteType(); });

            MyMessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(4000));
            MyMessageQueue.DiscardDuplicates = true;
        }

        private void deleteType()
        {
            var dbSelectedType = DataProvider.Ins.DB.LoaiMonAns.SingleOrDefault(type => type.ma_loai_mon_an == SelectedType.ma_loai_mon_an);
            DataProvider.Ins.DB.LoaiMonAns.Remove(dbSelectedType);
            DataProvider.Ins.DB.SaveChanges();
            loadFoodTypeList();
        }

        private void updateType(StackPanel updateTypeForm)
        {
            if (Validator.IsValid(updateTypeForm))
            {
                var dbSelectedType = DataProvider.Ins.DB.LoaiMonAns.SingleOrDefault(type => type.ma_loai_mon_an == SelectedType.ma_loai_mon_an);
                dbSelectedType.ten_loai_mon_an = SelectedUpdateFoodTypeName;
                DataProvider.Ins.DB.SaveChanges();
                loadFoodTypeList();
                MyMessageQueue.Enqueue("Cập nhật loại món ăn thành công!");
            }
            else
            {
                MyMessageQueue.Enqueue("Lỗi. Thông tin món ăn không hợp lệ");
            }
        }

        void loadFoodTypeList()
        {
            var distTypeListData = DataProvider.Ins.DB.LoaiMonAns.ToList();
            FoodTypeList = new ObservableCollection<LoaiMonAn>(distTypeListData);
        }
        
        private void addType(StackPanel addTypeForm)
        {
            if (Validator.IsValid(addTypeForm))
            {
                LoaiMonAn newType = new LoaiMonAn()
                {
                    ten_loai_mon_an = NewFoodTypeName,
                };
                DataProvider.Ins.DB.LoaiMonAns.Add(newType);
                DataProvider.Ins.DB.SaveChanges();
                loadFoodTypeList();
                NewFoodTypeName = "";
                MyMessageQueue.Enqueue("Thêm loại món ăn thành công!");
            }
            else
            {
                MyMessageQueue.Enqueue("Lỗi. Thông tin món ăn không hợp lệ");
            }
        }

    }
}
