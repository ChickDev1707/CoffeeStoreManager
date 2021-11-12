using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CoffeeStoreManager.Models;

namespace CoffeeStoreManager.ViewModels
{
    public class AddSourceViewModel: BaseViewModel
    {
        private ObservableCollection<SourceItemControlDataTemplate> sourceItemControlList;

        public ObservableCollection<SourceItemControlDataTemplate> SourceItemControlList { get => sourceItemControlList; set { sourceItemControlList = value; OnPropertyChanged(nameof(SourceItemControlList)); } }

        private string provider;
        private DateTime importDate;
        public ICommand AddItem { get; set; }
        public ICommand DeleteItem { get; set; }
        private int currentIndex = 1;

        public ICommand SaveSource { get; set; }
        public string Provider { get => provider; set { provider = value; OnPropertyChanged(nameof(Provider)); } }

        public DateTime ImportDate { get => importDate; set { importDate = value; OnPropertyChanged(nameof(ImportDate)); } }

        public AddSourceViewModel()
        {
            AddItem = new RelayCommand<object>((p) => { return true; }, (p) => { addItem(p); });
            DeleteItem = new RelayCommand<object>((p) => { return true; }, (p) => { deleteItem(p); });
            SaveSource = new RelayCommand<object>((p) => { return true; }, (p) => { saveSource(p); });

            SourceItemControlList = new ObservableCollection<SourceItemControlDataTemplate>();
            SourceItemControlList.Add(new SourceItemControlDataTemplate() { ItemIndex = 0, Name = "", Price = 0, Count = 0, DelItem = DeleteItem });
        }

        private void saveSource(object p)
        {
            List<CT_PhieuNhapHang> sourceDetailList = createSourceDetailList();

            var sourceCard = new PhieuNhapHang()
            {
                nha_cung_cap = Provider,
                ngay_nhap = ImportDate,
                CT_PhieuNhapHang = sourceDetailList
            };
            DataProvider.Ins.DB.PhieuNhapHangs.Add(sourceCard);
            DataProvider.Ins.DB.SaveChanges();
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
            SourceItemControlList.Add(new SourceItemControlDataTemplate() { ItemIndex = currentIndex, Name = "", Price = 0, Count = 0, DelItem = DeleteItem });
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
