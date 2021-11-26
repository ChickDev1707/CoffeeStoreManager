using System;
namespace CoffeeStoreManager.Models
{
    class ViewFood
    {
        public int STT { get; set; }
        public int ma_loai_mon_an { get; set; }
        public int ma_mon_an { get; set; }
        public string ten_mon_an { get; set; }
        public Nullable<decimal> gia_tien { get; set; }
        public string loai_mon_an { get; set; }

        public string nguyen_lieu { get; set; }
        public string mo_ta { get; set; }
        public byte[] anh { get; set; }

    }
}
