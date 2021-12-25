using CoffeeStoreManager.Models;
using CoffeeStoreManager.Resources.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CoffeeStoreManager.ViewModels
{
    public class AddEmployeeViewModel:BaseViewModel
    {
        private EmployeeViewModel employeeVM;
        private string addEmployeeSdt;
        private int addEmployeeMaloainhanvien;
        private string addEmployeeHovaten;
        private string addEmployeeDiachi;
        private DateTime addEmployeeNgayvaolam;
        private DateTime addEmployeeNgaySinh;
        private List<LoaiNhanVien> employeeTypeList;
        public List<LoaiNhanVien> EmployeeTypeList { get => employeeTypeList; set { employeeTypeList = value; OnPropertyChanged(nameof(employeeTypeList)); } }
        public string AddEmployeeSdt { get => addEmployeeSdt; set { addEmployeeSdt = value; OnPropertyChanged(nameof(addEmployeeSdt)); } }
        public int AddEmployeeMaloainhanvien { get => addEmployeeMaloainhanvien; set { addEmployeeMaloainhanvien = value; OnPropertyChanged(nameof(addEmployeeMaloainhanvien)); } }
        public string AddEmployeeHovaten { get => addEmployeeHovaten; set { addEmployeeHovaten = value; OnPropertyChanged(nameof(addEmployeeHovaten)); } }
        public string AddEmployeeDiachi { get => addEmployeeDiachi; set { addEmployeeDiachi = value; OnPropertyChanged(nameof(addEmployeeDiachi)); } }
        public DateTime AddEmployeeNgayvaolam { get => addEmployeeNgayvaolam; set { addEmployeeNgayvaolam = value; OnPropertyChanged(nameof(addEmployeeNgayvaolam)); } }
        public DateTime AddEmployeeNgaySinh { get => addEmployeeNgaySinh; set { addEmployeeNgaySinh = value; OnPropertyChanged(nameof(addEmployeeNgaySinh)); } }

        public ICommand AddEmployee { get; set; }
        public AddEmployeeViewModel(EmployeeViewModel VM)
        {
            employeeVM = VM;
            employeeVM.loadDataEmployee();
            employeeTypeList = VM.EmployeeTypeList;
            AddEmployee = new RelayCommand<StackPanel>((p) => { return true; }, (p) => { addEmployee(p); });
        }
        void addEmployee(StackPanel p)
        {
            if (Validator.IsValid(p))
            {
                NhanVien add = new NhanVien()
                {
                    ho_ten = AddEmployeeHovaten,
                    ngay_vao_lam = AddEmployeeNgayvaolam,
                    sdt = AddEmployeeSdt,
                    dia_chi = AddEmployeeDiachi,
                    ma_loai_nhan_vien = AddEmployeeMaloainhanvien,
                    so_ngay_nghi = 0,
                    ngay_sinh = AddEmployeeNgaySinh,
                };
                DataProvider.Ins.DB.NhanViens.Add(add);
                try
                {
                    DataProvider.Ins.DB.SaveChanges();
                }
                catch
                {
                    employeeVM.MyMessageQueue.Enqueue("Lỗi. Thông tin nhân viên không hợp lệ");
                    return;
                }
              
                employeeVM.loadDataEmployee();
                clearAddEmployeeForm();
                employeeVM.MyMessageQueue.Enqueue("Thêm nhân viên thành công!");
            }
            else
            {
                employeeVM.MyMessageQueue.Enqueue("Lỗi. Thông tin nhân viên không hợp lệ");
            }
        }
        void clearAddEmployeeForm()
        {
            AddEmployeeHovaten = "";
            AddEmployeeSdt = "";
            AddEmployeeDiachi = "";
        }
    }
}
