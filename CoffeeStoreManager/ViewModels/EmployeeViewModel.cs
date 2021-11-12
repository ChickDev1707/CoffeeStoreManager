    using CoffeeStoreManager.Models;
using CoffeeStoreManager.Views.ManageEmployee;
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
        public ObservableCollection<ViewEmployee> EmployeeList { get => employeeList; set { employeeList = value; OnPropertyChanged(nameof(employeeList)); } }
        public ObservableCollection<ViewEmployee> SalaryEmployeeList { get => salaryEmployeeList; set { salaryEmployeeList = value; OnPropertyChanged(nameof(salaryEmployeeList)); } }

        public List<LoaiNhanVien> EmployeeTypeList { get => employeeTypeList; set { employeeTypeList = value; OnPropertyChanged(nameof(employeeTypeList)); } }
        
        public List<string> DataCbxMonth { get; set; }
        public List<string> DataCbxYear { get => dataCbxYear; set { dataCbxYear = value; OnPropertyChanged(nameof(dataCbxYear)); } }

        public EmployeeViewModel()
        {
            loadData();
        }
        void loadData()
        {
            //ngayTinhLuong = 5;
            employeeList = new ObservableCollection<ViewEmployee>();
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
            loadDataEmployee();
        }
        void loadDataEmployee()
        {
            EmployeeTypeList.Clear();
            List<NhanVien> lNv = DataProvider.Ins.DB.NhanViens.ToList();
            EmployeeList = getViewEmployeeFromList(lNv);
            loadDataCbxEmployeeType();

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
                        ho_ten = employee.ho_va_ten,
                        dia_chi = employee.dia_chi,
                        sdt = employee.sdt,
                        ngay_vao_lam = employee.ngay_vao_lam,
                        ma_nv = employee.ma_nv,
                        so_ngay_nghi = (employee.so_ngay_nghi == null ? 0 : (int)employee.so_ngay_nghi).ToString(),
                    };
                    if (employeeType != null)
                    {
                        viewEmployee.tien_luong = employeeType.tien_luong;
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
            if (AddEmployeeHovaten == null)
            {
                MessageBox.Show("Ten khong duoc de trong!!!");
                return;
            }
            if(AddEmployeeSdt == null)
            {
                MessageBox.Show("So dien thoai khong duoc de trong!!!");
                return;
            }
            if(AddEmployeeNgaySinh == null)
            {
                MessageBox.Show("Ngay sinh khong duoc de trong!!!");
                return;
            }
            if(AddEmployeeSdt.All(char.IsDigit)==false)
            {
                MessageBox.Show("So dien thoai khong hop le !!! ");
                return;
            }
            if(AddEmployeeNgaySinh.Year < 1900)
            {
                MessageBox.Show("Ngay vao lam khong hop le !!! ");
                return;
            }
            if ( DateTime.Now.Year - AddEmployeeNgaySinh.Year < 18)
            {
                MessageBox.Show("Chua du tuoi!!! ");
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
                ho_va_ten = addEmployeeHovaten,
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
            if (SelectedNhanVien != null)
            {
                var delEmployee = DataProvider.Ins.DB.NhanViens.
                  Where(employee => employee.ma_nv == SelectedNhanVien.ma_nv).FirstOrDefault();
                List<CaLamPartTime> LCalam = DataProvider.Ins.DB.CaLamPartTimes.Where(t => t.ma_nv == delEmployee.ma_nv).ToList();
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
        void updateEmployee(object p)
        {
            if (SelectedNhanVien != null)
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
                if (SelectedNhanVien.ngay_vao_lam == null)
                {
                    MessageBox.Show("Ngay vao lam khong duoc de trong!!!");
                    return;
                }
                if(SelectedNhanVien.ngay_vao_lam.Year < 2000)
                {
                    MessageBox.Show("Ngay vao lam khong hop le!!!");
                    return;
                }    
                var updEmployee = DataProvider.Ins.DB.NhanViens.
                  Where(t => t.ma_nv == preMaNhanVien).FirstOrDefault();

                updEmployee.ho_va_ten = SelectedNhanVien.ho_ten;
                updEmployee.dia_chi = SelectedNhanVien.dia_chi;
                updEmployee.sdt = SelectedNhanVien.sdt;
                updEmployee.ngay_vao_lam = SelectedNhanVien.ngay_vao_lam;
                updEmployee.ma_loai_nhan_vien = SelectedNhanVien.ma_loai_nhan_vien;
                DataProvider.Ins.DB.SaveChanges();
                loadDataEmployee();
            }
        }
        void openUpdateEmployee(object p)
        {
            if (SelectedNhanVien != null)
            {
                var window = new UpdateEmployeeWindow();
                preMaNhanVien = SelectedNhanVien.ma_nv;
                window.ShowDialog();
            }
        }
        void openSalaryWindow(object p )
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
            Luong check = DataProvider.Ins.DB.Luongs.
                Where(p => p.ngay_tinh_luong.Value.Year == now.Year && p.ngay_tinh_luong.Value.Month == now.Month).FirstOrDefault();
            if (check == null)
            {
                Luong Sal = new Luong()
                {
                    ngay_tinh_luong = new DateTime(now.Year, now.Month, 1),
                    tong_tien_luong = totalSalary,
                };
                DataProvider.Ins.DB.Luongs.Add(Sal);
                DataProvider.Ins.DB.SaveChanges();
            }
            else
            {
                check.tong_tien_luong = totalSalary;
                DataProvider.Ins.DB.SaveChanges();

            }
        }
        void refreshData()
        {
            List<NhanVien> list = DataProvider.Ins.DB.NhanViens.ToList();
            for(int i=0;i<list.Count;i++)
            {
                list[i].so_ngay_nghi = 0;
            }
            DataProvider.Ins.DB.SaveChanges();
            loadData();
        }
        ObservableCollection<ViewEmployee> solveSalary()
        {
            int salary_per_day;
            totalSalary = 0;
            int month = DateTime.Today.Month;
            int year = DateTime.Today.Year;
            int days = DateTime.DaysInMonth(year, month ) ;
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
                viewE.ma_nv = list[i].ma_nv;
                viewE.ho_ten = list[i].ho_va_ten;
                viewE.ngay_vao_lam = list[i].ngay_vao_lam;
                viewE.Sngay_vao_lam = String.Format("{0:dd/MM/yyyy}", list[i].ngay_vao_lam);  // "03/09/2008"
                viewE.tien_luong = lnv.tien_luong;
                if (maloainv == 1)
                {
                    DateTime now = DateTime.Now;
                    int manv = list[i].ma_nv;
                 
                    CaLamPartTime calam = DataProvider.Ins.DB.CaLamPartTimes.
                        Where(t => t.ma_nv == manv &&
                                   now.Year == t.ngay_lam.Value.Year &&
                                    now.Month == t.ngay_lam.Value.Month).FirstOrDefault();
                    if(calam !=null)
                    {
                        viewE.so_gio_lam =  calam.so_gio_lam.ToString();
                        viewE.luong_nhan = calam.so_gio_lam * lnv.tien_luong;
                    }
                    else
                    {
                        viewE.so_gio_lam = "0";
                        viewE.luong_nhan = 0;
                    }

                }
                else
                {
                    viewE.so_gio_lam = null;
                    viewE.so_ngay_nghi = list[i].so_ngay_nghi.ToString();
                    if (viewE.ngay_vao_lam.Month == month)
                    {
                        int workdays = (int)(selectedDate - viewE.ngay_vao_lam).TotalDays + 1;
                        viewE.luong_nhan = lnv.tien_luong - salary_per_day * (days - workdays + list[i].so_ngay_nghi);
                    }
                    else
                    {
                        if (list[i].so_ngay_nghi >= days)
                        {
                            viewE.luong_nhan = 0;
                        }
                        viewE.luong_nhan = (lnv.tien_luong - salary_per_day * list[i].so_ngay_nghi);
                    }
                }
                totalSalary += (decimal)viewE.luong_nhan;
                res.Add(viewE);
            }
            return res;
        }
        void increaseAbsentDay(object p)
        {
            if (SelectedNhanVien != null)
            {
                NhanVien upd = DataProvider.Ins.DB.NhanViens.Where(t => t.ma_nv == SelectedNhanVien.ma_nv).FirstOrDefault();
                if(upd.ma_loai_nhan_vien == 1)
                {
                    MessageBox.Show("Day la nhan vien part time");
                    return;
                }
                upd.so_ngay_nghi++;
                DataProvider.Ins.DB.SaveChanges();
                loadDataEmployee();
            }
        }
        void decreaseAbsentDay(object p)
        {
            if (SelectedNhanVien != null)
            {
                NhanVien upd = DataProvider.Ins.DB.NhanViens.Where(t => t.ma_nv == SelectedNhanVien.ma_nv).FirstOrDefault();
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
