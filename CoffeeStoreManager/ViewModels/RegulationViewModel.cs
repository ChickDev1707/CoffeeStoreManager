using CoffeeStoreManager.Models;
using CoffeeStoreManager.Resources.Utils;
using CoffeeStoreManager.Views.Regulation;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CoffeeStoreManager.ViewModels
{
    public class RegulationViewModel : BaseViewModel
    {
        private int _id;
        private int _numberOfTable;
        private int _minAge;
        private int _maxAge;
        private int _specialOffer;
        private int _specialOfferCount;
        private decimal _specialOfferMoney;
        private ObservableCollection<LoaiMonAn> foodTypeList;
        public SnackbarMessageQueue MyMessageQueue { get => myMessageQueue; set { myMessageQueue = value; OnPropertyChanged(nameof(MyMessageQueue)); } }
        private SnackbarMessageQueue myMessageQueue;
        public ObservableCollection<LoaiMonAn> FoodTypeList { get => foodTypeList; set { foodTypeList = value; OnPropertyChanged(nameof(FoodTypeList)); } }
        private LoaiMonAn _selectedFoodType;
        public LoaiMonAn SelectedFoodType { get => _selectedFoodType; set { _selectedFoodType = value; OnPropertyChanged(nameof(SelectedFoodType)); } }
        public int Id { get => _id; set { _id = value; OnPropertyChanged(); } }
        public int NumberOfTable { get => _numberOfTable; set { _numberOfTable = value; OnPropertyChanged(); } }
        public int MinAge { get => _minAge; set { _minAge = value; OnPropertyChanged(); } }
        public int MaxAge { get => _maxAge; set { _maxAge = value; OnPropertyChanged(); } }
        public int SpecialOffer { get => _specialOffer; set { _specialOffer = value; OnPropertyChanged(); } }
        public int SpecialOfferCount { get => _specialOfferCount; set { _specialOfferCount = value; OnPropertyChanged(); } }
        public decimal SpecialOfferMoney { get => _specialOfferMoney; set { _specialOfferMoney = value; OnPropertyChanged(); } }

        public ICommand OpenRegulationChange { get; set; }
        public ICommand SaveRegulationChange { get; set; }

        public RegulationViewModel()
        {
            loadFoodTypeList();
            var select = from s in DataProvider.Ins.DB.QuyDinhs select s;
            foreach (var data in select)
            {               
                Id = 1;
                NumberOfTable = (int)data.so_ban;
                MinAge = (int)data.tuoi_toi_thieu_nv;
                MaxAge = (int)data.tuoi_toi_da_nv;
                SpecialOffer = (int)data.loai_san_pham_uu_dai;
                SpecialOfferCount = (int)data.count_uu_dai;
                SpecialOfferMoney = (decimal)data.muc_tien_nhan_uu_dai;  
            }

            SaveRegulationChange = new RelayCommand<StackPanel>((p) => { return true; }, (p) => { SaveRegulation(p); });
            FoodTypeList = new ObservableCollection<LoaiMonAn>(DataProvider.Ins.DB.LoaiMonAns);

            MyMessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(4000));
            MyMessageQueue.DiscardDuplicates = true;
        }
        void SaveRegulation(StackPanel regulationChangeForm)
        {
            if (Validator.IsValid(regulationChangeForm))
            {
                var change = DataProvider.Ins.DB.QuyDinhs.SingleOrDefault(x => x.ma_quy_dinh == 1);
                change.so_ban = NumberOfTable;
                change.tuoi_toi_thieu_nv = MinAge;
                change.tuoi_toi_da_nv = MaxAge;
                change.loai_san_pham_uu_dai = SpecialOffer;
                change.count_uu_dai = SpecialOfferCount;
                change.muc_tien_nhan_uu_dai = SpecialOfferMoney;
                DataProvider.Ins.DB.SaveChanges();
                MyMessageQueue.Enqueue("Lưu quy định thành công!");
            }
            else
            {
                MyMessageQueue.Enqueue("Lỗi. Quy định không hợp lệ");
            } 
                
        }
        void loadFoodTypeList()
        {
            var distTypeListData = DataProvider.Ins.DB.LoaiMonAns.ToList();
            FoodTypeList = new ObservableCollection<LoaiMonAn>(distTypeListData);
        }
    }
}
