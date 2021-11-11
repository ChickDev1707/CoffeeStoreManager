using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;


namespace CoffeeStoreManager.Models
{
    public class ViewBill
    {
        public int ma_mon_an { get; set; }
        public string ten_mon_an { get; set; }
        public Nullable<decimal> gia_tien { get; set; }
        public Nullable<int> so_luong { get; set; }
        public Nullable<decimal> thanh_tien { get; set; }
    }
    public class Table
    {
        public int tablenumber { get; set; }
        public bool status { get; set; }
        public ObservableCollection<ViewBill> viewbilloftable { get; set; }
        public ObservableCollection<CT_HoaDon> billoftable { get; set; }
    }
}
