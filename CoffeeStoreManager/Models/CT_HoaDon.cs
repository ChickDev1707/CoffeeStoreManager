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
    
    public partial class CT_HoaDon
    {
        public int ma_hoa_don { get; set; }
        public int ma_mon_an { get; set; }
        public Nullable<int> so_luong { get; set; }
        public Nullable<decimal> thanh_tien { get; set; }
    
        public virtual HoaDon HoaDon { get; set; }
        public virtual MonAn MonAn { get; set; }
    }
}
