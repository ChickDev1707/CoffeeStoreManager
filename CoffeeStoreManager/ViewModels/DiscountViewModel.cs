using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CoffeeStoreManager.Models;
using CoffeeStoreManager.Views;
using System.Collections.ObjectModel;
using System.Windows;
using MaterialDesignThemes.Wpf;
using System.Windows.Threading;

namespace CoffeeStoreManager.ViewModels
{
    public class DiscountViewModel : BaseViewModel
    {
        private ObservableCollection<PhieuUuDai> _DiscountList;
        public ObservableCollection<PhieuUuDai> DiscountList
        {
            get => _DiscountList;
            set
            {
                _DiscountList = value;
                OnPropertyChanged(nameof(DiscountList));
            }
        }
        private string _IdFirst;
        public string IdFirst { get => _IdFirst; set { _IdFirst = value; OnPropertyChanged(nameof(IdFirst)); } }
        private string _IdResult;
        public string IdResult { get => _IdResult; set { _IdResult = value; OnPropertyChanged(nameof(IdResult)); } }
        private string _NumberResult;
        public string NumberResult { get => _NumberResult; set { _NumberResult = value; OnPropertyChanged(nameof(NumberResult)); } }
        private QuyDinh _QuyDinh;
        public QuyDinh QuyDinh
        {
            get => _QuyDinh;
            set
            {
                _QuyDinh = value;
                OnPropertyChanged(nameof(QuyDinh));
            }
        }
        private ObservableCollection<MonAn> _DiscountFood;
        public ObservableCollection<MonAn> DiscountFood
        {
            get => _DiscountFood;
            set
            {
                _DiscountFood = value;
                OnPropertyChanged(nameof(DiscountFood));
            }
        }
        private MonAn _SelectedDiscountFood;
        public MonAn SelectedDiscountFood
        {
            get => _SelectedDiscountFood;
            set
            {
                _SelectedDiscountFood = value;
                OnPropertyChanged(nameof(SelectedDiscountFood));
            }
        }
        private int _numcheck;
        public int numcheck { get => _numcheck; set { _numcheck = value; OnPropertyChanged(nameof(numcheck)); } }
        private int _num;
        public int num { get => _num; set { _num = value; OnPropertyChanged(nameof(num)); } }
        private SnackbarMessageQueue myMessageQueue;
        public SnackbarMessageQueue MyMessageQueue { get => myMessageQueue; set { myMessageQueue = value; OnPropertyChanged(nameof(MyMessageQueue)); } }
        public ICommand FindIDCustomerCommand { get; set; }
        public ICommand AddCustomerCommand { get; set; }
        public ICommand SeleteDiscountCommand { get; set; }
        int FindIDCustomer(string IdFirst)
        {
            int i;
            for (i = 0; i < DiscountList.Count; i++)
            {
                if (DiscountList[i].ma_uu_dai == Convert.ToInt64(IdFirst))
                    return i;
            }
            return -1;
        }
        int CountFood()
        {
            int count = 0;
            foreach (var item in billdetail)
            {
                var food = (from u in DataProvider.Ins.DB.MonAns where u.ma_mon_an == item.ma_mon_an select u).Single();
                if (food.ma_loai_mon_an == QuyDinh.loai_san_pham_uu_dai)
                    count = count + (int)item.so_luong;
            }
            return count;
        }
        void CheckOut(HoaDon bill)
        {
            for (int i = 0; i < billdetail.Count(); i++)
            {
                var item = new CT_HoaDon()
                {
                    ma_hoa_don = bill.ma_hoa_don,
                    ma_mon_an = billdetail[i].ma_mon_an,
                    so_luong = billdetail[i].so_luong,
                    //gia_tien = billdetail[i].gia_tien,
                    //thanh_tien = billdetail[i].so_luong * billdetail[i].gia_tien
                };
                DataProvider.Ins.DB.CT_HoaDon.Add(item);
                DataProvider.Ins.DB.SaveChanges();
            }
            billdetail.Clear();
        }
        public List<CT_HoaDon> billdetail;
        public DiscountViewModel()
        {
            numcheck = 0;
            num = 0;
            IdFirst = "0";
            IdResult = NumberResult = "";
            DiscountList = new ObservableCollection<PhieuUuDai>(DataProvider.Ins.DB.PhieuUuDais);
            QuyDinh = DataProvider.Ins.DB.QuyDinhs.FirstOrDefault();
            DiscountFood = new ObservableCollection<MonAn>();
            var discountfood = new ObservableCollection<MonAn>(DataProvider.Ins.DB.MonAns);
            foreach (var item in discountfood)
            {
                if (item.ma_loai_mon_an == QuyDinh.loai_san_pham_uu_dai)
                    DiscountFood.Add(item);
            }
            var bill = DataProvider.Ins.DB.HoaDons.ToList();
            var billlast = bill[bill.Count() - 1];
            billdetail = (from u in DataProvider.Ins.DB.CT_HoaDon  where u.ma_hoa_don == billlast.ma_hoa_don select u).ToList<CT_HoaDon>();
            MyMessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(4000));
            MyMessageQueue.DiscardDuplicates = true;
            int? numfood = QuyDinh.count_uu_dai;
            

