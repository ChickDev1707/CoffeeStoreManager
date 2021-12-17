using CoffeeStoreManager.Models;
using CoffeeStoreManager.Views.ManageEmployee;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CoffeeStoreManager.Resources.Utils;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace CoffeeStoreManager.ViewModels
{
    public class EmployeeViewModel : BaseViewModel
    {
        private ObservableCollection<ViewEmployee> employeeList;
        private ObservableCollection<ViewEmployee> dtg_employeeList;
        private List<LoaiNhanVien> employeeTypeList;
        private ViewEmployee selectedEmployee;
        private List<string> dataCbxYear;
        private string searchKey;


        public string SearchKey { get => searchKey; set { searchKey = value; OnPropertyChanged(nameof(SearchKey)); } }
        public ViewEmployee SelectedEmployee { get => selectedEmployee; set { selectedEmployee = value; OnPropertyChanged(nameof(selectedEmployee)); } }

        public ObservableCollection<ViewEmployee> EmployeeList { get => employeeList; set { employeeList = value; OnPropertyChanged(nameof(employeeList)); } }
        public ObservableCollection<ViewEmployee> Dtg_employeeList { get => dtg_employeeList; set { dtg_employeeList = value; OnPropertyChanged(nameof(dtg_employeeList)); } }
        public List<LoaiNhanVien> EmployeeTypeList { get => employeeTypeList; set { employeeTypeList = value; OnPropertyChanged(nameof(employeeTypeList)); } }
        public List<string> DataCbxMonth { get; set; }
        public List<string> DataCbxYear
        {
            get => dataCbxYear; set { dataCbxYear = value; OnPropertyChanged(nameof(dataCbxYear)); }
        }
        #region Command
        public ICommand Search { get; set; }
        public ICommand AddEmployee { get; set; }
        public ICommand OpenAddEmployee { get; set; }
        public ICommand DeleteEmployee { get; set; }
        public ICommand OpenUpdateEmployee { get; set; }
        public ICommand OpenSalaryWindow { get; set; }
        public ICommand CheckItem { get; set; }
        public ICommand RefreshData { get; set; }
        public ICommand ImportExcel { get; set; }
        public ICommand ExportExcel { get; set; }
        public ICommand IncreaseDay { get; set; }
        public ICommand DecreaseDay { get; set; }



        #endregion

        public EmployeeViewModel()
        {
            employeeList = new ObservableCollection<ViewEmployee>();
            EmployeeTypeList = new List<LoaiNhanVien>();
            OpenAddEmployee = new RelayCommand<System.Windows.Window>((p) => { return true; }, (p) => { openAddEmployee(p); });
            DeleteEmployee = new RelayCommand<object>((p) => { return true; }, (p) => { deleteEmployee(p); });
            OpenUpdateEmployee = new RelayCommand<object>((p) => { return true; }, (p) => { openUpdateEmployee(p); });
            OpenSalaryWindow = new RelayCommand<object>((p) => { return true; }, (p) => { openSalaryWindow(p); });
            CheckItem = new RelayCommand<ListView>((p) => { return true; }, (p) => { checkSelectItem(p); });
            Search = new RelayCommand<object>((p) => { return true; }, (p) => { searchItem(p); });
            RefreshData = new RelayCommand<object>((p) => { return true; }, (p) => { refreshListEmployee(p); });
            IncreaseDay = new RelayCommand<object>((p) => { return true; }, (p) => { increaseDay(p); });
            DecreaseDay = new RelayCommand<object>((p) => { return true; }, (p) => { decreaseDay(p); });
            ExportExcel = new RelayCommand<DataGrid>((p) => { return true; }, (p) => { ExportFileExcel(p); });
            ImportExcel = new RelayCommand<DataGrid>((p) => { return true; }, (p) => { ImportFileExcel(p); });
            loadData();
        }
        void loadData()
        {
            loadDataEmployee();
        }

        void checkSelectItem(ListView selectedItemsp)
        {
            System.Collections.IList list = selectedItemsp.SelectedItems;
            for (int j = 0; j < EmployeeList.Count; j++)
            {
                EmployeeList[j].check_selected_item = false;
                for (int i = 0; i < list.Count; i++)
                {
                    if (EmployeeList[j].ma_nv == (list[i] as ViewEmployee).ma_nv)
                    {
                        EmployeeList[j].check_selected_item = true;
                    }
                }
            }
        }
        void refreshListEmployee(object p)
        {
            loadData();
        }
        void searchItem(object p)
        {
            List<NhanVien> list = DataProvider.Ins.DB.NhanViens.Where(t => t.ho_ten.ToLower().Contains(SearchKey.ToLower())).ToList();
            EmployeeList = getViewEmployeeFromList(list);
        }
        void increaseDay(object p)
        {
        
           for(int i=0;i<EmployeeList.Count;i++)
            {
                if (SelectedEmployee.ma_loai_nhan_vien == 1)
                {
                    return;
                }
                if (EmployeeList[i].check_selected_item == true)
                {
                    int m = EmployeeList[i].ma_nv;
                    NhanVien nv = DataProvider.Ins.DB.NhanViens.
                    Where(t => t.ma_nhan_vien == m).SingleOrDefault();
                    nv.so_ngay_nghi++;
                }
            }
            DataProvider.Ins.DB.SaveChanges();
            loadData();
        }
        void decreaseDay(object p)
        {
            for (int i = 0; i < EmployeeList.Count; i++)
            {
                if (SelectedEmployee.ma_loai_nhan_vien == 1)
                {
                    return;
                }
                if (EmployeeList[i].check_selected_item == true)
                {
                    int m = EmployeeList[i].ma_nv;
                    NhanVien nv = DataProvider.Ins.DB.NhanViens.
                      Where(t => t.ma_nhan_vien == m).SingleOrDefault();
                    if (nv.so_ngay_nghi > 0)
                    {
                        if (SelectedEmployee != null)
                        {
                            nv.so_ngay_nghi--;
                        }
                    }
                    DataProvider.Ins.DB.SaveChanges();
                }
            }
            loadData();
        }
        bool checkNumberOfEmployee()
        {
            int countitem = 0;
            for (int i = 0; i < EmployeeList.Count; i++)
            {
                if (EmployeeList[i].check_selected_item == true)
                {
                    countitem++;
                }
            }
            if (countitem != 1)
            {
                MessageBox.Show("Vui long chon 1 nhan vien");
                return false;
            }
            return true;
        }
        public void loadDataEmployee()
        {
            EmployeeTypeList.Clear();
            List<NhanVien> lNv = DataProvider.Ins.DB.NhanViens.ToList();
            EmployeeList = getViewEmployeeFromList(lNv);
            Dtg_employeeList = new ObservableCollection<ViewEmployee>(EmployeeList);
            loadDataCbxEmployeeType();
        }
        public void loadDataTypeEmployee(object p)
        {
            EmployeeTypeList.Clear();
            List<NhanVien> lNv = DataProvider.Ins.DB.NhanViens.ToList();
            EmployeeList = getViewEmployeeFromList(lNv);
        }
        void loadDataCbxEmployeeType()
        {
            EmployeeTypeList = DataProvider.Ins.DB.LoaiNhanViens.ToList();
        }
        void ImportFileExcel(object p)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "Excel files|*.xls;*.xlsx;*.xlsm";
            //Create COM Objects. Create a COM object for everything that is referenced
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(openFileDialog.FileName);
                Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;

                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;
                //iterate over the rows and columns and print to the console as it appears in the file
                //excel is not zero based!!
                for (int i = 2; i <= rowCount; i++)
                {
                    NhanVien add = new NhanVien();
                    for (int j = 2; j <= colCount; j++)
                    {
                        //write the value to the console
                        if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                        {

                            switch (j)
                            {
                                case 2:
                                    {
                                        add.ho_ten = xlRange.Cells[i, j].Value2.ToString();
                                        break;
                                    }
                                case 3:
                                    {

                                        add.ngay_sinh = DateTime.FromOADate(xlRange.Cells[i, j].Value2);
                                        break;
                                    }
                                case 4:
                                    {
                                        add.sdt = xlRange.Cells[i, j].Value2.ToString();
                                        break;
                                    }
                                case 5:
                                    {
                                        add.dia_chi = xlRange.Cells[i, j].Value2.ToString();
                                        break;
                                    }
                                case 6:
                                    {
                                        add.so_ngay_nghi = Int32.Parse(xlRange.Cells[i, j].Value2.ToString());
                                        break;
                                    }
                                case 7:
                                    {
                                        add.ma_loai_nhan_vien = Int32.Parse(xlRange.Cells[i, j].Value2.ToString());
                                        break;
                                    }
                                case 8:
                                    {
                                        add.ngay_vao_lam = DateTime.FromOADate(xlRange.Cells[i, j].Value2);
                                        break;
                                    }
                            }
                        }
                    }
                    AddImportedEmployee(add);
                }
                loadData();
                //cleanup
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Marshal.ReleaseComObject(xlRange);
                Marshal.ReleaseComObject(xlWorksheet);
                //close and release
                xlWorkbook.Close();
                Marshal.ReleaseComObject(xlWorkbook);
                //quit and release
                xlApp.Quit();
                Marshal.ReleaseComObject(xlApp);

            }
        }
        void AddImportedEmployee(NhanVien add)
        {
            QuyDinh QDinh = DataProvider.Ins.DB.QuyDinhs.Select(t => t).FirstOrDefault();
            List<NhanVien> List = DataProvider.Ins.DB.NhanViens.Select(t => t).ToList();
            bool checktype = false;
            if (add.ho_ten == null)
            {
                return;
            }
            if (add.sdt == null)
            {
                return;
            }
            else
            {
                add.sdt = "0" + add.sdt;
                for (int i = 0; i < EmployeeList.Count; i++)
                {

                    if (add.sdt == EmployeeList[i].sdt)
                    {
                        return;
                    }
                }
            }
            if (add.ngay_sinh == null)
            {
                return;
            }
            if (add.sdt.All(char.IsDigit) == false)
            {
                return;
            }
            if (add.ngay_sinh.Value.Year < 1900)
            {
                return;
            }
            int tuoi = DateTime.Now.Year - add.ngay_sinh.Value.Year;
            if (tuoi < QDinh.tuoi_toi_thieu_nv || tuoi > QDinh.tuoi_toi_da_nv)
            {
                return;
            }
            if (add.ma_loai_nhan_vien == 0)
            {
                return;
            }
            for (int i = 0; i < EmployeeTypeList.Count; i++)
            {
                if(add.ma_loai_nhan_vien == EmployeeTypeList[i].ma_loai_nhan_vien)
                {
                    checktype = true;
                }
            }
            if (List.Count == 0)
            {
                add.ma_nhan_vien = 1;
            }
            else
            {
                add.ma_nhan_vien = List[List.Count - 1].ma_nhan_vien + 1;
            }
            if (checktype == false)
            {
                return;
            }
            if (add.ngay_vao_lam.Year < 2000)
            {
                return;
            }
            DataProvider.Ins.DB.NhanViens.Add(add);
            DataProvider.Ins.DB.SaveChanges();
        }
        void ExportFileExcel(DataGrid dtGrid)
        {
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Visible = true;
            Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Worksheet sheet1 = (Worksheet)workbook.Sheets[1];
            int col = 8;
            for (int j = 0; j < col; j++)
            {
                Range myRange = (Range)sheet1.Cells[1, j + 1];
                sheet1.Cells[1, j + 1].Font.Bold = true;
                sheet1.Columns[j + 1].ColumnWidth = 15;
                myRange.Value2 = dtGrid.Columns[j].Header;
            }
            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < dtGrid.Items.Count; j++)
                {
                    TextBlock b = dtGrid.Columns[i].GetCellContent(dtGrid.Items[j]) as TextBlock;
                    if (b != null)
                    {
                        Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 2, i + 1];
                        myRange.Value2 = b.Text;
                    }
                }
            }
        }
        ObservableCollection<ViewEmployee> getViewEmployeeFromList(List<NhanVien> listNv)
        {
            ObservableCollection<ViewEmployee> obNv = new ObservableCollection<ViewEmployee>();
            int index = 1;
            foreach (var employee in listNv)
            {
                if (employee != null)
                {
                    LoaiNhanVien employeeType = DataProvider.Ins.DB.LoaiNhanViens.Where(p => p.ma_loai_nhan_vien == employee.ma_loai_nhan_vien).FirstOrDefault<LoaiNhanVien>();
                    ViewEmployee viewEmployee = new ViewEmployee()
                    {
                        STT = index,
                        ho_ten = employee.ho_ten,
                        dia_chi = employee.dia_chi,
                        ngay_sinh = (DateTime)employee.ngay_sinh,
                        sdt = employee.sdt,
                        ngay_vao_lam = employee.ngay_vao_lam,
                        ma_nv = employee.ma_nhan_vien,
                        so_ngay_nghi = (employee.so_ngay_nghi == null ? 0 : (int)employee.so_ngay_nghi)
                    };
                    if (employeeType != null)
                    {
                        viewEmployee.tien_luong = employeeType.tien_luong.ToString();
                        viewEmployee.tien_luong = MoneyConverter.convertMoney(viewEmployee.tien_luong);
                        viewEmployee.ma_loai_nhan_vien = employeeType.ma_loai_nhan_vien;
                        viewEmployee.VisiblePartTime = Visibility.Visible;
                        if (employeeType.ma_loai_nhan_vien == 1)
                        {
                            viewEmployee.so_ngay_nghi = 0;
                            viewEmployee.VisiblePartTime = Visibility.Hidden;
                        }
                        viewEmployee.loai_nhan_vien = employeeType.ten_loai_nhan_vien;
                    }
                    obNv.Add(viewEmployee);
                    index++;
                }
            }
            return obNv;
        }
        void openAddEmployee(System.Windows.Window p)
        {
            var a = new AddEmployeeWindow(this);
            a.ShowDialog();
        }
        void deleteEmployee(object p)
        {
            bool checkitem = false;
            for (int i = 0; i < EmployeeList.Count; i++)
            {
                if (EmployeeList[i].check_selected_item == true)
                {
                    int v = EmployeeList[i].ma_nv;
                    var delEmployee = DataProvider.Ins.DB.NhanViens.
                        Where(employee => employee.ma_nhan_vien == v).FirstOrDefault();
                    List<CaLamPartTime> LCalam = DataProvider.Ins.DB.CaLamPartTimes.Where(t => t.ma_nhan_vien == delEmployee.ma_nhan_vien).ToList();
                    for (int j = 0; j < LCalam.Count; j++)
                    {
                        DataProvider.Ins.DB.CaLamPartTimes.Remove(LCalam[j]);
                        DataProvider.Ins.DB.SaveChanges();
                    }
                    DataProvider.Ins.DB.NhanViens.Remove(delEmployee);
                    DataProvider.Ins.DB.SaveChanges();
                    checkitem = true;
                }
            }
            if (checkitem == true)
            {
                loadDataEmployee();
            }
            else
            {
                if (SelectedEmployee != null)
                {
                    var delEmployee = DataProvider.Ins.DB.NhanViens.
                      Where(employee => employee.ma_nhan_vien == SelectedEmployee.ma_nv).FirstOrDefault();
                    List<CaLamPartTime> LCalam = DataProvider.Ins.DB.CaLamPartTimes.Where(t => t.ma_nhan_vien == delEmployee.ma_nhan_vien).ToList();
                    for (int i = 0; i < LCalam.Count; i++)
                    {
                        DataProvider.Ins.DB.CaLamPartTimes.Remove(LCalam[i]);
                        DataProvider.Ins.DB.SaveChanges();
                    }
                    DataProvider.Ins.DB.NhanViens.Remove(delEmployee);
                    DataProvider.Ins.DB.SaveChanges();
                    loadDataEmployee();
                }
            }

        }
        void openUpdateEmployee(object p)
        {
            if(checkNumberOfEmployee() == false)
            {
                return;
            }
            if (SelectedEmployee != null)
            {
                var window = new UpdateEmployeeWindow(this);

                window.ShowDialog();
            }
        }
        void openSalaryWindow(object p)
        {
            SalaryWindow Swindow = new SalaryWindow();
            Swindow.ShowDialog();
            loadData();
        }

    }
}
