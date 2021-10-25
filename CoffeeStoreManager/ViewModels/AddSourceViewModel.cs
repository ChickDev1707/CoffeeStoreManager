using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
namespace CoffeeStoreManager.ViewModels
{
    public class AddSourceViewModel: BaseViewModel
    {
        private ObservableCollection<SourceItemControlDataTemplate> sourceItemControlList;

        public ObservableCollection<SourceItemControlDataTemplate> SourceItemControlList { get => sourceItemControlList; set { sourceItemControlList = value; OnPropertyChanged(nameof(SourceItemControlList)); } }

        public ICommand AddItem { get; set; }
        public ICommand DeleteItem { get; set; }
        private int currentIndex = 1;

        public AddSourceViewModel()
        {
            SourceItemControlList = new ObservableCollection<SourceItemControlDataTemplate>();
            SourceItemControlList.Add(new SourceItemControlDataTemplate() { ItemIndex=0, Name="", Price="", Count=0, TotalMoney=0, DelItem=DeleteItem });


            AddItem = new RelayCommand<object>((p) => { return true; }, (p) => { addItem(p); });
            DeleteItem = new RelayCommand<object>((p) => { return true; }, (p) => { deleteItem(p); });

        }

        private void deleteItem(object index)
        {
            int selectedItemIndex = Convert.ToInt32(index);
            SourceItemControlList.Remove(SourceItemControlList.Where(item => item.ItemIndex == selectedItemIndex).Single());
        }

        private void addItem(object p)
        {
            SourceItemControlList.Add(new SourceItemControlDataTemplate() { ItemIndex = currentIndex, Name = "", Price = "", Count = 0, TotalMoney = 0, DelItem = DeleteItem });
            currentIndex++;
        }

    }
    public class SourceItemControlDataTemplate
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public int Count { get; set; }
        public int TotalMoney { get; set; }
        public int ItemIndex { get; set; }

        public ICommand DelItem { get; set; }
    }
    
}
