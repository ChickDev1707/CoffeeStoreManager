﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CoffeeStoreManager.Models;
using CoffeeStoreManager.Views.ManageFood;
using System.Windows.Controls;
using System.Windows.Media;


namespace CoffeeStoreManager.ViewModels
{
    public class EmptyTable
    {
        public string emptytable { get; set; }
    }
    public class MainViewModel: BaseViewModel
    {
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
        private ObservableCollection<Table> _Tables;
        public ObservableCollection<Table> Tables
        {
            get => _Tables;
            set
            {
                _Tables = value;
                OnPropertyChanged(nameof(_Tables));
            }
        }
        private int _number;
        public int number { get => _number; set { _number = value; OnPropertyChanged(nameof(_number)); } }
        private ObservableCollection<EmptyTable> _EmptyTables;
        public ObservableCollection<EmptyTable> EmptyTables
        {
            get => _EmptyTables;
            set
            {
                _EmptyTables = value;
                OnPropertyChanged(nameof(_EmptyTables));
            }
        }
        private EmptyTable _SelectedEmptyTable;
        public EmptyTable SelectedEmptyTable
        {
            get => _SelectedEmptyTable;
            set
            {
                _SelectedEmptyTable = value;
                OnPropertyChanged(nameof(_SelectedEmptyTable));
            }
        }
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand CheckOutCommand { get; set; }
        public ICommand ChooseTableCommand { get; set; }
        public ICommand ChangeTableCommand { get; set; }
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
            var billdetail = new CT_HoaDon() { ma_mon_an = SelectedMonAn.ma_mon_an, gia_tien = SelectedMonAn.gia_tien, so_luong = so_luong, thanh_tien = SelectedMonAn.gia_tien * so_luong };
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
                gia_tien = BillDetail[i].gia_tien,
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
        }
        void LoadListTables(int number)
        {
            BillDetail.Clear();
            foreach (ViewBill item in Tables[number - 1].viewbilloftable)
            {
                BillDetail.Add(item);
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
                    var empty_table = new EmptyTable() { emptytable = "Bàn " + Tables[i].tablenumber };
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
            DateTime? today = DateTime.Now;
            BillDetail = new ObservableCollection<ViewBill>();
            LoaiMonAn = new ObservableCollection<LoaiMonAn>(DataProvider.Ins.DB.LoaiMonAns);
            MonAn = new ObservableCollection<MonAn>(DataProvider.Ins.DB.MonAns);
            Tables = new ObservableCollection<Table>()
            {
                new Table() {tablenumber = 1 ,status = false,viewbilloftable = new ObservableCollection<ViewBill>(),billoftable = new ObservableCollection<CT_HoaDon>()},
                new Table() {tablenumber = 2 ,status = false,viewbilloftable = new ObservableCollection<ViewBill>(),billoftable = new ObservableCollection<CT_HoaDon>()},
                new Table() {tablenumber = 3 ,status = false,viewbilloftable = new ObservableCollection<ViewBill>(),billoftable = new ObservableCollection<CT_HoaDon>()},
                new Table() {tablenumber = 4 ,status = false,viewbilloftable = new ObservableCollection<ViewBill>(),billoftable = new ObservableCollection<CT_HoaDon>()},
                new Table() {tablenumber = 5 ,status = false,viewbilloftable = new ObservableCollection<ViewBill>(),billoftable = new ObservableCollection<CT_HoaDon>()},
                new Table() {tablenumber = 6 ,status = false,viewbilloftable = new ObservableCollection<ViewBill>(),billoftable = new ObservableCollection<CT_HoaDon>()},
                new Table() {tablenumber = 7 ,status = false,viewbilloftable = new ObservableCollection<ViewBill>(),billoftable = new ObservableCollection<CT_HoaDon>()},
                new Table() {tablenumber = 8 ,status = false,viewbilloftable = new ObservableCollection<ViewBill>(),billoftable = new ObservableCollection<CT_HoaDon>()},
                new Table() {tablenumber = 9 ,status = false,viewbilloftable = new ObservableCollection<ViewBill>(),billoftable = new ObservableCollection<CT_HoaDon>()}
            };
            EmptyTables = new ObservableCollection<EmptyTable>();
            LoadEmptyTable();

            // Thêm món vào hóa đơn (chưa lưu vào database) 
            AddCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedLoaiMonAn == null || SelectedMonAn == null || so_luong == 0 || number == 0)
                    return false;
                return true;
            }, (p) =>
            {
                if (BillDetail.Count() == 0 || BillDetail == null)
                {
                    AddViewBill(number);
                    LoadEmptyTable();
                }
                else
                {
                    int i = FindMonAn(SelectedMonAn.ma_mon_an);
                    if (i == -1)
                        AddViewBill(number);
                    else
                        UpdateViewBill(i, so_luong);
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
                var bill = new HoaDon() { ma_ban_an = number, tong_tien = tong_tien, ngay_xuat_hoa_don = today };
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
                LoadEmptyTable();
            });