            //Tra cứu mã khách hàng trong database
            FindIDCustomerCommand = new RelayCommand<object>((p) =>
            {
                if (IdFirst.Length < 10 || String.IsNullOrEmpty(IdFirst))
                    return false;
                return true; 
            }, (p) =>
            {
                int i = FindIDCustomer(IdFirst);
                if (i == -1)
                {
                    IdResult = NumberResult = "Chưa có dữ liệu";
                }
                else
                {
                    IdResult = Convert.ToString(DiscountList[i].ma_uu_dai);
                    NumberResult = Convert.ToString(DiscountList[i].so_luot_mua);
                }
            });

            //Thêm số lần mua cho khách hàng
            AddCustomerCommand = new RelayCommand<Window>((p) =>
            {
                if (IdFirst.Length < 10 || IdFirst == null )
                    return false;
                return true;
            }, (p) =>
            {
                int i = FindIDCustomer(IdFirst);
                int count = CountFood();
                if (numcheck == 0)
                {
                    if (i == -1)
                    {
                        var discount = new PhieuUuDai() { ma_uu_dai = Convert.ToInt32(IdFirst) };
                        if (count > numfood)
                        {
                            while (count >= numfood)
                            {
                                num++;
                                count -= (int)numfood;
                            }
                            IdResult = IdFirst;
                            if (count < numfood)
                                NumberResult = Convert.ToString(count) + " (còn " + Convert.ToString(num) + " lượt ưu đãi)";
                            discount.so_luot_mua = count;
                            DataProvider.Ins.DB.PhieuUuDais.Add(discount);
                            DataProvider.Ins.DB.SaveChanges();
                        }
                        else if (count == numfood)
                        {
                            num = 1;
                            discount.so_luot_mua = 0;
                            NumberResult = " 0 (còn " + Convert.ToString(num) + " lượt ưu đãi)";
                            DataProvider.Ins.DB.PhieuUuDais.Add(discount);
                            DataProvider.Ins.DB.SaveChanges();
                        }
                        else
                        {
                            IdFirst = IdResult = NumberResult = "";
                            DataProvider.Ins.DB.PhieuUuDais.Add(discount);
                            DataProvider.Ins.DB.SaveChanges();
                            //CheckOut(bill);
                            p.Close();
                        }
                    }
                    else
                    {
                        int id = Convert.ToInt32(IdFirst);
                        var discount = (from u in DataProvider.Ins.DB.PhieuUuDais where u.ma_uu_dai == id select u).Single();
                        discount.so_luot_mua = count + discount.so_luot_mua;
                        DataProvider.Ins.DB.SaveChanges();
                        count = (int)discount.so_luot_mua;

                        // NumberResult = Convert.ToString(discount.so_luot_mua);
                        if (count < numfood)
                        {
                            IdFirst = IdResult = NumberResult = "";
                            p.Close();
                        }
                        else
                        {
                            while (count >= numfood)
                            {
                                num++;
                                count -= (int)numfood;
                            }
                            NumberResult = Convert.ToString(count) + " (còn " + Convert.ToString(num) + " lượt ưu đãi)";
                            discount.so_luot_mua = count;
                            DataProvider.Ins.DB.SaveChanges();
                        }
                    }
                    MyMessageQueue.Enqueue("Thêm ưu đãi thành công!");
                }
                else
                    MyMessageQueue.Enqueue("Error, hóa đơn đã được xử lý ưu đãi!");
                numcheck = 1;
                
            });

