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
        public Nullable<int> ma_nv { get; set; }
        public Nullable<System.DateTime> ngay_lam { get; set; }
        public Nullable<System.TimeSpan> gio_bat_dau { get; set; }
        public Nullable<System.TimeSpan> gio_ket_thuc { get; set; }
        public Nullable<int> so_gio_lam { get; set; }
    
        public virtual NhanVien NhanVien { get; set; }
    }
}
