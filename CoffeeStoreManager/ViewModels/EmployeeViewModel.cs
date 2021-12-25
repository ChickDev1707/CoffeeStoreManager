using CoffeeStoreManager.Models;
using CoffeeStoreManager.Views.ManageEmployee;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CoffeeStoreManager.Resources.Utils;
using MaterialDesignThemes.Wpf;
using OfficeOpenXml;
using System.IO;

namespace CoffeeStoreManager.ViewModels
{
    public class EmployeeViewModel : BaseViewModel
    {
        private ObservableCollection<ViewEmployee> employeeList;
        private ObservableCollection<ViewEmployee> dtg_employeeList;
        private List<LoaiNhanVien> employeeTypeList;
        private ViewEmployee selectedEmployee;
        private List<string> dataCbxYear;
        private string searchKey;

        private SnackbarMessageQueue myMessageQueue;

        public SnackbarMessageQueue MyMessageQueue { get => myMessageQueue; set { myMessageQueue = value; OnPropertyChanged(nameof(MyMessageQueue)); } }

        public string SearchKey { get => searchKey; set { searchKey = value; OnPropertyChanged(nameof(SearchKey)); } }
        public ViewEmployee SelectedEmployee { get => selectedEmployee; set { selectedEmployee = value; OnPropertyChanged(nameof(selectedEmployee)); } }

        public ObservableCollection<ViewEmployee> EmployeeList { get => employeeList; set { employeeList = value; OnPropertyChanged(nameof(employeeList)); } }
        public ObservableCollection<ViewEmployee> Dtg_employeeList { get => dtg_employeeList; set { dtg_employeeList = value; OnPropertyChanged(nameof(dtg_employeeList)); } }
        public List<LoaiNhanVien> EmployeeTypeList { get => employeeTypeList; set { employeeTypeList = value; OnPropertyChanged(nameof(employeeTypeList)); } }
        public List<string> DataCbxMonth { get; set; }
        public List<string> DataCbxYear
        {
            get => dataCbxYear; set { dataCbxYear = value; OnPropertyChanged(nameof(dataCbxYear)); }
        }
        #region Command
        public ICommand Search { get; set; }
        public ICommand AddEmployee { get; set; }
        public ICommand OpenAddEmployee { get; set; }
        public ICommand DeleteEmployee { get; set; }
        public ICommand OpenUpdateEmployee { get; set; }
       
        public ICommand CheckItem { get; set; }
        public ICommand RefreshData { get; set; }
        public ICommand ImportExcel { get; set; }
        public ICommand ExportExcel { get; set; }
        public ICommand IncreaseDay { get; set; }
        public ICommand DecreaseDay { get; set; }



        #endregion

        public EmployeeViewModel()
        {
            employeeList = new ObservableCollection<ViewEmployee>();
            EmployeeTypeList = new List<LoaiNhanVien>();
            OpenAddEmployee = new RelayCommand<System.Windows.Window>((p) => { return true; }, (p) => { openAddEmployee(p); });
            DeleteEmployee = new RelayCommand<object>((p) => { return true; }, (p) => { deleteEmployee(p); });
            OpenUpdateEmployee = new RelayCommand<object>((p) => { return true; }, (p) => { openUpdateEmployee(p); });
            
            CheckItem = new RelayCommand<ListView>((p) => { return true; }, (p) => { checkSelectItem(p); });
            Search = new RelayCommand<object>((p) => { return true; }, (p) => { searchItem(p); });
            RefreshData = new RelayCommand<object>((p) => { return true; }, (p) => { refreshListEmployee(p); });
            IncreaseDay = new RelayCommand<object>((p) => { return true; }, (p) => { increaseDay(p); });
            DecreaseDay = new RelayCommand<object>((p) => { return true; }, (p) => { decreaseDay(p); });
            ExportExcel = new RelayCommand<DataGrid>((p) => { return true; }, (p) => { ExportFileExcel(p); });
            ImportExcel = new RelayCommand<DataGrid>((p) => { return true; }, (p) => { ImportFileExcel(p); });


            MyMessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(4000));
            MyMessageQueue.DiscardDuplicates = true;

