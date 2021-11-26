using CoffeeStoreManager.Models;
using CoffeeStoreManager.Views.Regulation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CoffeeStoreManager.ViewModels
{
    public class RegulationViewModel : BaseViewModel
    {
        private int _id;
        private int _numberOfTable;
        private int _minAge;
        private int _maxAge;
        private string _specialOffer;
        private int _specialOfferCount;
        private decimal _specialOfferMoney;
        private ObservableCollection<LoaiMonAn> foodTypeList;
        public ObservableCollection<LoaiMonAn> FoodTypeList { get => foodTypeList; set { foodTypeList = value; OnPropertyChanged(nameof(FoodTypeList)); } }
        private LoaiMonAn _selectedFoodType;
        public LoaiMonAn SelectedFoodType { get => _selectedFoodType; set { _selectedFoodType = value; OnPropertyChanged(nameof(SelectedFoodType)); } }
        public int Id { get => _id; set { _id = value; OnPropertyChanged(); } }
        public int NumberOfTable { get => _numberOfTable; set { _numberOfTable = value; OnPropertyChanged(); } }
        public int MinAge { get => _minAge; set { _minAge = value; OnPropertyChanged(); } }
        public int MaxAge { get => _maxAge; set { _maxAge = value; OnPropertyChanged(); } }
        public string SpecialOffer { get => _specialOffer; set { _specialOffer = value; OnPropertyChanged(); } }
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
                //CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
                

                Id = 1;
                NumberOfTable = (int)data.so_ban;
                MinAge = (int)data.tuoi_toi_thieu_nv;
                MaxAge = (int)data.tuoi_toi_da_nv;
                SpecialOfferCount = (int)data.count_uu_dai;
                SpecialOfferMoney = (decimal)data.muc_tien_nhan_uu_dai;
                var select1 = DataProvider.Ins.DB.LoaiMonAns.Where(x => x.ma_loai_mon_an == data.loai_san_pham_uu_dai);
                foreach (var data1 in select1)
                {
                    SpecialOffer = data1.ten_loai_mon_an;
                }    
            }

            SaveRegulationChange = new RelayCommand<object>((p) => { return true; }, (p) => { SaveRegulation(p); });
            FoodTypeList = new ObservableCollection<LoaiMonAn>(DataProvider.Ins.DB.LoaiMonAns);
        }
        void SaveRegulation(object p)
        {
            if (NumberOfTable <= 0 || MinAge <= 0 || MaxAge <= 0 || SpecialOfferCount <= 0 || string.IsNullOrEmpty(NumberOfTable.ToString()) )
            {
                MessageBox.Show("Thông tin không hợp lệ!");
            }
            else
            {
                var change = DataProvider.Ins.DB.QuyDinhs.SingleOrDefault(x => x.ma_quy_dinh == 1);
                change.so_ban = NumberOfTable;
                change.tuoi_toi_thieu_nv = MinAge;
                change.tuoi_toi_da_nv = MaxAge;
                change.loai_san_pham_uu_dai = SelectedFoodType.ma_loai_mon_an;
                change.count_uu_dai = SpecialOfferCount;
                change.muc_tien_nhan_uu_dai = SpecialOfferMoney;
                MessageBox.Show("Lưu thành công!");
                DataProvider.Ins.DB.SaveChanges();
            }              
        }
        void loadFoodTypeList()
        {
            var distTypeListData = DataProvider.Ins.DB.LoaiMonAns.ToList();
            FoodTypeList = new ObservableCollection<LoaiMonAn>(distTypeListData);
        }
    }
}