            // Chọn bàn
            ChooseTableCommand = new RelayCommand<Button>((p) =>
            {
                return true;
            }, (p) =>
            {
                switch (p.Content)
                {
                    case "Bàn 1":
                        number = 1;
                        break;
                    case "Bàn 2":
                        number = 2;
                        break;
                    case "Bàn 3":
                        number = 3;
                        break;
                    case "Bàn 4":
                        number = 4;
                        break;
                    case "Bàn 5":
                        number = 5;
                        break;
                    case "Bàn 6":
                        number = 6;
                        break;
                    case "Bàn 7":
                        number = 7;
                        break;
                    case "Bàn 8":
                        number = 8;
                        break;
                    case "Bàn 9":
                        number = 9;
                        break;
                }
                if (Tables[number - 1].status == false && p.Background == Brushes.White)
                {
                    p.Background = Brushes.LightBlue;
                    LoadListTables(number);
                }
                else if (Tables[number - 1].status == false && p.Background == Brushes.LightBlue)
                {
                    p.Background = Brushes.White;
                    BillDetail.Clear();
                    number = 0;
                }
                else if (Tables[number - 1].status == true && (p.Background == Brushes.LightBlue || p.Background == Brushes.White)) 
                {
                    p.Background = Brushes.Yellow;
                    LoadListTables(number);
                }
                else if(Tables[number - 1].status == false && p.Background == Brushes.Yellow)
                {
                    p.Background = Brushes.White;
                    LoadListTables(number);
                    number = 0;
                }
                LoadEmptyTable();
            });

            //Chuyển bàn
            ChangeTableCommand = new RelayCommand<Button>((p) =>
            {
                if (SelectedEmptyTable == null)
                    return false;
                return true;
            }, (p) =>
            {
                int numberfirst = number;
                int numbersecond = 0;
                switch (SelectedEmptyTable.emptytable)
                {
                    case "Bàn 1":
                        numbersecond = 1;
                        break;
                    case "Bàn 2":
                        numbersecond = 2;
                        break;
                    case "Bàn 3":
                        numbersecond = 3;
                        break;
                    case "Bàn 4":
                        numbersecond = 4;
                        break;
                    case "Bàn 5":
                        numbersecond = 5;
                        break;
                    case "Bàn 6":
                        numbersecond = 6;
                        break;
                    case "Bàn 7":
                        numbersecond = 7;
                        break;
                    case "Bàn 8":
                        numbersecond = 8;
                        break;
                    case "Bàn 9":
                        numbersecond = 9;
                        break;
                }
                TransTable(Tables[numbersecond - 1], Tables[numberfirst - 1]);
                BillDetail.Clear();
                Tables[numberfirst - 1].viewbilloftable.Clear();
                Tables[numberfirst - 1].billoftable.Clear();
                Tables[numberfirst - 1].status = false;
                LoadEmptyTable();
            });
        }
    }
}