            MessageSalary();
            loadData();
        }
        void MessageSalary()
        {
            int daysInM = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            if (DateTime.Now.Day == daysInM)
            {
                SolveSalary();
                MyMessageQueue.Enqueue("Đã tính lương cho nhân viên");
            }
            else
            {
                string s = (daysInM - DateTime.Now.Day).ToString();
                MyMessageQueue.Enqueue("Còn " + s + " ngày nữa sẽ tới ngày tính lương");
            }    
        }
        void SolveSalary()
        {
            int month = DateTime.Today.Month;
            int year = DateTime.Today.Year;
            int days = DateTime.DaysInMonth(year, month);
            decimal salary_per_day,totalSalary = 0;
               

            DateTime selectedDate = new DateTime(year, month, days);
            List<NhanVien> list = DataProvider.Ins.DB.NhanViens.
                Where(p => p.ngay_vao_lam <= selectedDate).
                ToList();

            for (int i = 0; i < list.Count; i++)
            {
                int manv = list[i].ma_nhan_vien;
                int maloainv = (int)list[i].ma_loai_nhan_vien;
                LoaiNhanVien lnv = DataProvider.Ins.DB.LoaiNhanViens.
                    Where(p => p.ma_loai_nhan_vien == maloainv).SingleOrDefault();
                salary_per_day = (decimal)lnv.tien_luong / days;   
                if (maloainv == 1)
                {
                    DateTime now = DateTime.Now;
                    CaLamPartTime calam = DataProvider.Ins.DB.CaLamPartTimes.
                        Where(t => t.ma_nhan_vien == manv &&
                                   now.Year == t.ngay_lam.Value.Year &&
                                    now.Month == t.ngay_lam.Value.Month).FirstOrDefault();
                    if (calam != null)
                    {
                        totalSalary += (decimal)(calam.so_gio_lam * lnv.tien_luong);
                    }
                    else
                    {
                        totalSalary += 0;
                    }
                }
                else
                {
                    //viewE.VisiblePartTime = System.Windows.Visibility.Visible;
                    //viewE.so_gio_lam = null;
                    //viewE.so_ngay_nghi = (int)list[i].so_ngay_nghi;
                    if (list[i].ngay_vao_lam.Month == month)
                    {
                        int workdays = (int)(selectedDate - list[i].ngay_vao_lam).TotalDays + 1;
                        totalSalary = (decimal)(lnv.tien_luong - salary_per_day * (days - workdays + list[i].so_ngay_nghi));
                    }
                    else
                    {
                        if (list[i].so_ngay_nghi >= days)
                        {
                            totalSalary += 0;
                        }
                        totalSalary += (decimal)(lnv.tien_luong - (salary_per_day * list[i].so_ngay_nghi));
                    }
                }
                NhanVien nv = DataProvider.Ins.DB.NhanViens.Where(t => t.ma_nhan_vien == manv).SingleOrDefault();
                nv.so_ngay_nghi = 0;
                DataProvider.Ins.DB.SaveChanges();
            }
            SaveSalary(totalSalary);
        }
        void SaveSalary(decimal totalSalary)
        {
            DateTime now = DateTime.Now;
            PhieuTinhLuong check = DataProvider.Ins.DB.PhieuTinhLuongs.
                            Where(p => p.ngay_tinh_luong.Value.Year == now.Year && p.ngay_tinh_luong.Value.Month == now.Month)
                            .FirstOrDefault();
            if (check == null)
            {
                PhieuTinhLuong Sal = new PhieuTinhLuong()
                {
                    ngay_tinh_luong = new DateTime(now.Year, now.Month, 1),
                    tong_tien = totalSalary,
                };
                DataProvider.Ins.DB.PhieuTinhLuongs.Add(Sal);
                DataProvider.Ins.DB.SaveChanges();
            }
            else
            {
                check.tong_tien = totalSalary;
                DataProvider.Ins.DB.SaveChanges();
            }
        }

        void loadData()
        {
            loadDataEmployee();
        }

