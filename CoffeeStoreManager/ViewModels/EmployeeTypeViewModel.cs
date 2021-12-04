using CoffeeStoreManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CoffeeStoreManager.Resources.Utils;
namespace CoffeeStoreManager.ViewModels
{
    class EmployeeTypeViewModel : BaseViewModel
    {
        
        private ObservableCollection<ViewTypeEmployee> typeEmployeeList;
        private string textTypeNameEmployee;
        private string textSalary;
        private ViewTypeEmployee selectedLoaiNhanVien;
        public ObservableCollection<ViewTypeEmployee> TypeEmployeeList { get => typeEmployeeList; set { typeEmployeeList = value; OnPropertyChanged(nameof(typeEmployeeList)); } }
        public ViewTypeEmployee SelectedLoaiNhanVien
        {
            get { return selectedLoaiNhanVien; }
            set
            {
                selectedLoaiNhanVien = value;
                OnPropertyChanged(nameof(selectedLoaiNhanVien));
                if (SelectedLoaiNhanVien != null)
                {
                    TextSalary = SelectedLoaiNhanVien.tien_luong.ToString();
                    TextTypeNameEmployee = SelectedLoaiNhanVien.ten_loai_nhan_vien;
                }
            }
        }
        public string TextSalary
        {
            get { return textSalary; }
            set
            {
                textSalary = value;
                OnPropertyChanged(nameof(textSalary));
            }
        }
        public string TextTypeNameEmployee
        {
            get { return textTypeNameEmployee; }
            set
            {
                textTypeNameEmployee = value;
                OnPropertyChanged(nameof(textTypeNameEmployee));
            }
        }
        public ICommand AddType { get; set; }
        public ICommand UpdateType { get; set; }
        public ICommand DeleteType { get; set; }


        public EmployeeTypeViewModel()
        {
            TypeEmployeeList = new ObservableCollection<ViewTypeEmployee>();
            LoadData();
            AddType = new RelayCommand<object>((p) => { return true; }, (p) => { addType(p); });
            UpdateType = new RelayCommand<object>((p) => { return true; }, (p) => { updateType(p); });
            DeleteType = new RelayCommand<object>((p) => { return true; }, (p) => { deleteType(p); });
        }
        void LoadData()
        {
            TypeEmployeeList.Clear();
            List<LoaiNhanVien> employeelist = DataProvider.Ins.DB.LoaiNhanViens.ToList();
            TypeEmployeeList = getObsTypeEmployee(employeelist);
        }
        ObservableCollection<ViewTypeEmployee> getObsTypeEmployee(List<LoaiNhanVien> list)
        {
            ObservableCollection<ViewTypeEmployee> Obs = new ObservableCollection<ViewTypeEmployee>();
            for (int i = 0; i < list.Count; i++)
            {
                ViewTypeEmployee view = new ViewTypeEmployee();
                view.ma_loai_nhan_vien = list[i].ma_loai_nhan_vien;
                view.ten_loai_nhan_vien = list[i].ten_loai_nhan_vien;
                view.tien_luong = MoneyConverter.convertMoney(list[i].tien_luong.ToString());
                Obs.Add(view);
            }
            return Obs;
        }
        void addType(object p)
        {
            decimal check;
            string[] money = TextSalary.Split('.');
            string salary = "";
            for (int i = 0; i < money.Length; i++)
            {
                salary += money[i];
            }
            if (decimal.TryParse(salary, out check) == false)
            {
                MessageBox.Show("Tien luong khong hop le");
                return;
            }
            if (check < 0)
            {
                MessageBox.Show("Tien luong khong hop le");
                return;
            }
            DataProvider.Ins.DB.LoaiNhanViens.Add(new LoaiNhanVien() { ten_loai_nhan_vien = TextTypeNameEmployee, tien_luong = decimal.Parse(salary) });
            try
            {
                DataProvider.Ins.DB.SaveChanges();
            }
            catch
            {
                MessageBox.Show("So tien qua lon");
                return;
            }
            LoadData();
        }
        void updateType(object p)
        {
            if (SelectedLoaiNhanVien != null)
            {
                decimal check;
                string[] money = TextSalary.Split('.');
                string salary = "";
                for (int i = 0; i < money.Length; i++)
                {
                    salary += money[i];
                }
                if (decimal.TryParse(salary, out check) == false)
                {
                    MessageBox.Show("Tien luong khong hop le");
                    return;
                }
                if (check < 0)
                {
                    MessageBox.Show("Tien luong khong hop le");
                    return;
                }
                var UpdTypeEmployee = DataProvider.Ins.DB.LoaiNhanViens.
                    Where(t => t.ma_loai_nhan_vien == SelectedLoaiNhanVien.ma_loai_nhan_vien).FirstOrDefault();
                if (SelectedLoaiNhanVien.ma_loai_nhan_vien != 1)
                {
                    UpdTypeEmployee.ten_loai_nhan_vien = TextTypeNameEmployee;
                }
                UpdTypeEmployee.tien_luong = decimal.Parse(salary);
                try
                {
                    DataProvider.Ins.DB.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("So tien qua lon");
                    return;
                }
                LoadData();
            }
        }
        void deleteType(object p)
        {
            if (SelectedLoaiNhanVien != null)
            {
                if (SelectedLoaiNhanVien.ma_loai_nhan_vien == 1) //Ma nv part-time = 1
                {
                    MessageBox.Show("Khong the xoa loai nhan vien nay!!!");
                    return;
                }
                var ClrTypeEmployee = DataProvider.Ins.DB.LoaiNhanViens.
                   Where(t => t.ma_loai_nhan_vien == SelectedLoaiNhanVien.ma_loai_nhan_vien).FirstOrDefault();
                List<NhanVien> ListEmployee = DataProvider.Ins.DB.NhanViens.ToList();
                for (int i = 0; i < ListEmployee.Count; i++)
                {
                    if (ListEmployee[i].ma_loai_nhan_vien == ClrTypeEmployee.ma_loai_nhan_vien)
                    {
                        ListEmployee[i].LoaiNhanVien.tien_luong =0;
                        DataProvider.Ins.DB.NhanViens.Remove(ListEmployee[i]);
                    }
                }
                DataProvider.Ins.DB.LoaiNhanViens.Remove(ClrTypeEmployee);
                DataProvider.Ins.DB.SaveChanges();
                LoadData();
            }
        }
    }
}

