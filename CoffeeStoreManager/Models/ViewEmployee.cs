using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStoreManager.Models
{
    class ViewEmployee
    {
        public int STT { get; set; }
        public int ma_nv { get; set; }
        public string ho_ten { get; set; }
        public DateTime ngay_vao_lam { get; set; }
       
        public string Sngay_vao_lam { get; set; } //set format cho ngay vao lam de hien thi
        public Nullable<decimal> tien_luong { get; set; }
        public Nullable<decimal> luong_nhan { get; set; }
        public string so_ngay_nghi { get; set; }
        public string so_gio_lam { get; set; }
        public string sdt { get; set; }
        public string dia_chi { get; set; }
        public string loai_nhan_vien { get; set; }
        public int ma_loai_nhan_vien { get; set; }
    }
}
