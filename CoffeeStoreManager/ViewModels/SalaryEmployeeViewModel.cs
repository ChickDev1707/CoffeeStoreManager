using CoffeeStoreManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CoffeeStoreManager.Resources.Utils;
namespace CoffeeStoreManager.ViewModels
{
    public class SalaryEmployeeViewModel:BaseViewModel
    {
        private decimal totalSalary;
        private ObservableCollection<ViewEmployee> salaryEmployeeList;
        public ObservableCollection<ViewEmployee> SalaryEmployeeList
        {
            get
            {
                return salaryEmployeeList;
            }
            set
            {
                salaryEmployeeList = value;
                OnPropertyChanged(nameof(salaryEmployeeList));
            }
        }
        public SalaryEmployeeViewModel()
        {
            SalaryEmployeeList = new ObservableCollection<ViewEmployee>();
            loadSalaryEmployeeList();
        }
       void loadSalaryEmployeeList()
        {
            SalaryEmployeeList.Clear();
            SalaryEmployeeList = solveSalary();
            SaveDataSalary();
            refreshData();
        }
        void refreshData()
        {
            List<NhanVien> list = DataProvider.Ins.DB.NhanViens.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                list[i].so_ngay_nghi = 0;
            }
            DataProvider.Ins.DB.SaveChanges();
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
                salary_per_day = (decimal)lnv.tien_luong / days;
                ViewEmployee viewE = new ViewEmployee();
                viewE.loai_nhan_vien = lnv.ten_loai_nhan_vien;
                viewE.ma_nv = list[i].ma_nhan_vien;
                viewE.ho_ten = list[i].ho_ten;
                viewE.ngay_vao_lam = list[i].ngay_vao_lam;
                viewE.Sngay_vao_lam = String.Format("{0:dd/MM/yyyy}", list[i].ngay_vao_lam);
                viewE.tien_luong = MoneyConverter.convertMoney(lnv.tien_luong.ToString());
                if (maloainv == 1)
                {
                    viewE.VisiblePartTime = System.Windows.Visibility.Hidden;
                    DateTime now = DateTime.Now;
                    int manv = list[i].ma_nhan_vien;
                    
                    CaLamPartTime calam = DataProvider.Ins.DB.CaLamPartTimes.
                        Where(t => t.ma_nhan_vien == manv &&
                                   now.Year == t.ngay_lam.Value.Year &&
                                    now.Month == t.ngay_lam.Value.Month).FirstOrDefault();
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
                    viewE.VisiblePartTime = System.Windows.Visibility.Visible;
                    viewE.so_gio_lam = null;
                    viewE.so_ngay_nghi = (int)list[i].so_ngay_nghi;
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
                        viewE.luong_nhan = (lnv.tien_luong - (salary_per_day * list[i].so_ngay_nghi)).ToString();
                    }
                }

                totalSalary += Decimal.Parse(viewE.luong_nhan);
                viewE.luong_nhan = MoneyConverter.convertMoney(viewE.luong_nhan);
                res.Add(viewE);
            }
            return res;
        }
    }

}
