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
using CoffeeStoreManager.Resources.Utils;
namespace CoffeeStoreManager.ViewModels
{
    public class EmployeeViewModel : BaseViewModel
    {
        private ObservableCollection<ViewEmployee> employeeList;
        private List<LoaiNhanVien> employeeTypeList;
        private int preMaNhanVien;
        private bool isSelectedTab;
        private ViewEmployee selectedNhanVien;
        private List<string> dataCbxYear;

        //Su dung trong ham tim kiem nhan vien
        private ObservableCollection<ViewEmployee> employeeListContainer;
        public bool IsSelectedTab { get => isSelectedTab; set { isSelectedTab = value; OnPropertyChanged(nameof(isSelectedTab)); loadData(); } }
        public ViewEmployee SelectedNhanVien { get => selectedNhanVien; set { selectedNhanVien = value; OnPropertyChanged(nameof(selectedNhanVien)); } }

        public ObservableCollection<ViewEmployee> EmployeeList { get => employeeList; set { employeeList = value; OnPropertyChanged(nameof(employeeList)); } }

        public List<LoaiNhanVien> EmployeeTypeList { get => employeeTypeList; set { employeeTypeList = value; OnPropertyChanged(nameof(employeeTypeList)); } }
        public List<string> DataCbxMonth { get; set; }
        public List<string> DataCbxYear
        {
            get => dataCbxYear; set { dataCbxYear = value; OnPropertyChanged(nameof(dataCbxYear)); }
        }
        #region Command
        public ICommand Search { get; set; }
        public ICommand AddEmployee { get; set; }
        public ICommand IncAbsentDay { get; set; }
        public ICommand DecAbsentDay { get; set; }
        public ICommand OpenAddEmployee { get; set; }
        public ICommand DeleteEmployee { get; set; }
        public ICommand UpdateEmployee { get; set; }
        public ICommand OpenUpdateEmployee { get; set; }
        public ICommand OpenSalaryWindow { get; set; }
        public ICommand CheckItem { get; set; }
        public ICommand Find { get; set; }
        #endregion

        public EmployeeViewModel()
        {
            employeeList = new ObservableCollection<ViewEmployee>();
            employeeListContainer = new ObservableCollection<ViewEmployee>();
            EmployeeTypeList = new List<LoaiNhanVien>();
            //AddEmployee = new RelayCommand<Window>((p) => { return true; }, (p) => { addEmployee(p); });
            OpenAddEmployee = new RelayCommand<Window>((p) => { return true; }, (p) => { openAddEmployee(p); });
            DeleteEmployee = new RelayCommand<object>((p) => { return true; }, (p) => { deleteEmployee(p); });
            UpdateEmployee = new RelayCommand<object>((p) => { return true; }, (p) => { updateEmployee(p); });
            OpenUpdateEmployee = new RelayCommand<object>((p) => { return true; }, (p) => { openUpdateEmployee(p); });
            OpenSalaryWindow = new RelayCommand<object>((p) => { return true; }, (p) => { openSalaryWindow(p); });
            IncAbsentDay = new RelayCommand<object>((p) => { return true; }, (p) => { increaseAbsentDay(p); });
            DecAbsentDay = new RelayCommand<object>((p) => { return true; }, (p) => { decreaseAbsentDay(p); });
            CheckItem = new RelayCommand<ListView>((p) => { return true; }, (p) => { checkSelectItem(p); });
            Find = new RelayCommand<TextBox>((p) => { return true; }, (p) => { findItem(p); });
            loadData();
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
        public void loadDataEmployee()
        {
            EmployeeTypeList.Clear();
            List<NhanVien> lNv = DataProvider.Ins.DB.NhanViens.ToList();
            EmployeeList = getViewEmployeeFromList(lNv);
            loadDataCbxEmployeeType();
            employeeListContainer = getViewEmployeeFromList(lNv);
        }
        public void loadDataTypeEmployee(object p)
        {
            EmployeeTypeList.Clear();
            List<NhanVien> lNv = DataProvider.Ins.DB.NhanViens.ToList();
            EmployeeList = getViewEmployeeFromList(lNv);
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
                        viewEmployee.tien_luong = MoneyConverter.convertMoney(viewEmployee.tien_luong);
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
        void openAddEmployee(Window p)
        {
            var a = new AddEmployeeWindow(this);
            a.ShowDialog();
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
            #region Check
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
                    if (SelectedNhanVien.sdt == EmployeeList[i].sdt)
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
            #endregion
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
            SalaryWindow Swindow = new SalaryWindow();
            Swindow.ShowDialog();
            loadData();
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
