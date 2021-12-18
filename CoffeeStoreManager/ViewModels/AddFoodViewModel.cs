
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using CoffeeStoreManager.Models;
using CoffeeStoreManager.Resources.Utils;
using Image = System.Drawing.Image;

namespace CoffeeStoreManager.ViewModels
{
    public class AddFoodViewModel : BaseViewModel
    {
        public ObservableCollection<LoaiMonAn> FoodTypeList { get => foodTypeList; set { foodTypeList = value; OnPropertyChanged(nameof(FoodTypeList)); } }

        public string FoodName { get => foodName; set { foodName = value; OnPropertyChanged(nameof(FoodName)); } }
        public int FoodPrice { get => foodPrice; set { foodPrice = value; OnPropertyChanged(nameof(FoodPrice)); } }
        public int FoodType { get => foodType; set { foodType = value; OnPropertyChanged(nameof(FoodType)); } }
        public string FoodIngredient { get => foodIngredient; set { foodIngredient = value; OnPropertyChanged(nameof(FoodIngredient)); } }
        public string FoodDescription { get => foodDescription; set { foodDescription = value; OnPropertyChanged(nameof(FoodDescription)); } }
        public string FoodImagePath { get => foodImagePath; set { foodImagePath = value; OnPropertyChanged(nameof(FoodImagePath)); } }

        private string foodName;
        private int foodPrice;
        private int foodType;
        private string foodIngredient;
        private string foodDescription;
        private string foodImagePath;
        private ObservableCollection<LoaiMonAn> foodTypeList;

        private FoodViewModel foodVm;
        public ICommand AddFood { get; set; }
        public ICommand UploadFoodImage { get; set; }
        public AddFoodViewModel(FoodViewModel vm)
        {
            this.foodVm = vm;

            loadFoodTypeList();

            AddFood = new RelayCommand<StackPanel>((p) => { return true; }, (p) => { addFood(p); });
            UploadFoodImage = new RelayCommand<object>((p) => { return true; }, (p) => { uploadFoodImage(p); });
        }
        void loadFoodTypeList()
        {
            var distTypeListData = DataProvider.Ins.DB.LoaiMonAns.ToList();
            FoodTypeList = new ObservableCollection<LoaiMonAn>(distTypeListData);

        }
        private void uploadFoodImage(object p)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Only(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FoodImagePath = dialog.FileName;
            }
        }
        void addFood(StackPanel addFoodForm)
        {
            if (Validator.IsValid(addFoodForm))
            {
                Image foodImg = Image.FromFile(FoodImagePath);

                MonAn newFood = new MonAn()
                {
                    ten_mon_an = FoodName,
                    ma_loai_mon_an = FoodType,
                    gia_tien = FoodPrice,
                    nguyen_lieu = FoodIngredient,
                    mo_ta = FoodDescription,
                    anh = ImageConverterUtil.ImageToByteArray(foodImg)
                };
                DataProvider.Ins.DB.MonAns.Add(newFood);
                DataProvider.Ins.DB.SaveChanges();
                //udpate view
                foodVm.FoodList.Add(newFood);

            }
            else
            {
                System.Windows.MessageBox.Show("error");
            }
        }
    }
    
}
