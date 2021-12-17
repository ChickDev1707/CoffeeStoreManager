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
    public class UpdateEmployeeViewModel:BaseViewModel
    {
        EmployeeViewModel employeeVM;
        private ViewEmployee selectedEmployee;
        private List<LoaiNhanVien> employeeTypeList;
        public ViewEmployee SelectedEmployee { get => selectedEmployee; set { selectedEmployee = value; OnPropertyChanged(nameof(selectedEmployee)); } }

        public List<LoaiNhanVien> EmployeeTypeList { get => employeeTypeList; set { employeeTypeList = value; OnPropertyChanged(nameof(employeeTypeList)); } }
        public ICommand UpdateEmployee { get; set; }
        public UpdateEmployeeViewModel(EmployeeViewModel vm)
        {
            employeeVM = vm;
            SelectedEmployee = new ViewEmployee(vm.SelectedEmployee); 
            EmployeeTypeList = vm.EmployeeTypeList;
            UpdateEmployee = new RelayCommand<object>((p) => { return true; }, (p) => { updateEmployee(p); });
        }
        void updateEmployee(object p)
        {
            #region Check
            if (SelectedEmployee.ho_ten == null)
            {
                MessageBox.Show("Ten khong duoc de trong!!!");
                return;
            }
            if (SelectedEmployee.sdt.All(char.IsDigit) == false)
            {
                MessageBox.Show("So dien thoai khong hop le !!! ");
                return;
            }
            else
            {
                for (int i = 0; i < employeeVM.EmployeeList.Count; i++)
                {
                    if (SelectedEmployee.sdt == employeeVM.EmployeeList[i].sdt && SelectedEmployee.ma_nv != employeeVM.EmployeeList[i].ma_nv)
                    {
                        MessageBox.Show("So dien thoai bi trung!!!");
                        return;
                    }
                }
            }
            if (SelectedEmployee.ngay_vao_lam == null)
            {
                MessageBox.Show("Ngay vao lam khong duoc de trong!!!");
                return;
            }
            if (SelectedEmployee.ngay_vao_lam.Year < 2000)
            {
                MessageBox.Show("Ngay vao lam khong hop le!!!");
                return;
            }
            #endregion
            var updEmployee = DataProvider.Ins.DB.NhanViens.
              Where(t => t.ma_nhan_vien == SelectedEmployee.ma_nv).FirstOrDefault();
            updEmployee.ho_ten = SelectedEmployee.ho_ten;
            updEmployee.dia_chi = SelectedEmployee.dia_chi;
            updEmployee.sdt = SelectedEmployee.sdt;
            updEmployee.ngay_vao_lam = SelectedEmployee.ngay_vao_lam;
            updEmployee.ma_loai_nhan_vien = SelectedEmployee.ma_loai_nhan_vien;
            DataProvider.Ins.DB.SaveChanges();
            employeeVM.loadDataEmployee();
        }
    }
}
