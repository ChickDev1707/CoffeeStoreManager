
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using CoffeeStoreManager.Models;
using CoffeeStoreManager.Resources.Utils;
using CoffeeStoreManager.Views.MangeSource.Detail;
using MaterialDesignThemes.Wpf;

namespace CoffeeStoreManager.ViewModels
{
    public class SourceDetailViewModel : BaseViewModel
    {
        public ObservableCollection<CT_PhieuNhapHang> Detail { get => detail; set { detail = value; OnPropertyChanged(nameof(Detail)); } }
        private ObservableCollection<CT_PhieuNhapHang> detail;
        public CT_PhieuNhapHang SelectedDetailItem { get => selectedDetailItem; set => selectedDetailItem = value; }
        private CT_PhieuNhapHang selectedDetailItem;
        public string AddSourceName { get => addSourceName; set { addSourceName = value; OnPropertyChanged(nameof(AddSourceName)); } }
        private string addSourceName;
        public decimal? AddSourcePrice { get => addSourcePrice; set { addSourcePrice = value; OnPropertyChanged(nameof(AddSourcePrice)); } }
        private decimal? addSourcePrice;
        public int? AddSourceCount { get => addSourceCount; set { addSourceCount = value; OnPropertyChanged(nameof(AddSourceCount)); } }
        private int? addSourceCount;

        public string UpdateSourceName { get => updateSourceName; set { updateSourceName = value; OnPropertyChanged(nameof(UpdateSourceName)); } }
        private string updateSourceName;
        public decimal? UpdateSourcePrice { get => updateSourcePrice; set { updateSourcePrice = value; OnPropertyChanged(nameof(UpdateSourcePrice)); } }
        private decimal? updateSourcePrice;
        public int? UpdateSourceCount { get => updateSourceCount; set { updateSourceCount = value; OnPropertyChanged(nameof(UpdateSourceCount)); } }
        private int? updateSourceCount;
        public string SearchKey { get; set; }

        public SnackbarMessageQueue MyMessageQueue { get => myMessageQueue; set { myMessageQueue = value; OnPropertyChanged(nameof(MyMessageQueue)); } }
        private SnackbarMessageQueue myMessageQueue;

        private int selectedSourceItemIndex;
        public ICommand UpdateDetail { get; set; }
        public ICommand OpenAddWindow { get; set; }
        public ICommand OpenUpdateWindow { get; set; }
        public ICommand AddDetail { get; set; }
        public ICommand DeleteDetail { get; set; }
        public ICommand Search { get; set; }
        public ICommand RefreshDetail { get; set; }

        public SourceDetailViewModel(int sourceItemIndex)
        {
            this.selectedSourceItemIndex = sourceItemIndex;
            loadDetail();
            
            OpenAddWindow = new RelayCommand<object>((p) => { return true; }, (p) => { openAddWindow(p); });
            OpenUpdateWindow = new RelayCommand<object>((p) => { return true; }, (p) => { openUpdateWindow(p); });
            UpdateDetail = new RelayCommand<StackPanel>((p) => { return true; }, (p) => { updateDetail(p); });
            AddDetail = new RelayCommand<StackPanel>((p) => { return true; }, (p) => { addDetail(p); });
            DeleteDetail = new RelayCommand<StackPanel>((p) => { return true; }, (p) => { deleteDetail(p); });
            RefreshDetail = new RelayCommand<object>((p) => { return true; }, (p) => { loadDetail(); });
            Search = new RelayCommand<object>((p) => { return true; }, (p) => { search(p); });

            MyMessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(4000));
            MyMessageQueue.DiscardDuplicates = true;
        }

        private void openUpdateWindow(object p)
        {
            loadSelectedDetail();
            UpdateDetailWindow updateWindow = new UpdateDetailWindow(this);
            updateWindow.ShowDialog();
        }

        private void openAddWindow(object p)
        {
            AddDetailWindow addWindow = new AddDetailWindow(this);
            addWindow.ShowDialog();
        }

        private void loadDetail()
        {
            var dbDetailList = DataProvider.Ins.DB.CT_PhieuNhapHang.Where(detail => detail.ma_phieu_nhap_hang == this.selectedSourceItemIndex).ToList();
            Detail = new ObservableCollection<CT_PhieuNhapHang>(dbDetailList);
        }
        private void search(object p)
        {
            var searchedDetailList = Detail.Where<CT_PhieuNhapHang>(detail => detail.ten_mat_hang.ToLower().Contains(SearchKey.ToLower())).ToList();
            Detail = new ObservableCollection<CT_PhieuNhapHang>(searchedDetailList);
        }
        private void deleteDetail(object p)
        {
            var dbSelectedDetailItem = DataProvider.Ins.DB.CT_PhieuNhapHang.SingleOrDefault(detail => detail.ma_ct_phieu_nhap_hang == SelectedDetailItem.ma_ct_phieu_nhap_hang);
            DataProvider.Ins.DB.CT_PhieuNhapHang.Remove(dbSelectedDetailItem);
            DataProvider.Ins.DB.SaveChanges();

            Detail.Remove(Detail.Where(detail => detail.ma_ct_phieu_nhap_hang == SelectedDetailItem.ma_ct_phieu_nhap_hang).Single());
            MyMessageQueue.Enqueue("Xóa chi tiết hàng thành công!");
        }

        private void addDetail(StackPanel addDetailForm)
        {
            if (Validator.IsValid(addDetailForm))
            {
                CT_PhieuNhapHang newDetail = new CT_PhieuNhapHang()
                {
                    ma_phieu_nhap_hang = selectedSourceItemIndex,
                    ten_mat_hang = AddSourceName,
                    so_luong = AddSourceCount,
                    gia_tien = AddSourcePrice,
                    tong_tien = AddSourcePrice * AddSourceCount
                };
                DataProvider.Ins.DB.CT_PhieuNhapHang.Add(newDetail);
                DataProvider.Ins.DB.SaveChanges();

                Detail.Add(newDetail);
                MyMessageQueue.Enqueue("Thêm chi tiết hàng thành công!");
            }
            else
            {
                MyMessageQueue.Enqueue("Lỗi. Thông tin chi tiết hàng không hợp lệ.");
            }
        }

        private void updateDetail(StackPanel updateDetailForm)
        {
            if (Validator.IsValid(updateDetailForm))
            {
                var dbSelectedDetailItem = DataProvider.Ins.DB.CT_PhieuNhapHang.SingleOrDefault(detail => detail.ma_ct_phieu_nhap_hang == SelectedDetailItem.ma_ct_phieu_nhap_hang);
                dbSelectedDetailItem.ten_mat_hang = UpdateSourceName;
                dbSelectedDetailItem.so_luong = UpdateSourceCount;
                dbSelectedDetailItem.gia_tien = UpdateSourcePrice;
                dbSelectedDetailItem.tong_tien = UpdateSourcePrice * UpdateSourceCount;
                DataProvider.Ins.DB.SaveChanges();
                loadDetail();
                MyMessageQueue.Enqueue("Cập nhật chi tiết hàng thành công!");
            }
            else
            {
                MyMessageQueue.Enqueue("Lỗi. Thông tin cập nhật chi tiết hàng không hợp lệ.");
            }
        }
        private void loadSelectedDetail()
        {
            UpdateSourceName = SelectedDetailItem.ten_mat_hang;
            UpdateSourcePrice = SelectedDetailItem.gia_tien;
            UpdateSourceCount = SelectedDetailItem.so_luong;
        }

    }
}
