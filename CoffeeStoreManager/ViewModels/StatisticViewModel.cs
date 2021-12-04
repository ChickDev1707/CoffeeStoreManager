using CoffeeStoreManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Globalization;
using System.Windows.Controls;
using Microsoft.Office.Interop.Excel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CoffeeStoreManager.Resources.Utils;
namespace CoffeeStoreManager.ViewModels
{
    public class StatisticViewModel : BaseViewModel
    {  //chart
        private List<int> _datacombobox;
        private ObservableCollection<LineChartModel> dataLineChart;
        private Visibility checkSelected;

        public Visibility CheckSelected
        {
            get { return checkSelected; }
            set { checkSelected = value; OnPropertyChanged(nameof(checkSelected)); }
        }
        public List<int> DataCombobox
        {
            get
            {
                return _datacombobox;
            }
            set
            {
                _datacombobox = value;
            }
        }
        public ObservableCollection<LineChartModel> DataLineChart
        {
            get { return dataLineChart; }
            set { dataLineChart = value; OnPropertyChanged(nameof(dataLineChart)); }
        }
        //end chart

        private int selectedMonth;
        private Visibility checkData;

        private ObservableCollection<ViewStatistic> dataListView;
        private string totalSalary;
        private string totalFee;

        public string TotalFee
        {
            get { return totalFee; }
            set { totalFee = value; OnPropertyChanged(nameof(totalFee)); }
        }

        public string TotalSalary
        {
            get { return totalSalary; }
            set { totalSalary = value; OnPropertyChanged(nameof(totalSalary)); }
        }


        public ObservableCollection<ViewStatistic> DataListView
        {
            get { return dataListView; }
            set { dataListView = value; OnPropertyChanged(nameof(dataListView)); }
        }

        public Visibility CheckData
        {
            get { return checkData; }
            set { checkData = value; OnPropertyChanged(nameof(checkData)); }
        }
        private int selectedYear;

        public int SelectedYear
        {
            get { return selectedYear; }
            set { selectedYear = value; LoadDataChart(); }
        }
        public int SelectedMonth
        {
            get { return selectedMonth; }
            set { selectedMonth = value; }
        }

