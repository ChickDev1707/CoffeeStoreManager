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
            UpdateEmployee = new RelayCommand<StackPanel>((p) => { return true; }, (p) => { updateEmployee(p); });
        }
        void updateEmployee(StackPanel p)
        {
            if (Validator.IsValid(p))
            {
                var updEmployee = DataProvider.Ins.DB.NhanViens.
                  Where(t => t.ma_nhan_vien == SelectedEmployee.ma_nv).FirstOrDefault();
                updEmployee.ho_ten = SelectedEmployee.ho_ten;
                updEmployee.dia_chi = SelectedEmployee.dia_chi;
                updEmployee.sdt = SelectedEmployee.sdt;
                updEmployee.ngay_vao_lam = SelectedEmployee.ngay_vao_lam;
                updEmployee.ma_loai_nhan_vien = SelectedEmployee.ma_loai_nhan_vien;
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
                employeeVM.MyMessageQueue.Enqueue("Sửa thông tin thành công!");
            }
            else
            {
                employeeVM.MyMessageQueue.Enqueue("Lỗi. Thông tin nhân viên không hợp lệ");
            }
        }
    }
}
