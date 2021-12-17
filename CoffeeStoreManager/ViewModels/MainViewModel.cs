using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CoffeeStoreManager.Models;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System;
using CoffeeStoreManager.Views;
using CoffeeStoreManager.Views.Discount_Bill;

namespace CoffeeStoreManager.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public bool IsLoaded = false;
        public ICommand LoadedWindowCommand { get; set; }

        private ObservableCollection<LoaiMonAn> _LoaiMonAn;
        public ObservableCollection<LoaiMonAn> LoaiMonAn
        {
            get => _LoaiMonAn;
            set
            {
                _LoaiMonAn = value;
                OnPropertyChanged(nameof(LoaiMonAn));
            }
        }
        private LoaiMonAn _SelectedLoaiMonAn;
        public LoaiMonAn SelectedLoaiMonAn
        {
            get => _SelectedLoaiMonAn;
            set
            {

                _SelectedLoaiMonAn = value;
                OnPropertyChanged(nameof(SelectedLoaiMonAn));
            }
        }
        /*private int _typefood;
        public int typefood { get => _typefood; set { _typefood = value; OnPropertyChanged(nameof(typefood)); } }*/
        private ObservableCollection<MonAn> _MonAn;
        public ObservableCollection<MonAn> MonAn
        {
            get => _MonAn;
            set
            {
                _MonAn = value;
                OnPropertyChanged(nameof(MonAn));
            }
        }
        private MonAn _SelectedMonAn;
        public MonAn SelectedMonAn
        {
            get => _SelectedMonAn;
            set
            {
                _SelectedMonAn = value;
                OnPropertyChanged(nameof(SelectedMonAn));
            }
        }
        private int _so_luong;
        public int so_luong { get => _so_luong; set { _so_luong = value; OnPropertyChanged(nameof(so_luong)); } }
        private ObservableCollection<ViewBill> _BillDetail;
        public ObservableCollection<ViewBill> BillDetail
        {
            get => _BillDetail;
            set
            {
                _BillDetail = value;
                OnPropertyChanged(nameof(_BillDetail));
            }
        }
        private ViewBill _SelectedItem;
        public ViewBill SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
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
        private ObservableCollection<Table> _Tables;
        public ObservableCollection<Table> Tables
        {
            get => _Tables;
            set
            {
                _Tables = value;
                OnPropertyChanged(nameof(Tables));
            }
        }
        private int _number;
        public int number { get => _number; set { _number = value; OnPropertyChanged(nameof(number)); } }
        private decimal? _totalmoney;
        public decimal? totalmoney { get => _totalmoney; set { _totalmoney = value; OnPropertyChanged(nameof(totalmoney)); } }
        //private bool _IsChecked1;
        //public bool IsChecked1 { get => _IsChecked1; set { _IsChecked1 = value; OnPropertyChanged(nameof(IsChecked1)); } }
        //private bool _IsChecked2;
        //public bool IsChecked2 { get => _IsChecked2; set { _IsChecked2 = value; OnPropertyChanged(nameof(IsChecked2)); } }
        //private bool _IsChecked3;
        //public bool IsChecked3 { get => _IsChecked3; set { _IsChecked3 = value; OnPropertyChanged(nameof(IsChecked3)); } }
        //private bool _IsChecked4;
        //public bool IsChecked4 { get => _IsChecked4; set { _IsChecked4 = value; OnPropertyChanged(nameof(IsChecked4)); } }
        //private bool _IsChecked5;
        //public bool IsChecked5 { get => _IsChecked5; set { _IsChecked5 = value; OnPropertyChanged(nameof(IsChecked5)); } }
        //private bool _IsChecked6;
        //public bool IsChecked6 { get => _IsChecked6; set { _IsChecked6 = value; OnPropertyChanged(nameof(IsChecked6)); } }
        //private bool _IsChecked7;
        //public bool IsChecked7 { get => _IsChecked7; set { _IsChecked7 = value; OnPropertyChanged(nameof(IsChecked7)); } }
        //private bool _IsChecked8;
        //public bool IsChecked8 { get => _IsChecked8; set { _IsChecked8 = value; OnPropertyChanged(nameof(IsChecked8)); } }
        //private bool _IsChecked9;
        //public bool IsChecked9 { get => _IsChecked9; set { _IsChecked9 = value; OnPropertyChanged(nameof(IsChecked9)); } }
        private ObservableCollection<ItemcontrolButton> _ItemcontrolButtonList;
        public ObservableCollection<ItemcontrolButton> ItemcontrolButtonList
        {
            get => _ItemcontrolButtonList;
            set
            {
                _ItemcontrolButtonList = value;
                OnPropertyChanged(nameof(ItemcontrolButtonList));
            }
        }
        private ObservableCollection<EmptyTable> _EmptyTables;
        public ObservableCollection<EmptyTable> EmptyTables
        {
            get => _EmptyTables;
            set
            {
                _EmptyTables = value;
                OnPropertyChanged(nameof(EmptyTables));
            }
        }
        private EmptyTable _SelectedEmptyTable;
        public EmptyTable SelectedEmptyTable
        {
            get => _SelectedEmptyTable;
            set
            {
                _SelectedEmptyTable = value;
                OnPropertyChanged(nameof(SelectedEmptyTable));
            }
        }
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand CheckOutCommand { get; set; }
        public ICommand ChooseTableCommand { get; set; }
        public ICommand ChangeTableCommand { get; set; }
        public ICommand DiscountCommand { get; set; }
        void LoadFood()
        {
            ObservableCollection<MonAn> food = new ObservableCollection<MonAn>(DataProvider.Ins.DB.MonAns); ;
            if (SelectedLoaiMonAn != null)
            {
                MonAn.Clear();
                foreach (var item in food)
                {
                    if (item.ma_loai_mon_an == SelectedLoaiMonAn.ma_loai_mon_an)
                        MonAn.Add(item);
                }
            }
        }
        void AddViewBill(int number)
        {
            var billdetail = new CT_HoaDon() { ma_mon_an = SelectedMonAn.ma_mon_an, so_luong = so_luong, thanh_tien = SelectedMonAn.gia_tien * so_luong };
            var viewbill = new ViewBill()
            {
                ma_mon_an = billdetail.ma_mon_an,
                ten_mon_an = SelectedMonAn.ten_mon_an,
                gia_tien = SelectedMonAn.gia_tien,
                so_luong = so_luong,
                thanh_tien = so_luong * SelectedMonAn.gia_tien
            };
            BillDetail.Add(viewbill);
            Tables[number - 1].viewbilloftable.Add(viewbill);
            Tables[number - 1].billoftable.Add(billdetail);
            Tables[number - 1].status = true;
            SumMoney();
        }
        void UpdateViewBill(int i, int so_luong)
        {
            var viewbill = new ViewBill()
            {
                ma_mon_an = BillDetail[i].ma_mon_an,
                ten_mon_an = BillDetail[i].ten_mon_an,
                gia_tien = BillDetail[i].gia_tien,
                so_luong = BillDetail[i].so_luong + so_luong,
                thanh_tien = SelectedMonAn.gia_tien * (BillDetail[i].so_luong + so_luong)
            };
            var billdetail = new CT_HoaDon()
            {
                ma_mon_an = BillDetail[i].ma_mon_an,
                so_luong = BillDetail[i].so_luong + so_luong,
                thanh_tien = SelectedMonAn.gia_tien * (BillDetail[i].so_luong + so_luong)
            };
            BillDetail.Remove(BillDetail[i]);
            Tables[number - 1].viewbilloftable.Remove(Tables[number - 1].viewbilloftable[i]);
            Tables[number - 1].billoftable.Remove(Tables[number - 1].billoftable[i]);
            if (Tables[number - 1].viewbilloftable.Count < 0)
                Tables[number - 1].status = false;
            if (viewbill.so_luong > 0)
            {
                BillDetail.Add(viewbill);
                Tables[number - 1].viewbilloftable.Add(viewbill);
                Tables[number - 1].billoftable.Add(billdetail);
            }
            SumMoney();
        }
        void LoadListBill(int number)
        {
            BillDetail.Clear();
            totalmoney = 0;
            foreach (ViewBill item in Tables[number - 1].viewbilloftable)
            {
                BillDetail.Add(item);
            }
            SumMoney();
        }
        decimal? SumMoney()
        {
            totalmoney = 0;
            foreach (ViewBill item in BillDetail)
            {
                totalmoney += item.thanh_tien;
            }
            return totalmoney;
        }
        int FindFirstSpace(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == ' ')
                    return i;
            }
            return -1;
        }
        void CheckColor()
        {
            ItemcontrolButtonList.Clear();
            for (int i = 0; i < QuyDinh.so_ban; i++)
            //for (int i = 0; i < 15; i++)
            {
                ItemcontrolButton item = new ItemcontrolButton()
                {
                    index = i + 1,
                    text = "Bàn " + Convert.ToString(i + 1),
                    isnotempty = false,
                    isselected = false,
                    choosetable = ChooseTableCommand
                };
                if (Tables[i].status == true)
                    item.isnotempty = true;
                if (number == i + 1)
                    item.isselected = true;
                ItemcontrolButtonList.Add(item);
            }
            //for (int i = 1; i < 2; i++)
            //{
            //    ItemcontrolButtonList[i - 1].isnotempty = Tables[i - 1].status;
            //    //switch (i)
            //    //{
            //    //    case 1:
            //    //        IsChecked1 = Tables[i - 1].status;
            //    //        break;
            //    //    case 2:
            //    //        IsChecked2 = Tables[i - 1].status;
            //    //        break;
            //    //    case 3:
            //    //        IsChecked3 = Tables[i - 1].status;
            //    //        break;
            //    //    case 4:
            //    //        IsChecked4 = Tables[i - 1].status;
            //    //        break;
            //    //    case 5:
            //    //        IsChecked5 = Tables[i - 1].status;
            //    //        break;
            //    //    case 6:
            //    //        IsChecked6 = Tables[i - 1].status;
            //    //        break;
            //    //    case 7:
            //    //        IsChecked7 = Tables[i - 1].status;
            //    //        break;
            //    //    case 8:
            //    //        IsChecked8 = Tables[i - 1].status;
            //    //        break;
            //    //    case 9:
            //    //        IsChecked9 = Tables[i - 1].status;
            //    //        break;
            //    //}
            //}
        }
        int FindMonAn(int ma_mon_an)
        {
            for (int i = 0; i < BillDetail.Count; i++)
            {
                if (BillDetail[i].ma_mon_an == ma_mon_an)
                    return i;
            }
            return -1;
        }
        void LoadEmptyTable()
        {
            EmptyTables.Clear();
            for (int i = 0; i < Tables.Count; i++)
            {
                if (Tables[i].status == false)
                {
                    var empty_table = new EmptyTable()
                    {
                        index_emptytable = i + 1,
                        emptytable = ItemcontrolButtonList[i].text
                    };
                    EmptyTables.Add(empty_table);
                }
            }
        }
        void TransTable(Table A, Table B)
        {
            A.status = B.status;
            for (int i = 0; i < B.viewbilloftable.Count; i++)
            {
                A.viewbilloftable.Add(B.viewbilloftable[i]);
            }
            for (int i = 0; i < B.billoftable.Count; i++)
            {
                A.billoftable.Add(B.billoftable[i]);
            }
        }
        public MainViewModel()
        {

            // Chọn bàn
            ChooseTableCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                number = Convert.ToInt32(p);
                CheckColor();
                LoadListBill(number);
                LoadEmptyTable();
            });
            LoadedWindowCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                IsLoaded = true;
                if (p == null)
                    return;
                p.Hide();
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.ShowDialog();

                if (loginWindow.DataContext == null)
                    return;
                var loginVM = loginWindow.DataContext as LoginViewModel;

                if (loginVM.IsLogin)
                {
                    AdminWindow adminWindow = new AdminWindow();
                    adminWindow.Show();
                    loginWindow.Hide();
                }
                else
                {
                    p.Show();
                    loginWindow.Close();
                }
            });
            DateTime? today = DateTime.Now;
            BillDetail = new ObservableCollection<ViewBill>();
            MonAn = new ObservableCollection<MonAn>(DataProvider.Ins.DB.MonAns);
            Tables = new ObservableCollection<Table>();
            QuyDinh = DataProvider.Ins.DB.QuyDinhs.FirstOrDefault();
            for (int i = 0; i < QuyDinh.so_ban; i++)
            //for (int i = 0; i < 15; i++)
            {
                Table item = new Table()
                {
                    tablenumber = i + 1,
                    status = false,
                    viewbilloftable = new ObservableCollection<ViewBill>(),
                    billoftable = new ObservableCollection<CT_HoaDon>()
                };
                Tables.Add(item);
                //new Table() {tablenumber = 2 ,status = false,viewbilloftable = new ObservableCollection<ViewBill>(),billoftable = new ObservableCollection<CT_HoaDon>()},
                //new Table() {tablenumber = 3 ,status = false,viewbilloftable = new ObservableCollection<ViewBill>(),billoftable = new ObservableCollection<CT_HoaDon>()},
                //new Table() {tablenumber = 4 ,status = false,viewbilloftable = new ObservableCollection<ViewBill>(),billoftable = new ObservableCollection<CT_HoaDon>()},
                //new Table() {tablenumber = 5 ,status = false,viewbilloftable = new ObservableCollection<ViewBill>(),billoftable = new ObservableCollection<CT_HoaDon>()},
                //new Table() {tablenumber = 6 ,status = false,viewbilloftable = new ObservableCollection<ViewBill>(),billoftable = new ObservableCollection<CT_HoaDon>()},
                //new Table() {tablenumber = 7 ,status = false,viewbilloftable = new ObservableCollection<ViewBill>(),billoftable = new ObservableCollection<CT_HoaDon>()},
                //new Table() {tablenumber = 8 ,status = false,viewbilloftable = new ObservableCollection<ViewBill>(),billoftable = new ObservableCollection<CT_HoaDon>()},
                //new Table() {tablenumber = 9 ,status = false,viewbilloftable = new ObservableCollection<ViewBill>(),billoftable = new ObservableCollection<CT_HoaDon>()}
            };
            ItemcontrolButtonList = new ObservableCollection<ItemcontrolButton>();
            for (int i = 0; i < QuyDinh.so_ban; i++)
            //for (int i = 0; i < 15; i++)
            {
                ItemcontrolButton item = new ItemcontrolButton()
                {
                    index = i + 1,
                    text = "Bàn " + Convert.ToString(i + 1),
                    isnotempty = false,
                    isselected = false,
                    choosetable = ChooseTableCommand
                };
                ItemcontrolButtonList.Add(item);
            }
            number = 0;
            EmptyTables = new ObservableCollection<EmptyTable>();
            totalmoney = 0;
            LoadEmptyTable();

            // Thêm món vào hóa đơn (chưa lưu vào database) 
            AddCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedMonAn == null || so_luong == 0 || number == 0)
                    return false;
                return true;
            }, (p) =>
            {
                if (BillDetail.Count() == 0 || BillDetail == null)
                {
                    AddViewBill(number);
                    CheckColor();
                    LoadEmptyTable();
                }
                else
                {
                    int i = FindMonAn(SelectedMonAn.ma_mon_an);
                    if (i == -1)
                        AddViewBill(number);
                    else
                        UpdateViewBill(i, so_luong);
                    CheckColor();
                    LoadEmptyTable();
                }
            });

            // Xóa món được chọn trong list
            DeleteCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                return true;
            }, (p) =>
            {
                int i = FindMonAn(SelectedItem.ma_mon_an);
                BillDetail.Remove(SelectedItem);
                Tables[number - 1].viewbilloftable.Remove(Tables[number - 1].viewbilloftable[i]);
                Tables[number - 1].billoftable.Remove(Tables[number - 1].billoftable[i]);
                if (BillDetail.Count == 0)
                    Tables[number - 1].status = false;
                CheckColor();
                LoadEmptyTable();
            });

            // Thanh toán và lưu hóa đơn vào database
            CheckOutCommand = new RelayCommand<object>((p) =>
            {
                if (BillDetail == null || BillDetail.Count() == 0)
                    return false;
                return true;
            }, (p) =>
            {
                decimal? tong_tien = 0;
                for (int i = 0; i < BillDetail.Count; i++)
                {
                    tong_tien = tong_tien + BillDetail[i].thanh_tien;
                }
                var bill = new HoaDon() { tong_tien = tong_tien, ngay_xuat_hoa_don = today};
                DataProvider.Ins.DB.HoaDons.Add(bill);
                DataProvider.Ins.DB.SaveChanges();
                for (int i = 0; i < Tables[number - 1].billoftable.Count(); i++)
                {
                    Tables[number - 1].billoftable[i].ma_hoa_don = bill.ma_hoa_don;
                    DataProvider.Ins.DB.CT_HoaDon.Add(Tables[number - 1].billoftable[i]);
                    DataProvider.Ins.DB.SaveChanges();
                }
                BillDetail.Clear();
                Tables[number - 1].viewbilloftable.Clear();
                Tables[number - 1].billoftable.Clear();
                Tables[number - 1].status = false;
                CheckColor();
                number = 0;
                LoadEmptyTable();
                if (totalmoney >= 150000)
                {
                    DiscountWindow discountWindow = new DiscountWindow();
                    discountWindow.ShowDialog();
                }
                totalmoney = 0;
            });


            //Chuyển bàn
            ChangeTableCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedEmptyTable == null || number == 0)
                    return false;
                return true;
            }, (p) =>
            {
                int numberfirst = number;
                int numbersecond = SelectedEmptyTable.index_emptytable;
                //switch (SelectedEmptyTable.emptytable)
                //{
                //    case "Bàn 1":
                //        numbersecond = 1;
                //        break;
                //    case "Bàn 2":
                //        numbersecond = 2;
                //        break;
                //    case "Bàn 3":
                //        numbersecond = 3;
                //        break;
                //    case "Bàn 4":
                //        numbersecond = 4;
                //        break;
                //    case "Bàn 5":
                //        numbersecond = 5;
                //        break;
                //    case "Bàn 6":
                //        numbersecond = 6;
                //        break;
                //    case "Bàn 7":
                //        numbersecond = 7;
                //        break;
                //    case "Bàn 8":
                //        numbersecond = 8;
                //        break;
                //    case "Bàn 9":
                //        numbersecond = 9;
                //        break;
                //}
                TransTable(Tables[numbersecond - 1], Tables[numberfirst - 1]);
                BillDetail.Clear();
                totalmoney = 0;
                Tables[numberfirst - 1].viewbilloftable.Clear();
                Tables[numberfirst - 1].billoftable.Clear();
                Tables[numberfirst - 1].status = false;
                CheckColor();
                LoadEmptyTable();
            });
        }
    }
    public class EmptyTable
    {
        public int index_emptytable { get; set; }
        public string emptytable { get; set; }
    }
}
