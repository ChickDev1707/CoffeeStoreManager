using CoffeeStoreManager.Models;
using CoffeeStoreManager.Views.ManageEmployee;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CoffeeStoreManager.ViewModels
{
    class EmployeeViewModel : BaseViewModel
    {
        private ObservableCollection<ViewEmployee> employeeList;
        private ObservableCollection<ViewEmployee> salaryEmployeeList;
        private List<LoaiNhanVien> employeeTypeList;
        private int preMaNhanVien;
        private decimal totalSalary;
        private bool isSelectedTab;
        //private int ngayTinhLuong;
        private string addEmployeeSdt;
        private int addEmployeeMaloainhanvien;
        private string addEmployeeHovaten;
        private string addEmployeeDiachi;
        private ViewEmployee selectedNhanVien;
        private DateTime addEmployeeNgayvaolam;
        private DateTime addEmployeeNgaySinh;
        private List<string> dataCbxYear;
        private ObservableCollection<ViewEmployee> employeeListContainer;
        private int widthCheckCol;

        public int WidthCheckCol
        {
            get { return widthCheckCol; }
            set { widthCheckCol = value; OnPropertyChanged(nameof(widthCheckCol)); }
        }
        public bool IsSelectedTab { get => isSelectedTab; set { isSelectedTab = value; OnPropertyChanged(nameof(isSelectedTab)); loadData(); } }
        public ViewEmployee SelectedNhanVien { get => selectedNhanVien; set { selectedNhanVien = value; OnPropertyChanged(nameof(selectedNhanVien)); } }
        public string AddEmployeeSdt { get => addEmployeeSdt; set { addEmployeeSdt = value; OnPropertyChanged(nameof(addEmployeeSdt)); } }
        public int AddEmployeeMaloainhanvien { get => addEmployeeMaloainhanvien; set { addEmployeeMaloainhanvien = value; OnPropertyChanged(nameof(addEmployeeMaloainhanvien)); } }
        public string AddEmployeeHovaten { get => addEmployeeHovaten; set { addEmployeeHovaten = value; OnPropertyChanged(nameof(addEmployeeHovaten)); } }
        public string AddEmployeeDiachi { get => addEmployeeDiachi; set { addEmployeeDiachi = value; OnPropertyChanged(nameof(addEmployeeDiachi)); } }
        public DateTime AddEmployeeNgayvaolam { get => addEmployeeNgayvaolam; set { addEmployeeNgayvaolam = value; OnPropertyChanged(nameof(addEmployeeNgayvaolam)); } }
        public DateTime AddEmployeeNgaySinh { get => addEmployeeNgaySinh; set { addEmployeeNgaySinh = value; OnPropertyChanged(nameof(addEmployeeNgaySinh)); } }
        public ICommand Search { get; set; }
        public ICommand AddEmployee { get; set; }
        public ICommand IncAbsentDay { get; set; }
        public ICommand DecAbsentDay { get; set; }
        public ICommand OpenAddEmployee { get; set; }
        public ICommand DeleteEmployee { get; set; }
        public ICommand UpdateEmployee { get; set; }
        public ICommand OpenUpdateEmployee { get; set; }
        public ICommand OpenSalaryWindow { get; set; }
        public ICommand LoadSalary { get; set; }
        public ICommand CheckListItem { get; set; }
        public ICommand CheckItem { get; set; }
        public ICommand Find { get; set; }
        public ObservableCollection<ViewEmployee> EmployeeList { get => employeeList; set { employeeList = value; OnPropertyChanged(nameof(employeeList)); } }
        public ObservableCollection<ViewEmployee> SalaryEmployeeList { get => salaryEmployeeList; set { salaryEmployeeList = value; OnPropertyChanged(nameof(salaryEmployeeList)); } }

        public List<LoaiNhanVien> EmployeeTypeList { get => employeeTypeList; set { employeeTypeList = value; OnPropertyChanged(nameof(employeeTypeList)); } }
        public List<string> DataCbxMonth { get; set; }
        public List<string> DataCbxYear
        {
            get => dataCbxYear; set { dataCbxYear = value; OnPropertyChanged(nameof(dataCbxYear)); }
        }
public EmployeeViewModel()
        {
            loadData();
        }
        void changeWidthCol(object t)
        {
            WidthCheckCol = WidthCheckCol == 0 ? 30 : 0;
            if (WidthCheckCol == 0)
            {
                for (int i = 0; i < EmployeeList.Count; i++)
                {
                    EmployeeList[i].check_selected_item = false;
                }
            }
        }
        void loadData()
        {
            //ngayTinhPhieuTinhLuong = 5;
            widthCheckCol = 30;
            employeeList = new ObservableCollection<ViewEmployee>();
            employeeListContainer = new ObservableCollection<ViewEmployee>();
            EmployeeTypeList = new List<LoaiNhanVien>();
            AddEmployee = new RelayCommand<object>((p) => { return true; }, (p) => { addEmployee(p); });
            OpenAddEmployee = new RelayCommand<object>((p) => { return true; }, (p) => { openAddEmployee(p); });
            DeleteEmployee = new RelayCommand<object>((p) => { return true; }, (p) => { deleteEmployee(p); });
            UpdateEmployee = new RelayCommand<object>((p) => { return true; }, (p) => { updateEmployee(p); });
            OpenUpdateEmployee = new RelayCommand<object>((p) => { return true; }, (p) => { openUpdateEmployee(p); });
            OpenSalaryWindow = new RelayCommand<object>((p) => { return true; }, (p) => { openSalaryWindow(p); });
            LoadSalary = new RelayCommand<object>((p) => { return true; }, (p) => { loadSalaryEmployeeList(p); });
            IncAbsentDay = new RelayCommand<object>((p) => { return true; }, (p) => { increaseAbsentDay(p); });
            DecAbsentDay = new RelayCommand<object>((p) => { return true; }, (p) => { decreaseAbsentDay(p); });
            CheckListItem = new RelayCommand<object>((p) => { return true; }, (p) => { changeWidthCol(p); });
            CheckItem = new RelayCommand<ListView>((p) => { return true; }, (p) => { checkSelectItem(p); });
            Find = new RelayCommand<TextBox>((p) => { return true; }, (p) => { findItem(p); });
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
        void findItem(TextBox p)
        {
            EmployeeList.Clear();
            if (p.Text == "")
            {
                loadDataEmployee();
            }
            else
            {
                for (int i = 0; i < employeeListContainer.Count; i++)
                {
                    string findString = p.Text.ToLower();
                    if (employeeListContainer[i].ho_ten.ToLower().Contains(findString) == false)
                    {
                        EmployeeList.Remove(employeeListContainer[i]);
                    }
                    else
                    {
                        EmployeeList.Add(employeeListContainer[i]);
                    }
                }
            }
        }
        string convertMoney(string money)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
            string a = double.Parse(money).ToString("#,###", cul.NumberFormat);
            return a;
        }
        void loadDataEmployee()
        {
            EmployeeTypeList.Clear();
            List<NhanVien> lNv = DataProvider.Ins.DB.NhanViens.ToList();
            EmployeeList = getViewEmployeeFromList(lNv);
            loadDataCbxEmployeeType();
            employeeListContainer = getViewEmployeeFromList(lNv);
        }
        void loadDataCbxEmployeeType()
        {
            EmployeeTypeList = DataProvider.Ins.DB.LoaiNhanViens.ToList();
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
                        sdt = employee.sdt,
                        ngay_vao_lam = employee.ngay_vao_lam,
                        ma_nv = employee.ma_nhan_vien,
                        so_ngay_nghi = (employee.so_ngay_nghi == null ? 0 : (int)employee.so_ngay_nghi).ToString(),
                    };
                    if (employeeType != null)
                    {
                        viewEmployee.tien_luong = employeeType.tien_luong.ToString();
                        viewEmployee.tien_luong = convertMoney(viewEmployee.tien_luong);
                        viewEmployee.ma_loai_nhan_vien = employeeType.ma_loai_nhan_vien;
                        if (employeeType.ma_loai_nhan_vien == 1)
                        {
                            viewEmployee.so_ngay_nghi = null;
                        }
                        viewEmployee.loai_nhan_vien = employeeType.ten_loai_nhan_vien;
                    }
                    obNv.Add(viewEmployee);
                    index++;
                }
            }
            return obNv;
        }
        void openAddEmployee(object p)
        {
            var a = new AddEmployeeWindow();
            a.ShowDialog();
        }
        void addEmployee(object p)
        {
            QuyDinh QDinh = DataProvider.Ins.DB.QuyDinhs.Select(t => t).FirstOrDefault();
            if (AddEmployeeHovaten == null)
            {
                MessageBox.Show("Ten khong duoc de trong!!!");
                return;
            }
            if (AddEmployeeSdt == null)
            {
                MessageBox.Show("So dien thoai khong duoc de trong!!!");
                return;
            }
            else
            {
                for (int i = 0; i < EmployeeList.Count; i++)
                {
                    if (AddEmployeeSdt == EmployeeList[i].sdt)
                    {
                        MessageBox.Show("So dien thoai bi trung!!!");
                        return;
                    }
                }
            }
            if (AddEmployeeNgaySinh == null)
            {
                MessageBox.Show("Ngay sinh khong duoc de trong!!!");
                return;
            }
            if (AddEmployeeSdt.All(char.IsDigit) == false)
            {
                MessageBox.Show("So dien thoai khong hop le !!! ");
                return;
            }
            if (AddEmployeeNgaySinh.Year < 1900)
            {
                MessageBox.Show("Ngay vao lam khong hop le !!! ");
                return;
            }
            int tuoi = DateTime.Now.Year - AddEmployeeNgaySinh.Year;
            if (tuoi < QDinh.tuoi_toi_thieu_nv || tuoi > QDinh.tuoi_toi_da_nv)
            {
                MessageBox.Show("Do tuoi khong hop le!!! ");
                return;
            }
            if (AddEmployeeMaloainhanvien == 0)
            {
                MessageBox.Show("Vui long chon loai nhan vien !!! ");
                return;
            }
            if (AddEmployeeNgayvaolam.Year < 2000)
            {
                MessageBox.Show("Ngay vao lam khong hop le!!!");
                return;
            }
            NhanVien add = new NhanVien()
            {
                ho_ten = addEmployeeHovaten,
                ngay_vao_lam = addEmployeeNgayvaolam,
                sdt = addEmployeeSdt,
                dia_chi = addEmployeeDiachi,
                ma_loai_nhan_vien = addEmployeeMaloainhanvien,
                so_ngay_nghi = 0,
                ngay_sinh = AddEmployeeNgaySinh,
            };
            DataProvider.Ins.DB.NhanViens.Add(add);
            DataProvider.Ins.DB.SaveChanges();
            loadDataEmployee();
            clearAddEmployeeForm();
        }
        void clearAddEmployeeForm()
        {
            AddEmployeeHovaten = "";
            AddEmployeeSdt = "";
            AddEmployeeDiachi = "";
        }
        void deleteEmployee(object p)
        {
            bool checkitem = false;
            for (int i = 0; i < EmployeeList.Count; i++)
            {
                if (EmployeeList[i].check_selected_item == true)
                {
                    int v = EmployeeList[i].ma_nv;
                    var delEmployee = DataProvider.Ins.DB.NhanViens.
                        Where(employee => employee.ma_nhan_vien == v).FirstOrDefault();
                    List<CaLamPartTime> LCalam = DataProvider.Ins.DB.CaLamPartTimes.Where(t => t.ma_nhan_vien == delEmployee.ma_nhan_vien).ToList();
                    for (int j = 0; j < LCalam.Count; j++)
                    {
                        DataProvider.Ins.DB.CaLamPartTimes.Remove(LCalam[j]);
                        DataProvider.Ins.DB.SaveChanges();
                    }
                    DataProvider.Ins.DB.NhanViens.Remove(delEmployee);
                    DataProvider.Ins.DB.SaveChanges();
                    checkitem = true;
                }
            }
            if (checkitem == true)
            {
                loadDataEmployee();
            }
            else
            {
                if (SelectedNhanVien != null)
                {
                    var delEmployee = DataProvider.Ins.DB.NhanViens.
                      Where(employee => employee.ma_nhan_vien == SelectedNhanVien.ma_nv).FirstOrDefault();
                    List<CaLamPartTime> LCalam = DataProvider.Ins.DB.CaLamPartTimes.Where(t => t.ma_nhan_vien == delEmployee.ma_nhan_vien).ToList();
                    for (int i = 0; i < LCalam.Count; i++)
                    {
                        DataProvider.Ins.DB.CaLamPartTimes.Remove(LCalam[i]);
                        DataProvider.Ins.DB.SaveChanges();
                    }
                    DataProvider.Ins.DB.NhanViens.Remove(delEmployee);
                    DataProvider.Ins.DB.SaveChanges();
                    loadDataEmployee();
                }
            }
        }
        void updateEmployee(object p)
        {

            if (SelectedNhanVien.ho_ten == null)
            {
                MessageBox.Show("Ten khong duoc de trong!!!");
                return;
            }
            if (SelectedNhanVien.sdt.All(char.IsDigit) == false)
            {
                MessageBox.Show("So dien thoai khong hop le !!! ");
                return;
            }
            else
            {
                for (int i = 0; i < EmployeeList.Count; i++)
                {
                    if (AddEmployeeSdt == EmployeeList[i].sdt)
                    {
                        MessageBox.Show("So dien thoai bi trung!!!");
                        return;
                    }
                }
            }
            if (SelectedNhanVien.ngay_vao_lam == null)
            {
                MessageBox.Show("Ngay vao lam khong duoc de trong!!!");
                return;
            }
            if (SelectedNhanVien.ngay_vao_lam.Year < 2000)
            {
                MessageBox.Show("Ngay vao lam khong hop le!!!");
                return;
            }
            var updEmployee = DataProvider.Ins.DB.NhanViens.
              Where(t => t.ma_nhan_vien == preMaNhanVien).FirstOrDefault();
            updEmployee.ho_ten = SelectedNhanVien.ho_ten;
            updEmployee.dia_chi = SelectedNhanVien.dia_chi;
            updEmployee.sdt = SelectedNhanVien.sdt;
            updEmployee.ngay_vao_lam = SelectedNhanVien.ngay_vao_lam;
            updEmployee.ma_loai_nhan_vien = SelectedNhanVien.ma_loai_nhan_vien;
            DataProvider.Ins.DB.SaveChanges();
            loadDataEmployee();
        }
        void openUpdateEmployee(object p)
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
                MessageBox.Show("Vui long chi chon 1 nhan vien");
                return;
            }
            if (SelectedNhanVien != null)
            {
                var window = new UpdateEmployeeWindow();
                preMaNhanVien = SelectedNhanVien.ma_nv;
                window.ShowDialog();
            }
        }
        void openSalaryWindow(object p)
        {
            SalaryEmployeeList = new ObservableCollection<ViewEmployee>();
            SalaryWindow Swindow = new SalaryWindow();
            Swindow.ShowDialog();
        }
        void loadSalaryEmployeeList(object p)
        {
            SalaryEmployeeList.Clear();
            SalaryEmployeeList = solveSalary();
            SaveDataSalary();
            refreshData();
        }
        void SaveDataSalary()
        {
            DateTime now = DateTime.Now;
            PhieuTinhLuong check = DataProvider.Ins.DB.PhieuTinhLuongs.
                            Where(p => p.ngay_tinh_luong.Value.Year == now.Year && p.ngay_tinh_luong.Value.Month == now.Month).FirstOrDefault();
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
        void refreshData()
        {
            List<NhanVien> list = DataProvider.Ins.DB.NhanViens.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                list[i].so_ngay_nghi = 0;
            }
            DataProvider.Ins.DB.SaveChanges();
            loadData();
        }
        ObservableCollection<ViewEmployee> solveSalary()
        {
            decimal salary_per_day;
            totalSalary = 0;
            int month = DateTime.Today.Month;
            int year = DateTime.Today.Year;
            int days = DateTime.DaysInMonth(year, month);
            ObservableCollection<ViewEmployee> res = new ObservableCollection<ViewEmployee>();
            DateTime selectedDate = new DateTime(year, month, days);
            List<NhanVien> list = DataProvider.Ins.DB.NhanViens.
                Where(p => p.ngay_vao_lam <= selectedDate).
                ToList();
            for (int i = 0; i < list.Count; i++)
            {
                int maloainv = (int)list[i].ma_loai_nhan_vien;
                LoaiNhanVien lnv = DataProvider.Ins.DB.LoaiNhanViens.
                    Where(p => p.ma_loai_nhan_vien == maloainv).SingleOrDefault();
                salary_per_day = (int)lnv.tien_luong / days;
                ViewEmployee viewE = new ViewEmployee();
                viewE.loai_nhan_vien = lnv.ten_loai_nhan_vien;
                viewE.ma_nv = list[i].ma_nhan_vien;
                viewE.ho_ten = list[i].ho_ten;
                viewE.ngay_vao_lam = list[i].ngay_vao_lam;
                viewE.Sngay_vao_lam = String.Format("{0:dd/MM/yyyy}", list[i].ngay_vao_lam);
                viewE.tien_luong = convertMoney(lnv.tien_luong.ToString());
                if (maloainv == 1)
                {
                    DateTime now = DateTime.Now;
                    int manv = list[i].ma_nhan_vien;

                    CaLamPartTime calam = DataProvider.Ins.DB.CaLamPartTimes.
                        Where(t => t.ma_nhan_vien == manv &&
                                   now.Year == t.ngay_lam.Year &&
                                    now.Month == t.ngay_lam.Month).FirstOrDefault();
                    if (calam != null)
                    {
                        viewE.so_gio_lam = calam.so_gio_lam.ToString();
                        viewE.luong_nhan = (calam.so_gio_lam * lnv.tien_luong).ToString();
                    }
                    else
                    {
                        viewE.so_gio_lam = "0";
                        viewE.luong_nhan = "0";
                    }

                }
                else
                {
                    viewE.so_gio_lam = null;
                    viewE.so_ngay_nghi = list[i].so_ngay_nghi.ToString();
                    if (viewE.ngay_vao_lam.Month == month)
                    {
                        int workdays = (int)(selectedDate - viewE.ngay_vao_lam).TotalDays + 1;
                        viewE.luong_nhan = (lnv.tien_luong - salary_per_day * (days - workdays + list[i].so_ngay_nghi)).ToString();
                    }
                    else
                    {
                        if (list[i].so_ngay_nghi >= days)
                        {
                            viewE.luong_nhan = "0";
                        }
                        viewE.luong_nhan = (lnv.tien_luong - salary_per_day * list[i].so_ngay_nghi).ToString();
                    }
                }

                totalSalary += Decimal.Parse(viewE.luong_nhan);
                viewE.luong_nhan = convertMoney(viewE.luong_nhan);
                res.Add(viewE);
            }
            return res;
        }
        void increaseAbsentDay(object p)
        {
            bool checkitem = false;
            for (int i = 0; i < EmployeeList.Count; i++)
            {
                if (EmployeeList[i].check_selected_item == true && EmployeeList[i].ma_loai_nhan_vien != 1)
                {
                    int v = EmployeeList[i].ma_nv;
                    NhanVien upd = DataProvider.Ins.DB.NhanViens.Where(t => t.ma_nhan_vien == v).SingleOrDefault();
                    upd.so_ngay_nghi++;
                    checkitem = true;
                    DataProvider.Ins.DB.SaveChanges();
                }
            }
            if (checkitem == true)
            {
                loadDataEmployee();
            }
            else
            {
                if (SelectedNhanVien != null)
                {
                    NhanVien upd = DataProvider.Ins.DB.NhanViens.Where(t => t.ma_nhan_vien == SelectedNhanVien.ma_nv).FirstOrDefault();
                    if (upd.ma_loai_nhan_vien == 1)
                    {
                        MessageBox.Show("Day la nhan vien part time");
                        return;
                    }
                    upd.so_ngay_nghi++;
                    DataProvider.Ins.DB.SaveChanges();
                    loadDataEmployee();
                }
            }
        }
        void decreaseAbsentDay(object p)
        {
            bool checkitem = false;
            for (int i = 0; i < EmployeeList.Count; i++)
            {
                if (EmployeeList[i].check_selected_item == true && EmployeeList[i].ma_loai_nhan_vien != 1)
                {
                    int v = EmployeeList[i].ma_nv;
                    NhanVien upd = DataProvider.Ins.DB.NhanViens.Where(t => t.ma_nhan_vien == v).SingleOrDefault();
                    if (Int32.Parse(EmployeeList[i].so_ngay_nghi) > 0)
                    {
                        upd.so_ngay_nghi--;
                        DataProvider.Ins.DB.SaveChanges();
                        checkitem = true;
                    }
                }
            }
            if (checkitem == true)
            {
              
                loadDataEmployee();
            }
            else
            {
                if (SelectedNhanVien != null)
                {
                    NhanVien upd = DataProvider.Ins.DB.NhanViens.Where(t => t.ma_nhan_vien == SelectedNhanVien.ma_nv).FirstOrDefault();
                    if (upd.ma_loai_nhan_vien == 1)
                    {
                        MessageBox.Show("Day la nhan vien part time");
                        return;
                    }
                    if (Int32.Parse(SelectedNhanVien.so_ngay_nghi) > 0)
                    {
                        upd.so_ngay_nghi--;
                        DataProvider.Ins.DB.SaveChanges();
                    }
                    loadDataEmployee();
                }
            }
        }
    }
}
