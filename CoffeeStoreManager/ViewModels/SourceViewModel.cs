using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CoffeeStoreManager.Models;
using CoffeeStoreManager.Views.MangeSource.Item;
using MaterialDesignThemes.Wpf;

namespace CoffeeStoreManager.ViewModels
{
    public class SourceViewModel: BaseViewModel
    {
        public ObservableCollection<ViewSource> SourceList { get => sourceList; set { sourceList = value; OnPropertyChanged(nameof(SourceList)); } }
        private ObservableCollection<ViewSource> sourceList;
        public ViewSource SelectedSourceItem { get => selectedSourceItem; set { selectedSourceItem = value; OnPropertyChanged(nameof(SelectedSourceItem)); } }
        private ViewSource selectedSourceItem;
        public string SearchKey { get; set; }
        public SnackbarMessageQueue MyMessageQueue { get => myMessageQueue; set { myMessageQueue = value; OnPropertyChanged(nameof(MyMessageQueue)); } }
        private SnackbarMessageQueue myMessageQueue;

        public ICommand OpenAddSource { get; set; }
        public ICommand OpenUpdateSource { get; set; }
        public ICommand DeleteSource { get; set; }
        public ICommand RefreshSourceList { get; set; }
        public ICommand Search { get; set; }

        public SourceViewModel()
        {
            
            OpenAddSource = new RelayCommand<object>((p) => { return true; }, (p) => { openAddSource(); });
            OpenUpdateSource = new RelayCommand<object>((p) => { return true; }, (p) => { openUpdateSource(); });

            DeleteSource = new RelayCommand<object>((p) => { return true; }, (p) => { deleteSource(p); });
            RefreshSourceList = new RelayCommand<object>((p) => { return true; }, (p) => { LoadSourceList(); });
            Search = new RelayCommand<object>((p) => { return true; }, (p) => { search(p); });

            MyMessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(4000));
            MyMessageQueue.DiscardDuplicates = true;

            LoadSourceList();
        }

        private void search(object p)
        {
            var searchedSourceList=  SourceList.Where<ViewSource>(source => source.nha_cung_cap.ToLower().Contains(SearchKey.ToLower())).ToList();
            SourceList = new ObservableCollection<ViewSource>(searchedSourceList);
        }

        private void deleteSource(object p)
        {
            if(SelectedSourceItem != null)
            {
                var sqlDeleteSourceString = String.Format("delete from PhieuNhapHang where ma_phieu_nhap_hang = {0}", SelectedSourceItem.ma_phieu_nhap_hang.ToString());
                var sqlDeleteSourceDetailString = String.Format("delete from CT_PhieuNhapHang where ma_phieu_nhap_hang = {0}", SelectedSourceItem.ma_phieu_nhap_hang.ToString());
                DataProvider.Ins.DB.Database.ExecuteSqlCommand(sqlDeleteSourceDetailString);
                DataProvider.Ins.DB.Database.ExecuteSqlCommand(sqlDeleteSourceString);

                LoadSourceList();
                MyMessageQueue.Enqueue("Đã xóa thành công phiếu nhập hàng!");
            }
            else
            {
                MyMessageQueue.Enqueue("Bạn chưa chọn phiếu nhập hàng.");
            }
        }
        private void openUpdateSource()
        {
            if(SelectedSourceItem != null)
            {
                UpdateSourceWindow updateWindow = new UpdateSourceWindow(this);
                updateWindow.ShowDialog();
            }
            else
            {
                MyMessageQueue.Enqueue("Bạn chưa chọn phiếu nhập hàng.");
            }
        }

        private void openAddSource()
        {
            AddSourceWindow addSourceWindow = new AddSourceWindow(this);
            addSourceWindow.ShowDialog();
        }
        public void LoadSourceList()
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
