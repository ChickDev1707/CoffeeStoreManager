using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using CoffeeStoreManager.Models;
using CoffeeStoreManager.Resources.Utils;

namespace CoffeeStoreManager.ViewModels
{
    public class UpdateSourceViewModel:BaseViewModel
    {
        public string Provider { get => provider; set { provider = value; OnPropertyChanged(nameof(Provider)); } }
        private string provider;
        public DateTime? ImportDate { get => importDate; set { importDate = value; OnPropertyChanged(nameof(ImportDate)); } }
        private DateTime? importDate;
        public ICommand UpdateSource { get; set; }

        private SourceViewModel sourceVM;
        public UpdateSourceViewModel(SourceViewModel sourceViewModel)
        {
            this.sourceVM = sourceViewModel;
            UpdateSource = new RelayCommand<StackPanel>((p) => { return true; }, (p) => { updateSource(p); });
            loadSelectedSourceItem();
        }
        private void loadSelectedSourceItem()
        {
            Provider = this.sourceVM.SelectedSourceItem.nha_cung_cap;
            ImportDate = this.sourceVM.SelectedSourceItem.ngay_nhap;
        }
        private void updateSource(StackPanel updateSourceForm)
        {
            if (Validator.IsValid(updateSourceForm))
            {
                var selectedSourceItem = this.sourceVM.SelectedSourceItem;
                var dbSelectedSourceItem = DataProvider.Ins.DB.PhieuNhapHangs.SingleOrDefault(source => source.ma_phieu_nhap_hang == selectedSourceItem.ma_phieu_nhap_hang);
                dbSelectedSourceItem.nha_cung_cap = selectedSourceItem.nha_cung_cap;
                dbSelectedSourceItem.ngay_nhap = selectedSourceItem.ngay_nhap;
                DataProvider.Ins.DB.SaveChanges();

                this.sourceVM.LoadSourceList();
                this.sourceVM.MyMessageQueue.Enqueue("Cập nhật thông tin phiếu nhập hàng thành công!");
            }
            else
            {
                this.sourceVM.MyMessageQueue.Enqueue("Lỗi. Thông tin cập nhật không hợp lệ!");
            }
        }
    }
}
