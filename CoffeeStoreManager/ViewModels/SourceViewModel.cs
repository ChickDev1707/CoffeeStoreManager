using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CoffeeStoreManager.Models;
using CoffeeStoreManager.Views.MangeSource.Item;

namespace CoffeeStoreManager.ViewModels
{
    class SourceViewModel: BaseViewModel
    {
        public ObservableCollection<ViewSource> SourceList { get => sourceList; set { sourceList = value; OnPropertyChanged(nameof(SourceList)); } }

        private ObservableCollection<ViewSource> sourceList;

        private ViewSource selectedSourceItem;
        

        public ICommand OpenAddSource { get; set; }
        public ICommand OpenUpdateSource { get; set; }
        public ICommand UpdateSource { get; set; }

        public ICommand DeleteSource { get; set; }
        public ICommand RefreshSourceList { get; set; }
        public ICommand Search { get; set; }

        public ViewSource SelectedSourceItem { get => selectedSourceItem; set { selectedSourceItem = value; OnPropertyChanged(nameof(SelectedSourceItem)); } }
        public string SearchKey { get; set; }
        public SourceViewModel()
        {
            OpenAddSource = new RelayCommand<object>((p) => { return true; }, (p) => { openAddSource(p); });
            OpenUpdateSource = new RelayCommand<object>((p) => { return true; }, (p) => { openUpdateSource(p); });

            UpdateSource = new RelayCommand<object>((p) => { return true; }, (p) => { updateSource(p); });
            DeleteSource = new RelayCommand<object>((p) => { return true; }, (p) => { deleteSource(p); });
            RefreshSourceList = new RelayCommand<object>((p) => { return true; }, (p) => { loadSourceList(); });
            Search = new RelayCommand<object>((p) => { return true; }, (p) => { search(p); });

            loadSourceList();
        }

        private void search(object p)
        {
            var searchedSourceList=  SourceList.Where<ViewSource>(source => source.nha_cung_cap.ToLower().Contains(SearchKey.ToLower())).ToList();
            SourceList = new ObservableCollection<ViewSource>(searchedSourceList);
        }

        private void deleteSource(object p)
        {
            var sqlDeleteSourceString = String.Format("delete from PhieuNhapHang where ma_phieu_nhap_hang = {0}", SelectedSourceItem.ma_phieu_nhap_hang.ToString());
            var sqlDeleteSourceDetailString = String.Format("delete from CT_PhieuNhapHang where ma_phieu_nhap_hang = {0}", SelectedSourceItem.ma_phieu_nhap_hang.ToString());
            DataProvider.Ins.DB.Database.ExecuteSqlCommand(sqlDeleteSourceDetailString);
            DataProvider.Ins.DB.Database.ExecuteSqlCommand(sqlDeleteSourceString);

            loadSourceList();
        }


        private void updateSource(object p)
        {
            var dbSelectedSourceItem = DataProvider.Ins.DB.PhieuNhapHangs.SingleOrDefault(source => source.ma_phieu_nhap_hang == SelectedSourceItem.ma_phieu_nhap_hang);
            dbSelectedSourceItem.nha_cung_cap = selectedSourceItem.nha_cung_cap;
            dbSelectedSourceItem.ngay_nhap = selectedSourceItem.ngay_nhap;
            DataProvider.Ins.DB.SaveChanges();
        }

        private void openUpdateSource(object p)
        {
            UpdateSourceWindow updateWindow = new UpdateSourceWindow();
            updateWindow.ShowDialog();
        }

        private void openAddSource(object p)
        {
            AddSourceWindow addSourceWindow = new AddSourceWindow();
            addSourceWindow.ShowDialog();
        }
        void loadSourceList()
        {
            var db = DataProvider.Ins.DB;
            var sqlString = "select p.ma_phieu_nhap_hang, nha_cung_cap, ngay_nhap, count(*) as so_luong_mat_hang, sum(ct.tong_tien) as tong_tien " +
                            "from PhieuNhapHang as p, CT_PhieuNhapHang as ct " +
                            "where p.ma_phieu_nhap_hang = ct.ma_phieu_nhap_hang " +
                            "group by p.ma_phieu_nhap_hang, nha_cung_cap, ngay_nhap";

            var dataList = db.Database.SqlQuery<ViewSource>(sqlString).ToList();
            SourceList = new ObservableCollection<ViewSource>(dataList);
        }
       
    }
}
