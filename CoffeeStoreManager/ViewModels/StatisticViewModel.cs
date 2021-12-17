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
    {  
        private List<int> _datacombobox;
        private int selectedYear;
        private ObservableCollection<LineChartModel> dataLineChart;
        private ObservableCollection<LineChartModel> dataLineChartIncome;
        private ObservableCollection<ViewStatistic> dataListView;
        private ObservableCollection<ViewStatistic> dataGridView;

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
        public ObservableCollection<LineChartModel> DataLineChartIncome
        {
            get { return dataLineChartIncome; }
            set { dataLineChartIncome = value; OnPropertyChanged(nameof(dataLineChartIncome)); }
        }
        



        public ObservableCollection<ViewStatistic> DataListView
        {
            get { return dataListView; }
            set { dataListView = value; OnPropertyChanged(nameof(dataListView)); }
        }
        public ObservableCollection<ViewStatistic> DataGridView
        {
            get { return dataGridView; }
            set { dataGridView = value; OnPropertyChanged(nameof(dataGridView)); }
        }


        public int SelectedYear
        {
            get { return selectedYear; }
            set { selectedYear = value; LoadDataChart(); }
        }
        private List<string> dataComboboxYear;
        public List<string> DataComboboxYear
        {
            get { return dataComboboxYear; }
            set { dataComboboxYear = value; OnPropertyChanged(nameof(dataComboboxYear)); }
        }
        public ICommand LoadDataLv { get; set; }
        public ICommand ExportExcel { get; set; }
        public StatisticViewModel()
        {
            //chart
            CheckSelected = Visibility.Hidden;
            DataLineChart = new ObservableCollection<LineChartModel>();
            DataLineChartIncome = new ObservableCollection<LineChartModel>();
            // end chart
            DataListView = new ObservableCollection<ViewStatistic>();
            DataGridView = new ObservableCollection<ViewStatistic>();
            LoadDataLv = new RelayCommand<object>((p) => { return true; }, (p) => { LoadDataListView(p); });
            ExportExcel = new RelayCommand<DataGrid>((p) => { return true; }, (p) => { ExportFileExcel(p); });
            LoadDataComboBox();
        }
        //chart
        void LoadDataChart()
        {
            CheckSelected = Visibility.Visible;
            DataLineChart.Clear();
            DataLineChartIncome.Clear();
            for (int i = 1; i <= 12; i++)
            {
                LineChartModel IncomeChartModel = new LineChartModel(CalculateInComeChart(i), i);
                LineChartModel ChartModel = new LineChartModel(Calculate(i), i);
                DataLineChartIncome.Add(IncomeChartModel);
                DataLineChart.Add(ChartModel);
            }
        }
        decimal CalculateInComeChart(int month)
        {
            decimal tong = 0;
            List<HoaDon> list = DataProvider.Ins.DB.HoaDons
                .Where(p => p.ngay_xuat_hoa_don.Value.Year == SelectedYear && p.ngay_xuat_hoa_don.Value.Month == month).ToList();
                for (int i = 0; i <list.Count; i++)
                {
                    tong = tong + (decimal)list[i].tong_tien;
                }
            tong = tong / 1000;
            return tong;
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
            for (int i = 0; i < phieuNhapHang.Count; i++)
            {
                tong = tong - (decimal)phieuNhapHang[i].tong_tien;
            }
            for (int i = 0; i < list.Count; i++)
            {
                tong = tong + (decimal)list[i].tong_tien;
            }
            if (luong != null)
            {
                tong = tong - (decimal)luong.tong_tien;
            }
            return tong;
        }
        //end chart
        void ExportFileExcel(DataGrid dtGrid)
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
            DataGridView.Clear();
            if (SelectedYear != 0)
            {
                decimal profit = 0;
                for (int i = 1; i <= 12; i++)
                {
                    decimal totalHD = 0, totalPNH = 0, salary = 0;
                    ViewStatistic view = new ViewStatistic();
                    List<HoaDon> hd = DataProvider.Ins.DB.HoaDons.
                        Where(p => p.ngay_xuat_hoa_don.Value.Year == SelectedYear && p.ngay_xuat_hoa_don.Value.Month ==i).ToList();
                    List<PhieuNhapHang> a = DataProvider.Ins.DB.PhieuNhapHangs.
                        Where(p => p.ngay_nhap.Value.Year == SelectedYear && p.ngay_nhap.Value.Month == i).ToList();
                    PhieuTinhLuong pSalary = DataProvider.Ins.DB.PhieuTinhLuongs
                        .Where(p => p.ngay_tinh_luong.Value.Year == SelectedYear && p.ngay_tinh_luong.Value.Month == i).FirstOrDefault();
                    if(pSalary !=null)
                    {
                        salary = (decimal)pSalary.tong_tien;
                    }
                    if (pSalary == null)
                    {
                        view.tien_luong = "0";
                    }
                    else
                    { 
                        view.tien_luong = MoneyConverter.convertMoney(pSalary.tong_tien.ToString());
                    }
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
                    profit = (decimal)(totalHD - salary - totalPNH);
                    if (profit == 0)
                    {
                        view.tong = "0";
                    }
                    else view.tong = MoneyConverter.convertMoney(profit.ToString());
                    view.thoi_gian = String.Format("Tháng {0}", i);
                    DataListView.Add(view);
                    DataGridView.Add(view);
                }
            }
        }
        void LoadDataComboBox()
        {
            DataComboboxYear = new List<string>();
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
