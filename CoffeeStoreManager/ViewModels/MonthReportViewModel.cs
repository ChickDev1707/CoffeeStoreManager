using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CoffeeStoreManager.Models;

namespace CoffeeStoreManager.ViewModels
{
    public class MonthReportViewModel:BaseViewModel
    {
        public ObservableCollection<MonthReport> Report { get => report; set { report = value; OnPropertyChanged(nameof(report)); } }
        private ObservableCollection<MonthReport> report;

        public ICommand RenderReport { get; set; }
        public int Month { get => month; set { month = value; OnPropertyChanged(nameof(Month)); } }
        private int month;
        public int Year { get => year; set { year = value; OnPropertyChanged(nameof(Year)); } }
        private int year;
        public MonthReportViewModel()
        {
            RenderReport = new RelayCommand<object>((p) => { return true; }, renderReport);
            loadTime();
        }
        private void loadTime()
        {
            var now = DateTime.Now;
            Month = now.Month;
            Year = now.Year;
        }
        private void renderReport(object obj)
        {
            var db = DataProvider.Ins.DB;
            var sqlStringFormat = "select ten_loai_mon_an, sum(so_luong) as tong_so_luong, sum(thanh_tien) as tong_so_tien "
                            + "from LoaiMonAn as fType, MonAn as f, HoaDon as bill, CT_HoaDon as dBill "
                            + "where fType.ma_loai_mon_an = f.ma_loai_mon_an and f.ma_mon_an = dBill.ma_mon_an and dBill.ma_hoa_don = bill.ma_hoa_don and month(ngay_xuat_hoa_don) = {0} and year(ngay_xuat_hoa_don) = {1} "
                            + "group by fType.ma_loai_mon_an, ten_loai_mon_an";
            var sqlString = String.Format(sqlStringFormat, Month, Year);
            var dataList = db.Database.SqlQuery<MonthReport>(sqlString).ToList();
            Report = new ObservableCollection<MonthReport>(dataList);
        }
    }
}
