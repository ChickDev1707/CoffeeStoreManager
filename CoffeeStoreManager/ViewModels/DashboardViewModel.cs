using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeStoreManager.Models;

namespace CoffeeStoreManager.ViewModels
{
    public class DashboardViewModel:BaseViewModel
    {
        public int FoodCount { get => foodCount; set { foodCount = value; OnPropertyChanged(nameof(FoodCount)); } }
        private int foodCount;
        public int EmployeeCount { get => employeeCount; set { employeeCount = value; OnPropertyChanged(nameof(EmployeeCount)); } }
        private int employeeCount;

        public decimal? DayRevenue { get => dayRevenue; set { dayRevenue = value; OnPropertyChanged(nameof(DayRevenue)); } }
        private decimal? dayRevenue;
        public decimal? DaySpend { get => daySpend; set { daySpend = value; OnPropertyChanged(nameof(DaySpend)); } }
        private decimal? daySpend;

        public ObservableCollection<LineChartModel> ProfitChartData { get => profitChartData; set { profitChartData = value; OnPropertyChanged(nameof(ProfitChartData)); } }
        private ObservableCollection<LineChartModel> profitChartData;

        public ObservableCollection<FoodTypeChartModel> FoodTypeChartData { get => foodTypeChartData; set => foodTypeChartData = value; }
        private ObservableCollection<FoodTypeChartModel> foodTypeChartData;

        public string SelectedItemName { get => selectedItemName; set { selectedItemName = value; OnPropertyChanged(nameof(SelectedItemName)); } }
        private string selectedItemName;

        public int SelectedItemPercentage { get => selectedItemPercentage; set { selectedItemPercentage = value; OnPropertyChanged(nameof(SelectedItemPercentage)); } }
        private int selectedItemPercentage;

        public string AdminName { get => adminName; set { adminName = value; OnPropertyChanged(nameof(AdminName)); } }
        private string adminName;
        public int SelectedIndex { get => selectedIndex; set
            {
                selectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
                if(value!= -1)
                {
                    SelectedItemName = FoodTypeChartData[SelectedIndex].ten_loai_mon_an;
                    SelectedItemPercentage = FoodTypeChartData[SelectedIndex].ty_le;
                }
            }
        }

        private int selectedIndex;

        public DashboardViewModel()
        {
            loadData();
        }
        private void loadData()
        {
            AdminName = DataProvider.Ins.DB.TaiKhoanAdmins.FirstOrDefault().ho_ten;
            FoodCount = DataProvider.Ins.DB.MonAns.ToList().Count();
            EmployeeCount = DataProvider.Ins.DB.NhanViens.ToList().Count();
            loadDayRevenue();
            loadDaySpend();
            loadProfitChart();
            loadFoodTypeChart();
        }
        private void loadDayRevenue()
        {
            DateTime date = DateTime.Now;
            
            var db = DataProvider.Ins.DB;
            var sqlStringFormat = "select sum(tong_tien) "
                            + "from HoaDon "
                            + "where DAY(ngay_xuat_hoa_don) = {0} and MONTH(ngay_xuat_hoa_don) = {1} and YEAR(ngay_xuat_hoa_don) = {2} ";
            var sqlString = String.Format(sqlStringFormat, date.Day, date.Month, date.Year);
            decimal? result = db.Database.SqlQuery<decimal?>(sqlString).FirstOrDefault();
            DayRevenue = result == null ? 0 : result;
        }
        private void loadDaySpend()
        {
            DateTime date = DateTime.Now;

            var db = DataProvider.Ins.DB;
            var sqlStringFormat = "select sum(tong_tien) "
                            + "from PhieuNhapHang "
                            + "where DAY(ngay_nhap) = {0} and MONTH(ngay_nhap) = {1} and YEAR(ngay_nhap) = {2} ";
            var sqlString = String.Format(sqlStringFormat, date.Day, date.Month, date.Year);
            decimal? result = db.Database.SqlQuery<decimal?>(sqlString).FirstOrDefault();
            DaySpend = result == null ? 0 : result;
        }
        private void loadProfitChart()
        {
            ProfitChartData = new ObservableCollection<LineChartModel>();
            var noMonth = 6;
            DateTime date = DateTime.Now;
            for(int i = date.Month- noMonth; i<= date.Month; i++)
            {
                LineChartModel chartModel = new LineChartModel(calculateProfitOfMonth(i, date.Year), i);
                ProfitChartData.Add(chartModel);
            }
        }
        private decimal calculateProfitOfMonth(int month, int year)
        {
            decimal tong = 0;
            List<HoaDon> list = DataProvider.Ins.DB.HoaDons
                .Where(p => p.ngay_xuat_hoa_don.Value.Year == year && p.ngay_xuat_hoa_don.Value.Month == month).ToList();
            PhieuTinhLuong luong = DataProvider.Ins.DB.PhieuTinhLuongs.
                Where(p => p.ngay_tinh_luong.Value.Year == year && p.ngay_tinh_luong.Value.Month == month).FirstOrDefault();
            List<PhieuNhapHang> phieuNhapHang = DataProvider.Ins.DB.PhieuNhapHangs.
                Where(p => p.ngay_nhap.Value.Year == year && p.ngay_nhap.Value.Month == month).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                tong = tong + (decimal)list[i].tong_tien;
            }
            for (int i = 0; i < phieuNhapHang.Count; i++)
            {
                tong = tong - (decimal)phieuNhapHang[i].tong_tien;
            }
            if (luong != null)
            {
                tong = tong - (decimal)luong.tong_tien;
            }
            tong = tong / 1000;
            return tong;
        }
        private void loadFoodTypeChart()
        {
            var db = DataProvider.Ins.DB;

            var sqlString = "select fType.ma_loai_mon_an, ten_loai_mon_an, ROUND((count(*)*100)/(select count(*) from MonAn), 2) as ty_le "
                            + "from LoaiMonAn as fType, MonAn as food "
                            + "where fType.ma_loai_mon_an = food.ma_loai_mon_an "
                            + "group by fType.ma_loai_mon_an, ten_loai_mon_an ";
            var result = db.Database.SqlQuery<FoodTypeChartModel>(sqlString).ToList();
            foodTypeChartData = new ObservableCollection<FoodTypeChartModel>(result);
        }
    }
}
