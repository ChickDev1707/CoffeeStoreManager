//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CoffeeStoreManager.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CaLamPartTime
    {
        public int ma_ca_partTime { get; set; }
        public Nullable<int> ma_nhan_vien { get; set; }
        public System.DateTime ngay_lam { get; set; }
        public System.TimeSpan gio_ket_thuc { get; set; }
        public Nullable<int> so_gio_lam { get; set; }
        public System.TimeSpan gio_bat_dau { get; set; }
    
        public virtual NhanVien NhanVien { get; set; }

        public static CaLamPartTime fromShift(PartTimeShift shift)
        {
            var hourDiff = shift.Ket_thuc.TimeOfDay.Hours - shift.Bat_dau.TimeOfDay.Hours;
            CaLamPartTime dbShift = new CaLamPartTime()
            {
                ma_ca_partTime = shift.Ma_ca_partTime,
                ma_nhan_vien = shift.Ma_nhan_vien,
                ngay_lam = shift.Bat_dau.Date,
                gio_bat_dau = shift.Bat_dau.TimeOfDay,
                gio_ket_thuc = shift.Ket_thuc.TimeOfDay,
                so_gio_lam = hourDiff
            };
            return dbShift;
        }
    }
}