        void checkSelectItem(ListView selectedItemsp)
        {
            System.Collections.IList list = selectedItemsp.SelectedItems;
            for (int j = 0; j < EmployeeList.Count; j++)
            {
                EmployeeList[j].check_selected_item = false;
                for (int i = 0; i < list.Count; i++)
                {
                    if (EmployeeList[j].ma_nv == (list[i] as ViewEmployee).ma_nv)
                    {
                        EmployeeList[j].check_selected_item = true;
                    }
                }
            }
        }
        void refreshListEmployee(object p)
        {
            loadData();
        }
        void searchItem(object p)
        {
            List<NhanVien> list = DataProvider.Ins.DB.NhanViens.Where(t => t.ho_ten.ToLower().Contains(SearchKey.ToLower())).ToList();
            EmployeeList = getViewEmployeeFromList(list);
        }
        void increaseDay(object p)
        {
                for (int i = 0; i < EmployeeList.Count; i++)
                {
                    if (EmployeeList[i].check_selected_item == true)
                    {
                        if (EmployeeList[i].ma_loai_nhan_vien == 1)
                        {
                            continue;
                        }
                        int m = EmployeeList[i].ma_nv;
                        NhanVien nv = DataProvider.Ins.DB.NhanViens.
                        Where(t => t.ma_nhan_vien == m).SingleOrDefault();
                        nv.so_ngay_nghi++;
                    }
                }
                DataProvider.Ins.DB.SaveChanges();
                loadData();
        }
        void decreaseDay(object p)
        {
                for (int i = 0; i < EmployeeList.Count; i++)
                {
                   
                    if (EmployeeList[i].check_selected_item == true)
                    {
                        if (EmployeeList[i].ma_loai_nhan_vien == 1)
                        {
                            continue;
                        }
                        int m = EmployeeList[i].ma_nv;
                        NhanVien nv = DataProvider.Ins.DB.NhanViens.
                          Where(t => t.ma_nhan_vien == m).SingleOrDefault();
                        if (nv.so_ngay_nghi > 0)
                        {
                            if (SelectedEmployee != null)
                            {
                                nv.so_ngay_nghi--;
                            }
                        }
                        DataProvider.Ins.DB.SaveChanges();
                    }
                }
                loadData();
        }
        bool checkNumberOfEmployee()
        {
            int countitem = 0;
            for (int i = 0; i < EmployeeList.Count; i++)
            {
                if (EmployeeList[i].check_selected_item == true)
                {
                    countitem++;
                }
            }
            if (countitem != 1)
            {
                return false;
            }
            return true;
        }
        public void loadDataEmployee()
        {
            EmployeeTypeList.Clear();
            List<NhanVien> lNv = DataProvider.Ins.DB.NhanViens.ToList();
            EmployeeList = getViewEmployeeFromList(lNv);
            Dtg_employeeList = new ObservableCollection<ViewEmployee>(EmployeeList);
            loadDataCbxEmployeeType();
        }
        public void loadDataTypeEmployee(object p)
        {
            EmployeeTypeList.Clear();
            List<NhanVien> lNv = DataProvider.Ins.DB.NhanViens.ToList();
            EmployeeList = getViewEmployeeFromList(lNv);
        }
        void loadDataCbxEmployeeType()
        {
            EmployeeTypeList = DataProvider.Ins.DB.LoaiNhanViens.ToList();
        }
        void ImportFileExcel(object p)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = dialog.FileName;
                AddImportedEmployees(fileName);
            }
        }
        void AddImportedEmployees(string fileName)
        {
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
                        bool check = true;
                        NhanVien newEmployee = new NhanVien()
                        {
                            ma_nhan_vien = Convert.ToInt32(workSheet.Cells[i, j++].Value),
                            ma_loai_nhan_vien = Convert.ToInt32(workSheet.Cells[i, j++].Value),
                            ho_ten = workSheet.Cells[i, j++].Value.ToString(),
                            ngay_sinh = DateTime.FromOADate((double)(workSheet.Cells[i, j++].Value)),
                            ngay_vao_lam = DateTime.FromOADate((double)(workSheet.Cells[i, j++].Value)),
                            sdt = workSheet.Cells[i, j++].Value.ToString(),
                            dia_chi = workSheet.Cells[i, j++].Value.ToString(),
                        };
                        for(int g=0;g<EmployeeList.Count;g++)
                        {
                            if(newEmployee.sdt == EmployeeList[g].sdt)
                            {
                                check = false ;
                            }
                        }
                        // add UserInfo vào danh sách userList◘
                        if (check == true)
                        {
                            DataProvider.Ins.DB.NhanViens.Add(newEmployee);
                            DataProvider.Ins.DB.SaveChanges();
                        }
                        MyMessageQueue.Enqueue("Thêm dữ liệu từ file excel thành công!");

                    }
                    catch (Exception error)
                    {
                        MyMessageQueue.Enqueue("Lỗi. Đã xảy ra lỗi khi đọc file excel.");
                    }
                }
                loadData();
            }
            catch (Exception ee)
            {
                MyMessageQueue.Enqueue("Lỗi. Đã xảy ra lỗi khi import file excel.");
            }
        }
        void ExportFileExcel(DataGrid dtGrid)
        {
            string filePath = "";
            System.Windows.Forms.SaveFileDialog dialog = new System.Windows.Forms.SaveFileDialog();
            dialog.Filter = "Excel | *.xlsx | Excel 2003 | *.xls";

            // Nếu mở file và chọn nơi lưu file thành công sẽ lưu đường dẫn lại dùng
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
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
                    package.Workbook.Properties.Title = "Danh sách món ăn";
                    package.Workbook.Worksheets.Add("Sheet 1");

                    ExcelWorksheet workSheet = package.Workbook.Worksheets[0];
                    //add sheet
                    workSheet.Name = "Sheet 1";
                    workSheet.Cells.Style.Font.Size = 12;
                    workSheet.Cells.Style.Font.Name = "Calibri";
                    // Tạo danh sách các column header
                    string[] arrColumnHeader = {
                        "Mã nhân viên",
                        "Mã loại",
                        "Họ tên",
                        "Ngày sinh",
                        "Ngày vào làm",
                        "SĐT",
                        "Địa chỉ"
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

                    foreach (var item in EmployeeList)
                    {
                        colIndex = 1;
                        rowIndex++;

                        workSheet.Cells[rowIndex, colIndex++].Value = item.ma_nv;
                        workSheet.Cells[rowIndex, colIndex++].Value = item.ma_loai_nhan_vien;
                        workSheet.Cells[rowIndex, colIndex++].Value = item.ho_ten;
                        workSheet.Cells[rowIndex, colIndex++].Value = item.ngay_sinh;
                        workSheet.Cells[rowIndex, colIndex++].Value = item.ngay_vao_lam;
                        workSheet.Cells[rowIndex, colIndex++].Value = item.sdt;
                        workSheet.Cells[rowIndex, colIndex++].Value = item.dia_chi;

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
        ObservableCollection<ViewEmployee> getViewEmployeeFromList(List<NhanVien> listNv)
        {
            ObservableCollection<ViewEmployee> obNv = new ObservableCollection<ViewEmployee>();
            int index = 1;
            foreach (var employee in listNv)
            {
                if (employee != null)
                {
                    LoaiNhanVien employeeType = DataProvider.Ins.DB.LoaiNhanViens.Where(p => p.ma_loai_nhan_vien == employee.ma_loai_nhan_vien).FirstOrDefault<LoaiNhanVien>();
                    ViewEmployee viewEmployee = new ViewEmployee()
                    {
                        STT = index,
                        ho_ten = employee.ho_ten,
                        dia_chi = employee.dia_chi,
                        ngay_sinh = (DateTime)employee.ngay_sinh,
                        sdt = employee.sdt,
                        ngay_vao_lam = employee.ngay_vao_lam,
                        ma_nv = employee.ma_nhan_vien,
                        so_ngay_nghi = (employee.so_ngay_nghi == null ? 0 : (int)employee.so_ngay_nghi)
                    };
                    if (employeeType != null)
                    {
                        viewEmployee.tien_luong = (decimal)employeeType.tien_luong;
                        viewEmployee.ma_loai_nhan_vien = employeeType.ma_loai_nhan_vien;
                        viewEmployee.VisiblePartTime = Visibility.Visible;
                        if (employeeType.ma_loai_nhan_vien == 1)
                        {
                            viewEmployee.so_ngay_nghi = 0;
                            viewEmployee.VisiblePartTime = Visibility.Hidden;
                        }
                        viewEmployee.loai_nhan_vien = employeeType.ten_loai_nhan_vien;
                    }
                    obNv.Add(viewEmployee);
                    index++;
                }
            }
            return obNv;
        }
        void openAddEmployee(System.Windows.Window p)
        {
            var a = new AddEmployeeWindow(this);
            a.ShowDialog();
        }
        void deleteEmployee(object p)
        {
            bool check = true;
            for (int i = 0; i < EmployeeList.Count; i++)
            {
                if (EmployeeList[i].check_selected_item == true)
                {
                    int v = EmployeeList[i].ma_nv;
                    if (CanDelete(v) == true)
                    {
                        
                        var delEmployee = DataProvider.Ins.DB.NhanViens.
                          Where(employee => employee.ma_nhan_vien == v).FirstOrDefault();
                        DataProvider.Ins.DB.NhanViens.Remove(delEmployee);
                        DataProvider.Ins.DB.SaveChanges();
                    }
                    else
                    {
                        check = false;
                    }
                }
            }
            if(check == false)
            {
                MyMessageQueue.Enqueue("Lỗi. Nhân viên còn lịch làm việc không thể xóa");
            }
            else
            {
                MyMessageQueue.Enqueue("Xóa thành công");
            }    
                loadDataEmployee();
        }
        bool CanDelete(int manv)
        {
            bool check = true;
            List<CaLamPartTime> LCalam = DataProvider.Ins.DB.CaLamPartTimes.
                Where(t => t.ma_nhan_vien == manv && t.ngay_lam.Value.Month == DateTime.Now.Month && t.ngay_lam.Value.Year == DateTime.Now.Year)
                .ToList();
            if(LCalam.Count !=0)
            {
                check = false;
            }
            return check;
        }
        void openUpdateEmployee(object p)
        {
            if(checkNumberOfEmployee() == false)
            {
                this.MyMessageQueue.Enqueue("Vui lòng chỉ chọn 1 nhân viên");
                return;
            }
            if (SelectedEmployee != null)
            {
                var window = new UpdateEmployeeWindow(this);

                window.ShowDialog();
            }
        }
    }
}
