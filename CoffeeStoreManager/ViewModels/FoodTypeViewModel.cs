using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CoffeeStoreManager.Models;

namespace CoffeeStoreManager.ViewModels
{
    class FoodTypeViewModel:BaseViewModel
    {
        public ObservableCollection<LoaiMonAn> FoodTypeList { get => foodTypeList; set { foodTypeList = value; OnPropertyChanged(nameof(FoodTypeList)); } }

        private ObservableCollection<LoaiMonAn> foodTypeList;
        public ICommand AddType { get; set; }
        public ICommand UpdateType { get; set; }
        public ICommand DeleteType { get; set; }

        private string addFormFoodTypeName;
        private LoaiMonAn selectedType;
        public string AddFormFoodTypeName { get => addFormFoodTypeName; set { addFormFoodTypeName = value; OnPropertyChanged(nameof(AddFormFoodTypeName)); } }
        public LoaiMonAn SelectedType { get => selectedType; set { selectedType = value; OnPropertyChanged(nameof(selectedType)); } }

        public FoodTypeViewModel()
        {
            loadFoodTypeList();
            AddType = new RelayCommand<object>((p) => { return true; }, (p) => { addType(p); });
            UpdateType = new RelayCommand<object>((p) => { return true; }, (p) => { updateType(p); });
            DeleteType = new RelayCommand<object>((p) => { return true; }, (p) => { deleteType(p); });

        }

        private void deleteType(object p)
        {
            var dbSelectedType = DataProvider.Ins.DB.LoaiMonAns.SingleOrDefault(type => type.ma_loai_mon_an == SelectedType.ma_loai_mon_an);
            DataProvider.Ins.DB.LoaiMonAns.Remove(dbSelectedType);
            DataProvider.Ins.DB.SaveChanges();
            loadFoodTypeList();
        }

        private void updateType(object p)
        {
            var dbSelectedType = DataProvider.Ins.DB.LoaiMonAns.SingleOrDefault(type => type.ma_loai_mon_an == SelectedType.ma_loai_mon_an);
            dbSelectedType.ten_loai_mon_an = SelectedType.ten_loai_mon_an;
            DataProvider.Ins.DB.SaveChanges();
        }

        void loadFoodTypeList()
        {
            var distTypeListData = DataProvider.Ins.DB.LoaiMonAns.ToList();
            FoodTypeList = new ObservableCollection<LoaiMonAn>(distTypeListData);
        }
        
        private void addType(object state)
        {
            FoodTypeViewModel vm = state as FoodTypeViewModel;
            LoaiMonAn newType = new LoaiMonAn()
            {
                ten_loai_mon_an = vm.AddFormFoodTypeName,
            };
            DataProvider.Ins.DB.LoaiMonAns.Add(newType);
            DataProvider.Ins.DB.SaveChanges();
            loadFoodTypeList();
            addFormFoodTypeName = "";
        }

    }
}
