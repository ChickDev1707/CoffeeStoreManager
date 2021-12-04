using CoffeeStoreManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            AddEmployee = new RelayCommand<Window>((p) => { return true; }, (p) => { addEmployee(p); });
        }
        void addEmployee(Window p)
        {
        
            QuyDinh QDinh = DataProvider.Ins.DB.QuyDinhs.Select(t => t).FirstOrDefault();
            #region Check
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
                for (int i = 0; i < employeeVM.EmployeeList.Count; i++)
                {
                    if (AddEmployeeSdt == employeeVM.EmployeeList[i].sdt)
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
            #endregion add 

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
            employeeVM.loadDataEmployee();
            clearAddEmployeeForm();
            p.Close();
        }
        void clearAddEmployeeForm()
        {
            AddEmployeeHovaten = "";
            AddEmployeeSdt = "";
            AddEmployeeDiachi = "";
        }
    }
}
