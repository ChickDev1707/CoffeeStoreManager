using System;
using System.Collections.Generic;


namespace CoffeeStoreManager.Models
{
    class ViewSource
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ViewSource()
        {
            this.CT_PhieuNhapHang = new HashSet<CT_PhieuNhapHang>();
        }

        public int ma_phieu_nhap_hang { get; set; }
        public Nullable<System.DateTime> ngay_nhap { get; set; }
        public string nha_cung_cap { get; set; }

        public decimal tong_tien { get; set; }
        public int so_luong_mat_hang { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CT_PhieuNhapHang> CT_PhieuNhapHang { get; set; }
    }
}