        private List<string> dataComboboxYear;
        public List<string> DataComboboxMonth { get; set; }
        public List<string> DataComboboxYear
        {
            get { return dataComboboxYear; }
            set { dataComboboxYear = value; OnPropertyChanged(nameof(dataComboboxYear)); }
        }
        public ICommand LoadDataLv { get; set; }
        public ICommand ExportFileExcel { get; set; }
        public StatisticViewModel()
        {
            //chart
            CheckSelected = Visibility.Hidden;
            DataLineChart = new ObservableCollection<LineChartModel>();
            // end chart
            TotalFee = "";
            checkData = Visibility.Visible;
            DataListView = new ObservableCollection<ViewStatistic>();
            LoadDataLv = new RelayCommand<object>((p) => { return true; }, (p) => { LoadDataListView(p); });
            ExportFileExcel = new RelayCommand<DataGrid>((p) => { return true; }, (p) => { exportFileExcel(p); });
            LoadDataComboBox();
        }
        //chart
        void LoadDataChart()
        {
            CheckSelected = Visibility.Visible;
            DataLineChart.Clear();
            for (int i = 0; i < 12; i++)
            {
                //LineVal.Add(Calculate(i + 1));
                LineChartModel chartModel = new LineChartModel(Calculate(i + 1), i + 1);
                DataLineChart.Add(chartModel);
            }
        }
        decimal Calculate(int month)
        {
            decimal tong = 0;
            List<HoaDon> list = DataProvider.Ins.DB.HoaDons
                .Where(p => p.ngay_xuat_hoa_don.Value.Year == SelectedYear && p.ngay_xuat_hoa_don.Value.Month == month).ToList();
            PhieuTinhLuong luong = DataProvider.Ins.DB.PhieuTinhLuongs.
                Where(p => p.ngay_tinh_luong.Value.Year == SelectedYear && p.ngay_tinh_luong.Value.Month == month).FirstOrDefault();
            List<PhieuNhapHang> phieuNhapHang = DataProvider.Ins.DB.PhieuNhapHangs.
                Where(p => p.ngay_nhap.Value.Year == SelectedYear && p.ngay_nhap.Value.Month == month).ToList();
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
        //end chart
        void exportFileExcel(DataGrid dtGrid)
        {
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Visible = true; //www.yazilimkodlama.com
            Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Worksheet sheet1 = (Worksheet)workbook.Sheets[1];
            int col = dtGrid.Columns.Count / 2;
            for (int j = 0; j < col; j++) //Başlıklar için
            {
                Range myRange = (Range)sheet1.Cells[1, j + 1];
                sheet1.Cells[1, j + 1].Font.Bold = true; //Başlığın Kalın olması için
                sheet1.Columns[j + 1].ColumnWidth = 15; //Sütun genişliği ayarı
                myRange.Value2 = dtGrid.Columns[j].Header;
            }
            for (int i = 0; i < col; i++)
            { //www.yazilimkodlama.com
                for (int j = 0; j < dtGrid.Items.Count; j++)
                {
                    TextBlock b = dtGrid.Columns[i].GetCellContent(dtGrid.Items[j]) as TextBlock;
                    if (b != null)
                    {
                       
                        Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 2, i + 1];
                       
                        myRange.Value2 = b.Text;
                    }
                }
            }
        }
        void LoadDataListView(object f)
        {
            DataListView.Clear();
            decimal total = 0;
            ObservableCollection<ViewStatistic> b = new ObservableCollection<ViewStatistic>();
            if (SelectedYear != 0)
            {
                int days = DateTime.DaysInMonth(SelectedYear, SelectedMonth + 1);
                for (int i = 1; i <= days; i++)
                {
                    decimal totalHD = 0, totalPNH = 0;
                    DateTime day = new DateTime(SelectedYear, SelectedMonth + 1, i);
                    ViewStatistic view = new ViewStatistic();
                    ViewEmployee t = new ViewEmployee();
                    List<HoaDon> hd = DataProvider.Ins.DB.HoaDons.Where(p => p.ngay_xuat_hoa_don == day).ToList();
                    List<PhieuNhapHang> a = DataProvider.Ins.DB.PhieuNhapHangs.Where(p => p.ngay_nhap == day).ToList();
                    if (hd.Count == 0 && a.Count == 0)
                    {
                        continue;
                    }
                    for (int j = 0; j < hd.Count; j++)
                    {
                        totalHD += (decimal)hd[j].tong_tien;
                    }
                    for (int j = 0; j < a.Count; j++)
                    {
                        totalPNH += (decimal)a[j].tong_tien;
                    }
                    view.tien_hoa_don = MoneyConverter.convertMoney(totalHD.ToString());
                    view.tien_nguon_hang = MoneyConverter.convertMoney(totalPNH.ToString());
                    total = total + (decimal)(totalHD + totalPNH);
                    view.tong = MoneyConverter.convertMoney((totalHD + totalPNH).ToString());
                    view.thoi_gian = String.Format("{0:dd/MM/yyyy}", day);
                    b.Add(view);
                }
            }
            if (b.Count != 0)
            {
                TotalFee = MoneyConverter.convertMoney(total.ToString());
                DataListView = b;
                PhieuTinhLuong pSalary = DataProvider.Ins.DB.PhieuTinhLuongs
                    .Where(p => p.ngay_tinh_luong.Value.Month == SelectedMonth + 1 &&
                    p.ngay_tinh_luong.Value.Year == SelectedYear).FirstOrDefault();
                if(pSalary == null)
                {
                    TotalSalary = "0";
                }
                else
                {
                    TotalSalary = pSalary.tong_tien.ToString();
                }
                TotalSalary = "Tổng tiền lương trả cho nhân viên trong tháng " + MoneyConverter.convertMoney(TotalSalary);
            }
            else
            {
                if(TotalSalary == null)
                {
                    CheckData = Visibility.Visible;
                    return;
                }
                TotalSalary = "Tổng tiền lương trả cho nhân viên trong tháng " + MoneyConverter.convertMoney(TotalSalary);
            }
            CheckData = Visibility.Hidden;
        }
        void LoadDataComboBox()
        {
            DataComboboxYear = new List<string>();
            DataComboboxMonth = new List<string>() {
                "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6",
                 "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12",
            };
            List<PhieuTinhLuong> checkExist = DataProvider.Ins.DB.PhieuTinhLuongs.ToList();
            if (checkExist.Count != 0)  
            {
                int min = DataProvider.Ins.DB.PhieuTinhLuongs.Min(t => t.ngay_tinh_luong.Value.Year);
                for (int i = min; i <= DateTime.Now.Year; i++)
                {
                    DataComboboxYear.Add(i.ToString());
                }
            }
            return;
        }
    }
}
