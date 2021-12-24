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
using MaterialDesignThemes.Wpf;
using System.Windows.Controls;

namespace CoffeeStoreManager.ViewModels
{
    class EmployeeTypeViewModel : BaseViewModel
    {
        
        private ObservableCollection<ViewTypeEmployee> typeEmployeeList;
        private string textTypeNameEmployee;
        private long salary;
        private ViewTypeEmployee selectedLoaiNhanVien;
        private SnackbarMessageQueue myMessageQueue;

        public SnackbarMessageQueue MyMessageQueue { get => myMessageQueue; set { myMessageQueue = value; OnPropertyChanged(nameof(MyMessageQueue)); } }
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
                    Salary = (long)SelectedLoaiNhanVien.tien_luong;
                    TextTypeNameEmployee = SelectedLoaiNhanVien.ten_loai_nhan_vien;
                }
            }
        }
        public long Salary
        {
            get { return salary; }
            set
            {
                salary = value;
                OnPropertyChanged(nameof(salary));
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
            AddType = new RelayCommand<StackPanel>((p) => { return true; }, (p) => { addType(p); });
            UpdateType = new RelayCommand<StackPanel>((p) => { return true; }, (p) => { updateType(p); });
            DeleteType = new RelayCommand<object>((p) => { return true; }, (p) => { deleteType(p); });

            MyMessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(4000));
            MyMessageQueue.DiscardDuplicates = true;

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
                view.tien_luong = (decimal)list[i].tien_luong;
                Obs.Add(view);
            }
            return Obs;
        }
        void addType(StackPanel p)
        {
            if (Validator.IsValid(p))
            {
              
                DataProvider.Ins.DB.LoaiNhanViens.Add(new LoaiNhanVien() { ten_loai_nhan_vien = TextTypeNameEmployee, tien_luong = Salary });
                try
                {
                    DataProvider.Ins.DB.SaveChanges();
                }
                catch
                {
                    return;
                }
                LoadData();
                Salary = 0;
                TextTypeNameEmployee = "";
                this.MyMessageQueue.Enqueue("Thêm thành công!");
            }
            else
            {
                this.MyMessageQueue.Enqueue("Lỗi. thông tin không hợp lệ");
            }
        }
        void updateType(StackPanel p)
        {

            if (Validator.IsValid(p))
            {
               
                if (SelectedLoaiNhanVien == null)
                {
                    this.MyMessageQueue.Enqueue("Lỗi. Vui lòng chọn 1 nhân viên");
                    return;
                }
                var UpdTypeEmployee = DataProvider.Ins.DB.LoaiNhanViens.
                    Where(t => t.ma_loai_nhan_vien == SelectedLoaiNhanVien.ma_loai_nhan_vien).FirstOrDefault();
                if (SelectedLoaiNhanVien.ma_loai_nhan_vien != 1)
                {
                    UpdTypeEmployee.ten_loai_nhan_vien = TextTypeNameEmployee;
                }
                UpdTypeEmployee.tien_luong = Salary;
                try
                {
                    DataProvider.Ins.DB.SaveChanges();
                }
                catch
                {
                    this.MyMessageQueue.Enqueue("Lỗi. Số tiền quá lớn");
                    return;
                }
                LoadData();
                Salary = 0;
                TextTypeNameEmployee = "";
                this.MyMessageQueue.Enqueue("Sửa thông tin thành công!");
            }
            else
            {
                this.MyMessageQueue.Enqueue("Lỗi. Thông tin không hợp lệ");
            }
        }
        void deleteType(object p)
        {
            if (SelectedLoaiNhanVien != null)
            {
                List<NhanVien> ListEmployee = DataProvider.Ins.DB.NhanViens.Where(t => t.ma_loai_nhan_vien == SelectedLoaiNhanVien.ma_loai_nhan_vien).ToList();
                if (SelectedLoaiNhanVien.ma_loai_nhan_vien == 1) //Ma nv part-time = 1
                {
                    this.MyMessageQueue.Enqueue("Lỗi. không thể xóa loại nhân viên này");
                    return;
                }
                else
                {
                    if (ListEmployee.Count != 0)
                    {
                        this.MyMessageQueue.Enqueue("Lỗi. không thể xóa vì loại nhân viên đang được sử dụng");
                        return;
                    }
                    var ClrTypeEmployee = DataProvider.Ins.DB.LoaiNhanViens.
                       Where(t => t.ma_loai_nhan_vien == SelectedLoaiNhanVien.ma_loai_nhan_vien).FirstOrDefault();
                    for (int i = 0; i < ListEmployee.Count; i++)
                    {
                        if (ListEmployee[i].ma_loai_nhan_vien == ClrTypeEmployee.ma_loai_nhan_vien)
                        {
                            ListEmployee[i].LoaiNhanVien.tien_luong = 0;
                            DataProvider.Ins.DB.NhanViens.Remove(ListEmployee[i]);
                        }
                    }
                    DataProvider.Ins.DB.LoaiNhanViens.Remove(ClrTypeEmployee);
                    DataProvider.Ins.DB.SaveChanges();
                    LoadData();
                    this.MyMessageQueue.Enqueue("Xóa thành công");
                }
            }
        }
    }
}

