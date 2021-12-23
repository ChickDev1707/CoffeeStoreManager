using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using CoffeeStoreManager.Models;
using CoffeeStoreManager.Resources.Utils;

namespace CoffeeStoreManager.ViewModels
{
    public class AddSourceViewModel: BaseViewModel
    {
        private ObservableCollection<SourceItemControlDataTemplate> sourceItemControlList;

        public ObservableCollection<SourceItemControlDataTemplate> SourceItemControlList { get => sourceItemControlList; set { sourceItemControlList = value; OnPropertyChanged(nameof(SourceItemControlList)); } }

        private int currentIndex = 1;

        public string Provider { get => provider; set { provider = value; OnPropertyChanged(nameof(Provider)); } }
        private string provider;

        public DateTime ImportDate { get => importDate; set { importDate = value; OnPropertyChanged(nameof(ImportDate)); } }
        private DateTime importDate;
        public ICommand AddItem { get; set; }
        public ICommand SaveSource { get; set; }
        public ICommand DeleteItem { get; set; }

        private SourceViewModel sourceVM;
        public AddSourceViewModel(SourceViewModel sourceViewModel)
        {
            this.sourceVM = sourceViewModel;
            ImportDate = DateTime.Now;

            AddItem = new RelayCommand<object>((p) => { return true; }, (p) => { addItem(p); });
            DeleteItem = new RelayCommand<object>((p) => { return true; }, (p) => { deleteItem(p); });
            SaveSource = new RelayCommand<StackPanel>((p) => { return true; }, (p) => { saveSource(p); });

            SourceItemControlList = new ObservableCollection<SourceItemControlDataTemplate>();
            SourceItemControlList.Add(new SourceItemControlDataTemplate() { ItemIndex = 0, Name = "", Price = 0, Count = 1, DelItem = DeleteItem });
        }

        private void saveSource(StackPanel itemsContainer)
        {
            if (Validator.IsValid(itemsContainer))
            {
                List<CT_PhieuNhapHang> sourceDetailList = createSourceDetailList();

                var sourceCard = new PhieuNhapHang()
                {
                    nha_cung_cap = Provider,
                    ngay_nhap = ImportDate,
                    tong_tien = calculateSourceCardTotalMoney(sourceDetailList),
                    CT_PhieuNhapHang = sourceDetailList
                };
                DataProvider.Ins.DB.PhieuNhapHangs.Add(sourceCard);
                DataProvider.Ins.DB.SaveChanges();
                this.sourceVM.LoadSourceList();
                this.sourceVM.MyMessageQueue.Enqueue("Thêm phiếu nhập hàng thành công!");
            }
            else
            {
                this.sourceVM.MyMessageQueue.Enqueue("Lỗi. Thông tin hàng hóa không hợp lệ.");
            }
        }
        private decimal? calculateSourceCardTotalMoney(List<CT_PhieuNhapHang> details)
        {
            decimal? total = 0;
            foreach(CT_PhieuNhapHang item in details)
            {
                total += item.tong_tien;
            }
            return total;
        }
        private List<CT_PhieuNhapHang> createSourceDetailList()
        {
            List<CT_PhieuNhapHang> sourceDetailList = new List<CT_PhieuNhapHang>();
            foreach(var item in SourceItemControlList)
            {
                var detail = new CT_PhieuNhapHang()
                {
                    ten_mat_hang = item.Name,
                    gia_tien = item.Price,
                    so_luong = item.Count,
                    tong_tien = item.Price * item.Count
                };
                sourceDetailList.Add(detail);
            }
            return sourceDetailList;
        }
        private void deleteItem(object index)
        {
            int selectedItemIndex = Convert.ToInt32(index);
            SourceItemControlList.Remove(SourceItemControlList.Where(item => item.ItemIndex == selectedItemIndex).Single());
        }

        private void addItem(object p)
        {
            SourceItemControlList.Add(new SourceItemControlDataTemplate() { ItemIndex = currentIndex, Name = "", Price = 0, Count = 1, DelItem = DeleteItem });
            currentIndex++;
        }

    }
    public class SourceItemControlDataTemplate
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public int ItemIndex { get; set; }
        public ICommand DelItem { get; set; }
    }
    
}
