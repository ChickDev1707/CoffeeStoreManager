using CoffeeStoreManager.Models;
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
    class EmployeeTypeViewModel : BaseViewModel
    {
        private ObservableCollection<LoaiNhanVien> typeEmployeeList;
        private string textTypeNameEmployee;
        private string textSalary;
        private LoaiNhanVien selectedLoaiNhanVien;
        public ObservableCollection<LoaiNhanVien> TypeEmployeeList { get => typeEmployeeList; set { typeEmployeeList = value; OnPropertyChanged(nameof(typeEmployeeList)); } }
        public LoaiNhanVien SelectedLoaiNhanVien
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
            TypeEmployeeList = new ObservableCollection<LoaiNhanVien>();
            LoadData();
            AddType = new RelayCommand<object>((p) => { return true; }, (p) => { addType(p); });
            UpdateType = new RelayCommand<object>((p) => { return true; }, (p) => { updateType(p); });
            DeleteType = new RelayCommand<object>((p) => { return true; }, (p) => { deleteType(p); });
        }
        void LoadData()
        {
            TypeEmployeeList.Clear();
            List<LoaiNhanVien> employeelist = DataProvider.Ins.DB.LoaiNhanViens.ToList();
            for (int i = 0; i < employeelist.Count; i++)
            {
                TypeEmployeeList.Add(employeelist[i]);
            }
        }
        void addType(object p)
        {
            decimal check;
            if(decimal.TryParse(TextSalary, out check) ==false)
            {
                MessageBox.Show("Tien luong khong hop le");
                return;
            }
            DataProvider.Ins.DB.LoaiNhanViens.Add(new LoaiNhanVien() { ten_loai_nhan_vien = TextTypeNameEmployee, tien_luong = decimal.Parse(TextSalary) });
            DataProvider.Ins.DB.SaveChanges();
            LoadData();
        }
        void updateType(object p)
        {
            if (SelectedLoaiNhanVien != null)
            {
                decimal check;
                if(decimal.TryParse(TextSalary, out check)==false)
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
                UpdTypeEmployee.tien_luong = decimal.Parse(TextSalary);
                DataProvider.Ins.DB.SaveChanges();
                LoadData();
            }
        }
        void deleteType(object p)
        {
            if (SelectedLoaiNhanVien != null)
            {
                if(SelectedLoaiNhanVien.ma_loai_nhan_vien == 1) //Ma nv part-time = 1
                {
                    MessageBox.Show("Khong the xoa loai nhan vien nay!!!");
                    return;
                }
                var ClrTypeEmployee = DataProvider.Ins.DB.LoaiNhanViens.
                   Where(t => t.ma_loai_nhan_vien == SelectedLoaiNhanVien.ma_loai_nhan_vien).FirstOrDefault();
                var ListEmployee = DataProvider.Ins.DB.NhanViens.ToList();
                for (int i = 0; i < ListEmployee.Count; i++)
                {
                    if(ListEmployee[i].ma_loai_nhan_vien == ClrTypeEmployee.ma_loai_nhan_vien)
                    {
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
