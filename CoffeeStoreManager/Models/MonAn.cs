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
    
    public partial class MonAn
    {
        public int ma_mon_an { get; set; }
        public Nullable<decimal> gia_tien { get; set; }
        public Nullable<int> ma_loai_mon_an { get; set; }
        public string ten_mon_an { get; set; }
        public string nguyen_lieu { get; set; }
        public string mo_ta { get; set; }
        public byte[] anh { get; set; }
    
        public virtual LoaiMonAn LoaiMonAn { get; set; }
    }
}
