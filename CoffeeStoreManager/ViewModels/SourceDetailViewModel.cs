
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CoffeeStoreManager.Models;

namespace CoffeeStoreManager.ViewModels
{
    public class SourceDetailViewModel : BaseViewModel
    {
        private ObservableCollection<CT_PhieuNhapHang> detail;
        public ObservableCollection<CT_PhieuNhapHang> Detail { get => detail; set { detail = value; OnPropertyChanged(nameof(Detail)); } }
        private CT_PhieuNhapHang selectedDetailItem;
        public CT_PhieuNhapHang SelectedDetailItem { get => selectedDetailItem; set => selectedDetailItem = value; }

        private int selectedSourceItemIndex;
        public ICommand UpdateDetail { get; set; }
        public ICommand AddDetail { get; set; }
        public ICommand DeleteDetail { get; set; }
        public ICommand Search { get; set; }
        public ICommand RefreshDetail { get; set; }

        private string sourceName;
        private int sourcePrice;
        private int sourceCount;
        public string SourceName { get => sourceName; set { sourceName = value; OnPropertyChanged(nameof(SourceName)); } }
        public int SourcePrice { get => sourcePrice; set { sourcePrice = value; OnPropertyChanged(nameof(SourcePrice)); } }
        public int SourceCount { get => sourceCount; set { sourceCount = value; OnPropertyChanged(nameof(SourceCount)); } }
        public string SearchKey { get; set; }

        //add detail form field binding 

        public SourceDetailViewModel(int sourceItemIndex)
        {
            this.selectedSourceItemIndex = sourceItemIndex;
            loadDetail();

            UpdateDetail = new RelayCommand<object>((p) => { return true; }, (p) => { updateDetail(p); });
            AddDetail = new RelayCommand<object>((p) => { return true; }, (p) => { addDetail(p); });
            DeleteDetail = new RelayCommand<object>((p) => { return true; }, (p) => { deleteDetail(p); });
            RefreshDetail = new RelayCommand<object>((p) => { return true; }, (p) => { loadDetail(); });
            Search = new RelayCommand<object>((p) => { return true; }, (p) => { search(p); });
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
        }

        private void addDetail(object p)
        {
            CT_PhieuNhapHang newDetail = new CT_PhieuNhapHang()
            {
                ma_phieu_nhap_hang = selectedSourceItemIndex,
                ten_mat_hang = SourceName,
                so_luong = SourceCount,
                gia_tien = SourcePrice,
                tong_tien = SourcePrice * sourceCount
            };
            DataProvider.Ins.DB.CT_PhieuNhapHang.Add(newDetail);
            DataProvider.Ins.DB.SaveChanges();

            Detail.Add(newDetail);
        }

        private void updateDetail(object p)
        {
            var dbSelectedDetailItem = DataProvider.Ins.DB.CT_PhieuNhapHang.SingleOrDefault(detail => detail.ma_ct_phieu_nhap_hang == SelectedDetailItem.ma_ct_phieu_nhap_hang);
            dbSelectedDetailItem.ten_mat_hang = selectedDetailItem.ten_mat_hang;
            dbSelectedDetailItem.so_luong = selectedDetailItem.so_luong;
            dbSelectedDetailItem.gia_tien = selectedDetailItem.gia_tien;
            dbSelectedDetailItem.tong_tien = selectedDetailItem.gia_tien * selectedDetailItem.so_luong;
            DataProvider.Ins.DB.SaveChanges();

        }

    }
}
