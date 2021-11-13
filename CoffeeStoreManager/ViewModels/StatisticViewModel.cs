using CoffeeStoreManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CoffeeStoreManager.Views.Statistic;
namespace CoffeeStoreManager.ViewModels
{
    public class StatisticViewModel : BaseViewModel
    {
        private ObservableCollection<LineChartModel> dataListView;
        private List<HoaDon> listHoaDon;
        private int selectedMonth;

        private int selectedYear;

        public int SelectedYear
        {
            get { return selectedYear; }
            set { selectedYear = value; LoadDataListView(); }
        }
        public int SelectedMonth
        {
            get { return selectedMonth; }
            set { selectedMonth = value; LoadDataListView(); }
        }

        private List<string> dataComboboxYear;
        public List<string> DataComboboxMonth { get; set; }
        public List<string> DataComboboxYear
        {
            get { return dataComboboxYear; }
            set { dataComboboxYear = value; OnPropertyChanged(nameof(dataComboboxYear));}
        }
        public List<HoaDon> ListHoaDon
        {
            get { return listHoaDon; }
            set { listHoaDon = value; OnPropertyChanged(nameof(listHoaDon)); }
        }

        public ICommand OpenStatisticChart { get; set; }
        public ObservableCollection<LineChartModel> DataListView
        {
            get { return dataListView; }
            set { dataListView = value; OnPropertyChanged(nameof(dataListView)); }
        }
        public StatisticViewModel()
        {
            DataListView = new ObservableCollection<LineChartModel>();
            ListHoaDon = new List<HoaDon>();
            OpenStatisticChart = new  RelayCommand<object>((p) => { return true; }, (p) => { openChart(p); });
          
            LoadDataComboBox();
            LoadDataListView();
        }
        void LoadDataListView()
        {
            LoadListHoaDon();
        }
        void LoadListHoaDon()
        {
            ListHoaDon.Clear();
            ListHoaDon = DataProvider.Ins.DB.HoaDons.
                Where(t => t.ngay_xuat_hoa_don.Value.Year == SelectedYear + 2021 &&
                           t.ngay_xuat_hoa_don.Value.Month == SelectedMonth + 1).
                ToList();
        }
        void LoadDataComboBox()
        {
            DataComboboxYear = new List<string>();
            DataComboboxMonth = new List<string>() { 
                "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6",
                 "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12",
            };
            if(DataComboboxYear.Count <= 0)
            {
                DataComboboxYear.Add(DateTime.Now.Year.ToString());
            }
            else if(DataComboboxYear[DataComboboxYear.Count -1] != DateTime.Now.Year.ToString())
            {
                DataComboboxYear.Add(DateTime.Now.Year.ToString());
            }
            return;

        }
     
        void openChart(object p )
        {
            var window = new StatisticChartWindow();
            window.ShowDialog();
        }

    }
}