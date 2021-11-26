
using System;
using CoffeeStoreManager.ViewModels;
using Syncfusion.UI.Xaml.Scheduler;

namespace CoffeeStoreManager.Models {
  
    public class PartTimeShift: BaseViewModel
    {
        private int ma_ca_partTime;
        private Nullable<int> ma_nhan_vien;
        private string ten_nhan_vien;
        private DateTime bat_dau;
        private DateTime ket_thuc;
        public int Ma_ca_partTime { get => ma_ca_partTime; set { ma_ca_partTime = value; OnPropertyChanged(nameof(Ma_ca_partTime)); } }
        public int? Ma_nhan_vien { get => ma_nhan_vien; set { ma_nhan_vien = value; OnPropertyChanged(nameof(Ma_nhan_vien)); } }
        public string Ten_nhan_vien { get => ten_nhan_vien; set { ten_nhan_vien = value; OnPropertyChanged(nameof(Ten_nhan_vien)); } }
        public DateTime Bat_dau { get => bat_dau; set { bat_dau = value; OnPropertyChanged(nameof(Bat_dau)); } }
        public DateTime Ket_thuc { get => ket_thuc; set { ket_thuc = value; OnPropertyChanged(nameof(Ket_thuc)); } }

        public static PartTimeShift fromDbShift(CaLamPartTime dbShift)
        {
            DateTime? start = dbShift.ngay_lam + dbShift.gio_bat_dau;
            DateTime? end = dbShift.ngay_lam + dbShift.gio_ket_thuc;
            return new PartTimeShift()
            {
                Ma_ca_partTime = dbShift.ma_ca_partTime,
                Ma_nhan_vien = dbShift.ma_nhan_vien,
                Ten_nhan_vien = dbShift.NhanVien.ho_ten,
                Bat_dau = (DateTime)start,
                Ket_thuc = (DateTime)end
            };
        }
    }
}
