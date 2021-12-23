using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CoffeeStoreManager.Models
{
    public class ViewEmployee
    {
        public int STT { get; set; }
        public int ma_nv { get; set; }
        public string ho_ten { get; set; }
        public DateTime ngay_vao_lam { get; set; }
        public DateTime ngay_sinh { get; set; }

        //set format cho ngay vao lam de hien thi
        public string Sngay_vao_lam { get; set; }
        public bool check_selected_item { get; set; }
        //
        public Visibility VisiblePartTime { get; set; }
        public string tien_luong { get; set; }
        public string luong_nhan { get; set; }
        public int so_ngay_nghi { get; set; }
        public string so_gio_lam { get; set; }
        public string sdt { get; set; }
        public string dia_chi { get; set; }
        public string loai_nhan_vien { get; set; }
        public int ma_loai_nhan_vien { get; set; }
        public ViewEmployee()
        {

        }
        public ViewEmployee(ViewEmployee b)
        {
            ngay_sinh = b.ngay_sinh;
            STT = b.STT;
            ma_nv = b.ma_nv;
            ma_loai_nhan_vien = b.ma_loai_nhan_vien;
            ho_ten = b.ho_ten;
            dia_chi = b.dia_chi;
            sdt = b.sdt;
            ngay_vao_lam = b.ngay_vao_lam;
            Sngay_vao_lam = b.Sngay_vao_lam;
            check_selected_item = b.check_selected_item;
            VisiblePartTime = b.VisiblePartTime;
            tien_luong = b.tien_luong;
            luong_nhan = b.luong_nhan;
            so_ngay_nghi = b.so_ngay_nghi;
            so_gio_lam = b.so_gio_lam;
            loai_nhan_vien = b.loai_nhan_vien;
        }
    }
}