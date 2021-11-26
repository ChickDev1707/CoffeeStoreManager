using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CoffeeStoreManager.Models;

namespace CoffeeStoreManager.ViewModels
{
    public class MonthReportViewModel:BaseViewModel
    {
        private ObservableCollection<MonthReport> report;
        public ObservableCollection<MonthReport> Report { get => report; set { report = value; OnPropertyChanged(nameof(report)); } }

        private DateTime targetTime;
        public ICommand RenderReport { get; set; }
        public DateTime TargetTime { get => targetTime; set { targetTime = value; OnPropertyChanged(nameof(targetTime)); } }

        public MonthReportViewModel()
        {
            RenderReport = new RelayCommand<object>((p) => { return true; }, renderReport);
        }

        private void renderReport(object obj)
        {
            var db = DataProvider.Ins.DB;
            var sqlString = "select ten_loai_mon_an, sum(so_luong) as tong_so_luong, sum(thanh_tien) as tong_so_tien "
                            + "from LoaiMonAn as fType, MonAn as f, HoaDon as bill, CT_HoaDon as dBill "
                            + "where fType.ma_loai_mon_an = f.ma_loai_mon_an and f.ma_mon_an = dBill.ma_mon_an and dBill.ma_hoa_don = bill.ma_hoa_don and month(ngay_xuat_hoa_don) = 11 and year(ngay_xuat_hoa_don) = 2021 "
                            + "group by fType.ma_loai_mon_an, ten_loai_mon_an";
            var dataList = db.Database.SqlQuery<MonthReport>(sqlString).ToList();
            Report = new ObservableCollection<MonthReport>(dataList);
        }
    }
}
