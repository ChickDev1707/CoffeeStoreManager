using CoffeeStoreManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStoreManager.ViewModels
{
    class StatisticChartViewModel : BaseViewModel
    {
        static bool check = false;
        private int valueofcbx;
        private List<int> _datacombobox;
        private ObservableCollection<LineChartModel> dataLineChart;
        public int ValueOfCbx
        {
            get { return valueofcbx; }
            set { valueofcbx = value; OnPropertyChanged(nameof(valueofcbx)); LoadDataChart(); }
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
                check = true;
            }
        }
        public ObservableCollection<LineChartModel> DataLineChart
        {
            get { return dataLineChart; }
            set { dataLineChart = value; OnPropertyChanged(nameof(dataLineChart)); }
        }
        public StatisticChartViewModel()
        {
            DataLineChart = new ObservableCollection<LineChartModel>();
            if (check == false)  // check xem combobox duoc khoi tao lan nao chua
            {
                DataCombobox = new List<int>();
            }
            AddDataCombobox();
        }
        void AddDataCombobox()
        {
            if (DataCombobox.Count == 0 || DataCombobox[DataCombobox.Count - 1] != DateTime.Now.Year) ;
            DataCombobox.Add(DateTime.Now.Year);
        }
        void LoadDataChart()
        {

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
                .Where(p => p.ngay_xuat_hoa_don.Value.Year == valueofcbx && p.ngay_xuat_hoa_don.Value.Month == month).ToList();
            Luong luong = DataProvider.Ins.DB.Luongs.
                Where(p => p.ngay_tinh_luong.Value.Year == valueofcbx && p.ngay_tinh_luong.Value.Month == month).FirstOrDefault();
            for (int i = 0; i < list.Count; i++)
            {
                tong = tong + (decimal)list[i].tong_tien;
            }
            if(luong != null)
            {
                tong = tong - (decimal)luong.tong_tien_luong;
            }
            return tong;
        }
    }
}
