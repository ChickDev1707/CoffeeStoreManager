using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using CoffeeStoreManager.Models;
using CoffeeStoreManager.Resources.Utils;
using MaterialDesignThemes.Wpf;
using OfficeOpenXml;

namespace CoffeeStoreManager.ViewModels
{
    public class MonthReportViewModel:BaseViewModel
    {
        public ObservableCollection<MonthReport> Report { get => report; set { report = value; OnPropertyChanged(nameof(report)); } }
        private ObservableCollection<MonthReport> report;

        public int SelectedMonth { get => month; set { month = value; OnPropertyChanged(nameof(SelectedMonth)); } }
        private int month;
        public int SelectedYear { get => year; set { year = value; OnPropertyChanged(nameof(SelectedYear)); } }
        private int year;
        public List<int> Months { get => months; set { months = value; OnPropertyChanged(nameof(Months)); } }
        private List<int> months;
        public ICommand RenderReport { get; set; }
        public ICommand ExportExcel { get; set; }

        public SnackbarMessageQueue MyMessageQueue { get => myMessageQueue; set { myMessageQueue = value; OnPropertyChanged(nameof(MyMessageQueue)); } }
        private SnackbarMessageQueue myMessageQueue;
        public MonthReportViewModel()
        {
            RenderReport = new RelayCommand<StackPanel>((p) => { return true; }, renderReport);
            ExportExcel = new RelayCommand<object>((p) => { return true; }, (p) => { exportExcel(p); });

            MyMessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(4000));
            MyMessageQueue.DiscardDuplicates = true;
            init();
        }
        private void init()
        {
            var now = DateTime.Now;
            SelectedMonth = now.Month;
            SelectedYear = now.Year;

            Months = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        }
        private void renderReport(StackPanel reportForm)
        {
            if (Validator.IsValid(reportForm))
            {
                var db = DataProvider.Ins.DB;
                var sqlStringFormat = "select ten_loai_mon_an, sum(so_luong) as tong_so_luong, sum(thanh_tien) as tong_so_tien "
                                + "from LoaiMonAn as fType, MonAn as f, HoaDon as bill, CT_HoaDon as dBill "
                                + "where fType.ma_loai_mon_an = f.ma_loai_mon_an and f.ma_mon_an = dBill.ma_mon_an and dBill.ma_hoa_don = bill.ma_hoa_don and month(ngay_xuat_hoa_don) = {0} and year(ngay_xuat_hoa_don) = {1} "
                                + "group by fType.ma_loai_mon_an, ten_loai_mon_an";
                var sqlString = String.Format(sqlStringFormat, SelectedMonth, SelectedYear);
                var dataList = db.Database.SqlQuery<MonthReport>(sqlString).ToList();
                Report = new ObservableCollection<MonthReport>(dataList);
                MyMessageQueue.Enqueue("Tạo báo cáo thành công!");
            }
            else
            {
                MyMessageQueue.Enqueue("Lỗi. Thông tin truy xuất không hợp lệ.");
            }
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
                MyMessageQueue.Enqueue("Lỗi. Đường dẫn báo cáo không hợp lệ.");
                return;
            }

            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    package.Workbook.Properties.Author = "Admin";
                    package.Workbook.Properties.Title = "Báo cáo loại món ăn";
                    package.Workbook.Worksheets.Add("Sheet 1");

                    ExcelWorksheet workSheet = package.Workbook.Worksheets[0];
                    //add sheet
                    workSheet.Name = "Sheet 1";
                    workSheet.Cells.Style.Font.Size = 12;
                    workSheet.Cells.Style.Font.Name = "Calibri";
                    // Tạo danh sách các column header
                    string[] arrColumnHeader = {
                        "Tên loại món",
                        "Tổng số lượng",
                        "Tổng doanh thu",
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

                    foreach (var item in Report)
                    {
                        colIndex = 1;
                        rowIndex++;

                        workSheet.Cells[rowIndex, colIndex++].Value = item.ten_loai_mon_an;
                        workSheet.Cells[rowIndex, colIndex++].Value = item.tong_so_luong;
                        workSheet.Cells[rowIndex, colIndex++].Value = item.tong_so_tien;
                    }

                    //Lưu file lại
                    Byte[] bin = package.GetAsByteArray();
                    File.WriteAllBytes(filePath, bin);
                }
                MyMessageQueue.Enqueue("Xuất excel thành công!");
            }
            catch (Exception EE)
            {
                MyMessageQueue.Enqueue("Lỗi. Đã xảy ra lỗi khi xuất file excel.");
            }

        }
    }
}
