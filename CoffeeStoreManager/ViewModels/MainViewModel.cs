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
using MaterialDesignThemes.Wpf;
using System.Windows.Forms;

namespace CoffeeStoreManager.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public bool IsLoaded = false;
        public ICommand LoadedWindowCommand { get; set; }

        //private int _statusload;
        //public int statusload { get => _statusload; set { _statusload = value; OnPropertyChanged(nameof(statusload)); } }
        //private DateTime? _today;
        //public DateTime? today { get => _today; set { _today = value; OnPropertyChanged(nameof(today)); } }
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
        private int? _soban;
        public int? soban { get => _soban; set { _soban = value; OnPropertyChanged(nameof(soban)); } }
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
        private SnackbarMessageQueue myMessageQueue;
        public SnackbarMessageQueue MyMessageQueue { get => myMessageQueue; set { myMessageQueue = value; OnPropertyChanged(nameof(MyMessageQueue)); } }
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand CheckOutCommand { get; set; }
        public ICommand ChooseTableCommand { get; set; }
        public ICommand ChangeTableCommand { get; set; }
        public ICommand DiscountCommand { get; set; }

        void LoadInfor()
        {
            DateTime? today = DateTime.Now;
            BillDetail = new ObservableCollection<ViewBill>();
            MonAn = new ObservableCollection<MonAn>(DataProvider.Ins.DB.MonAns);
            QuyDinh = DataProvider.Ins.DB.QuyDinhs.FirstOrDefault();
            Tables = new ObservableCollection<Table>();
            //soban = QuyDinh.so_ban;
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
        }
        void AddViewBill(int number)
        {
            var billdetail = new CT_HoaDon() { ma_mon_an = SelectedMonAn.ma_mon_an, so_luong = so_luong, gia_tien = SelectedMonAn.gia_tien, thanh_tien = SelectedMonAn.gia_tien * so_luong };
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
                    EmptyTable empty_table = new EmptyTable()
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
                    adminWindow.ShowDialog();
                    loginWindow.Close();
                }
                else
                {
                    p.ShowDialog();
                    loginWindow.Close();
                }
            });
            DateTime? today = DateTime.Now;
            BillDetail = new ObservableCollection<ViewBill>();
            MonAn = new ObservableCollection<MonAn>(DataProvider.Ins.DB.MonAns);
            QuyDinh = DataProvider.Ins.DB.QuyDinhs.FirstOrDefault();
            Tables = new ObservableCollection<Table>();
            for (int i = 0; i < QuyDinh.so_ban; i++)
            {
                Table item = new Table()
                {
                    tablenumber = i + 1,
                    status = false,
                    viewbilloftable = new ObservableCollection<ViewBill>(),
                    billoftable = new ObservableCollection<CT_HoaDon>()
                };
                Tables.Add(item);
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
            MyMessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(4000));
            MyMessageQueue.DiscardDuplicates = true;
            //LoadInfor();
            //if (statusload == 1)
            //    MessageBox.Show("11111");

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
                    if (so_luong < 0)
                    {
                        MyMessageQueue.Enqueue("Số lượng món nhỏ hơn 0 nên đã bị xóa!");
                        so_luong = 0;
                    }
                    else
                        AddViewBill(number);
                    CheckColor();
                    LoadEmptyTable();
                }
                else
                {
                    int i = FindMonAn(SelectedMonAn.ma_mon_an);
                    if (i == -1)
                    {
                        if (so_luong < 0)
                        {
                            MyMessageQueue.Enqueue("Số lượng món nhỏ hơn 0 nên đã bị xóa!");
                            so_luong = 0;
                        }
                        else
                            AddViewBill(number);
                    }
                    else
                    {
                        UpdateViewBill(i, so_luong);
                        if ((BillDetail[i].so_luong + so_luong) <= 0)
                        {
                            MyMessageQueue.Enqueue("Số lượng món nhỏ hơn 0 nên đã bị xóa!");
                        }
                        so_luong = 0;
                    }
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
                number = 0;
                CheckColor();
                LoadEmptyTable();
                MyMessageQueue.Enqueue("Thanh toán thành công! ");
                MessageBoxButton buttons = MessageBoxButton.YesNo;
                DialogResult result = (DialogResult)System.Windows.MessageBox.Show("Bạn đã có phiếu ưu đãi hay chưa? ", "Kiểm tra", buttons);
                if (result == DialogResult.Yes)
                {
                    UpdateDiscountWindow updatediscountWindow = new UpdateDiscountWindow();
                    updatediscountWindow.ShowDialog();
                    MyMessageQueue.Enqueue("Xử lý ưu đãi thành công!");
                }
                else
                {
                    if (totalmoney >= QuyDinh.muc_tien_nhan_uu_dai)
                    {
                        CreateDiscountWindow creatediscountWindow = new CreateDiscountWindow();
                        creatediscountWindow.ShowDialog();
                        MyMessageQueue.Enqueue("Tạo phiếu ưu đãi thành công!");
                    }

                }
                SelectedMonAn = null;
                so_luong = 0;
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
