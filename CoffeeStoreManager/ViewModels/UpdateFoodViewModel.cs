
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using CoffeeStoreManager.Models;
using CoffeeStoreManager.Resources.Utils;

namespace CoffeeStoreManager.ViewModels
{
    public class UpdateFoodViewModel:BaseViewModel
    {
        public ObservableCollection<LoaiMonAn> FoodTypeList { get => foodTypeList; set { foodTypeList = value; OnPropertyChanged(nameof(FoodTypeList)); } }
        public string FoodName { get => foodName; set { foodName = value; OnPropertyChanged(nameof(FoodName)); } }
        public decimal? FoodPrice { get => foodPrice; set { foodPrice = value; OnPropertyChanged(nameof(FoodPrice)); } }
        public int FoodType { get => foodType; set { foodType = value; OnPropertyChanged(nameof(FoodType)); } }
        public string FoodIngredient { get => foodIngredient; set { foodIngredient = value; OnPropertyChanged(nameof(FoodIngredient)); } }
        public string FoodDescription { get => foodDescription; set { foodDescription = value; OnPropertyChanged(nameof(FoodDescription)); } }
        public byte[] FoodImage { get => foodImage; set { foodImage = value; OnPropertyChanged(nameof(FoodImage)); } }

        private string foodName;
        private decimal? foodPrice;
        private int foodType;
        private string foodIngredient;
        private string foodDescription;
        private byte[] foodImage;
        private ObservableCollection<LoaiMonAn> foodTypeList;

        private FoodViewModel foodVm;
        public ICommand UpdateSelectedFood { get; set; }
        public ICommand UpLoadFoodImage { get; set; }

        public UpdateFoodViewModel(FoodViewModel vm)
        {
            this.foodVm = vm;
            loadFoodTypeList();
            loadSelectedFood();

            UpdateSelectedFood = new RelayCommand<StackPanel>((p) => { return true; }, (p) => { updateSelectedFood(p); });
            UpLoadFoodImage = new RelayCommand<object>((p) => { return true; }, (p) => { upLoadFoodImage(p); });

        }
        private void upLoadFoodImage(object p)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Only(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FoodImage = ImageConverterUtil.ImageToByteArray(System.Drawing.Image.FromFile(dialog.FileName));
            }
        }
        private void loadSelectedFood()
        {
            FoodName = this.foodVm.SelectedFood.ten_mon_an;
            FoodPrice = this.foodVm.SelectedFood.gia_tien;
            FoodType = this.foodVm.SelectedFood.LoaiMonAn.ma_loai_mon_an;
            FoodIngredient = this.foodVm.SelectedFood.nguyen_lieu;
            FoodDescription = this.foodVm.SelectedFood.mo_ta;
            FoodImage = this.foodVm.SelectedFood.anh;
        }
        private void updateSelectedFood(StackPanel updateFoodForm)
        {
            if (Validator.IsValid(updateFoodForm))
            {
                var dbSelectedFood = DataProvider.Ins.DB.MonAns.SingleOrDefault(food => food.ma_mon_an == this.foodVm.SelectedFood.ma_mon_an);
                dbSelectedFood.ten_mon_an = FoodName;
                dbSelectedFood.ma_loai_mon_an = FoodType;
                dbSelectedFood.gia_tien = FoodPrice;
                dbSelectedFood.nguyen_lieu = FoodIngredient;
                dbSelectedFood.mo_ta = FoodDescription;
                dbSelectedFood.anh = FoodImage;
                DataProvider.Ins.DB.SaveChanges();

                this.foodVm.LoadFoodList();
                foodVm.MyMessageQueue.Enqueue("Cập nhật món ăn thành công!");
            }
            else
            {
                foodVm.MyMessageQueue.Enqueue("Lỗi. Thông tin món ăn không hợp lệ");
            }
        }
        void loadFoodTypeList()
        {
            var distTypeListData = DataProvider.Ins.DB.LoaiMonAns.ToList();
            FoodTypeList = new ObservableCollection<LoaiMonAn>(distTypeListData);
        }

    }
}
