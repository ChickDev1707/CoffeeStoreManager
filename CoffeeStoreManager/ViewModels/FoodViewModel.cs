using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using CoffeeStoreManager.Models;
using CoffeeStoreManager.Views.ManageFood;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using MessageBox = System.Windows.Forms.MessageBox;

namespace CoffeeStoreManager.ViewModels
{
    public class FoodViewModel: BaseViewModel
    {
        public ObservableCollection<MonAn> FoodList { get => foodList; set { foodList = value; OnPropertyChanged(nameof(FoodList)); } }
        public MonAn SelectedFood { get => selectedFood; set { selectedFood = value; OnPropertyChanged(nameof(SelectedFood)); } }
        public string SearchKey { get => searchKey; set { searchKey = value; OnPropertyChanged(nameof(SearchKey)); } }
        
        private ObservableCollection<MonAn> foodList;
        private MonAn selectedFood;
        private string searchKey;
        
        public ICommand OpenAddFoodWindow { get; set; }
        public ICommand OpenUpdateFoodWindow { get; set; }
        public ICommand Search { get; set; }
        public ICommand DeleteFood { get; set; }
        public ICommand RefreshFoodList { get; set; }
        public ICommand ImportExcel { get; set; }
        public ICommand ExportExcel { get; set; }

        public FoodViewModel()
        {
            LoadFoodList();

            OpenAddFoodWindow = new RelayCommand<object>((p) => { return true; }, (p) => { openAddFoodWindow(p); });
            OpenUpdateFoodWindow = new RelayCommand<object>((p) => { return true; }, (p) => { openUpdateFoodWindow(p); });
            Search = new RelayCommand<object>((p) => { return true; }, (p) => { search(p); });
            DeleteFood = new RelayCommand<object>((p) => { return true; }, (p) => { deleteFood(p); });
            RefreshFoodList = new RelayCommand<object>((p) => { return true; }, (p) => { refreshFoodList(p); });
            ImportExcel = new RelayCommand<object>((p) => { return true; }, (p) => { importExcel(p); });
            ExportExcel = new RelayCommand<object>((p) => { return true; }, (p) => { exportExcel(p); });
        }

        private void exportExcel(object p)
        {
            string filePath = "";
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Excel | *.xlsx | Excel 2003 | *.xls";

            // Nếu mở file và chọn nơi lưu file thành công sẽ lưu đường dẫn lại dùng
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                filePath = dialog.FileName;
            }

            // nếu đường dẫn null hoặc rỗng thì báo không hợp lệ và return hàm
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Đường dẫn báo cáo không hợp lệ");
                return;
            }

            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    package.Workbook.Properties.Author = "Admin";
                    package.Workbook.Properties.Title = "Danh sách món ăn";
                    package.Workbook.Worksheets.Add("Sheet 1");

                    ExcelWorksheet workSheet = package.Workbook.Worksheets[0];
                    //add sheet
                    workSheet.Name = "Sheet 1";
                    workSheet.Cells.Style.Font.Size = 12;
                    workSheet.Cells.Style.Font.Name = "Calibri";
                    // Tạo danh sách các column header
                    string[] arrColumnHeader = {
                        "Tên món",
                        "Giá tiền",
                        "Loại món",
                        "Nguyên liệu",
                        "Mô tả"
                    };

                    var countColHeader = arrColumnHeader.Count();

                    int colIndex = 1;
                    int rowIndex = 2;

                    //tạo các header từ column header đã tạo từ bên trên
                    foreach (var item in arrColumnHeader)
                    {
                        var cell = workSheet.Cells[rowIndex, colIndex];

                        //gán giá trị
                        cell.Value = item;

                        colIndex++;
                    }

                    foreach (var item in FoodList)
                    {
                        colIndex = 1;
                        rowIndex++;

                        workSheet.Cells[rowIndex, colIndex++].Value = item.ten_mon_an;
                        workSheet.Cells[rowIndex, colIndex++].Value = item.gia_tien;
                        workSheet.Cells[rowIndex, colIndex++].Value = item.LoaiMonAn.ten_loai_mon_an;
                        workSheet.Cells[rowIndex, colIndex++].Value = item.nguyen_lieu;
                        workSheet.Cells[rowIndex, colIndex++].Value = item.mo_ta;
                    }

                    //Lưu file lại
                    Byte[] bin = package.GetAsByteArray();
                    File.WriteAllBytes(filePath, bin);
                }
                MessageBox.Show("Xuất excel thành công!");
            }
            catch (Exception EE)
            {
                MessageBox.Show("Có lỗi khi lưu file!");
            }
        }

        private void importExcel(object p)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = dialog.FileName;
                handleImportExcelFile(fileName);
            }
        }
        private void handleImportExcelFile(string fileName)
        {
            List<MonAn> excelFoodList = new List<MonAn>();
            try
            {
                var package = new ExcelPackage(new FileInfo(fileName));
                ExcelWorksheet workSheet = package.Workbook.Worksheets[0];

                for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                {
                    try
                    {
                        // biến j biểu thị cho một column trong file
                        int j = 1;
                        
                        MonAn newFood = new MonAn()
                        {
                            ten_mon_an = workSheet.Cells[i, j++].Value.ToString(),
                            gia_tien = Convert.ToDecimal(workSheet.Cells[i, j++].Value),
                            ma_loai_mon_an = Convert.ToInt32(workSheet.Cells[i, j++].Value),
                            nguyen_lieu = workSheet.Cells[i, j++].Value.ToString(),
                            mo_ta = workSheet.Cells[i, j++].Value.ToString()
                        };
                        // add UserInfo vào danh sách userList
                        DataProvider.Ins.DB.MonAns.Add(newFood);
                        DataProvider.Ins.DB.SaveChanges();
                        FoodList.Add(newFood);

                    }
                    catch (Exception error)
                    {
                        MessageBox.Show("error");
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("Đã xảy ra lỗi khi import file excel!");
            }
        }
        public void LoadFoodList()
        {
            var foodList = DataProvider.Ins.DB.MonAns.ToList();
            FoodList = new ObservableCollection<MonAn>(foodList);
        }

        void openAddFoodWindow(object p)
        {
            var window = new AddFoodWindow(this);
            window.ShowDialog();
        } 
        void openUpdateFoodWindow(object p)
        {
            if(SelectedFood != null)
            {
                var window = new UpdateFoodWindow(this);
                window.ShowDialog();
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn món ăn");
            }
        }
        private void refreshFoodList(object p)
        {
            LoadFoodList();
        }

        private void deleteFood(object p)
        {
            var selectedFoodId = SelectedFood.ma_mon_an;
            var selectedFood = DataProvider.Ins.DB.MonAns.Where(food => food.ma_mon_an == selectedFoodId).First();
            DataProvider.Ins.DB.MonAns.Remove(selectedFood);
            DataProvider.Ins.DB.SaveChanges();

            FoodList.Remove(FoodList.Where(item => item.ma_mon_an == selectedFoodId).Single());
        }

        private void search(object p)
        {
            var searchResult = DataProvider.Ins.DB.MonAns.Where(food => food.ten_mon_an.ToLower().Contains(SearchKey.ToLower())).ToList();
            FoodList = new ObservableCollection<MonAn>(searchResult);
        }

    }
}
