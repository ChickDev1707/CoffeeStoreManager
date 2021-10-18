using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStoreManager.Models
{
    class ViewFood
    {
        public int STT { get; set; }
        public int ma_loai_mon_an { get; set; }
        public string ten_mon_an { get; set; }
        public Nullable<decimal> gia_tien { get; set; }
        public string loai_mon_an { get; set; }
    }
}