            //Chọn món ưu đãi
            SeleteDiscountCommand = new RelayCommand<Window>((p) =>
            {
                if (SelectedDiscountFood == null || String.IsNullOrEmpty(IdFirst) || num == 0 )
                    return false;
                return true;
            }, (p) =>
            {
                int id = Convert.ToInt32(IdFirst);
                var discount = (from u in DataProvider.Ins.DB.PhieuUuDais where u.ma_uu_dai == id select u).Single();
                num--;
                if (num > 0 && discount.so_luot_mua < numfood)
                {
                    var discountbill = (from u in DataProvider.Ins.DB.CT_HoaDon where u.ma_mon_an == SelectedDiscountFood.ma_mon_an select u).FirstOrDefault();
                    if(discountbill != null)
                    {
                        discountbill.so_luong++;
                        DataProvider.Ins.DB.SaveChanges();
                    }
                    else if(discountbill == null)
                    {
                        var item = new CT_HoaDon()
                        {
                            ma_hoa_don = bill[bill.Count() - 1].ma_hoa_don,
                            ma_mon_an = SelectedDiscountFood.ma_mon_an,
                            so_luong = 1,
                            //gia_tien = 0,
                            thanh_tien = 0
                        };
                        DataProvider.Ins.DB.CT_HoaDon.Add(item);
                        DataProvider.Ins.DB.SaveChanges();
                    }
                    NumberResult = Convert.ToString(discount.so_luot_mua) + " (còn " + Convert.ToString(num) + " lượt ưu đãi)";
                }
                else if (num == 0 && discount.so_luot_mua == numfood)
                {
                    var discountbill = (from u in DataProvider.Ins.DB.CT_HoaDon where u.ma_mon_an == SelectedDiscountFood.ma_mon_an select u).FirstOrDefault();
                    if (discountbill != null)
                    {
                        discountbill.so_luong++;
                        DataProvider.Ins.DB.SaveChanges();
                    }
                    else if (discountbill == null)
                    {
                        var item = new CT_HoaDon()
                        {
                            ma_hoa_don = bill[bill.Count() - 1].ma_hoa_don,
                            ma_mon_an = SelectedDiscountFood.ma_mon_an,
                            so_luong = 1,
                            //gia_tien = 0,
                            thanh_tien = 0
                        };
                        DataProvider.Ins.DB.CT_HoaDon.Add(item);
                        DataProvider.Ins.DB.SaveChanges();
                    }
                    DataProvider.Ins.DB.PhieuUuDais.Remove(discount);
                    DataProvider.Ins.DB.SaveChanges();
                }
                else if (num == 0 && discount.so_luot_mua < numfood)
                {
                    var discountbill = (from u in DataProvider.Ins.DB.CT_HoaDon where u.ma_mon_an == SelectedDiscountFood.ma_mon_an select u).FirstOrDefault();
                    if (discountbill != null)
                    {
                        discountbill.so_luong++;
                        DataProvider.Ins.DB.SaveChanges();
                    }
                    else if (discountbill == null)
                    {
                        var item = new CT_HoaDon()
                        {
                            ma_hoa_don = bill[bill.Count() - 1].ma_hoa_don,
                            ma_mon_an = SelectedDiscountFood.ma_mon_an,
                            so_luong = 1,
                            //gia_tien = 0,
                            thanh_tien = 0
                        };
                        DataProvider.Ins.DB.CT_HoaDon.Add(item);
                        DataProvider.Ins.DB.SaveChanges();
                    }
                    IdFirst = IdResult = NumberResult = "";
                    p.Close();
                }
            });

        }
    }
}