using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CoffeeStoreManager.Models;
using Syncfusion.UI.Xaml.Scheduler;

namespace CoffeeStoreManager.ViewModels
{
    class PartTimeShiftEditorViewModel: BaseViewModel
    {
        public ObservableCollection<NhanVien> PartTimeEmployeeList { get => partTimeEmployeeList; set { partTimeEmployeeList = value; OnPropertyChanged(nameof(PartTimeEmployeeList)); } }
        private ObservableCollection<NhanVien> partTimeEmployeeList;

        public NhanVien SelectedPartTimeEmployee { get => selectedPartTimeEmployee; set { selectedPartTimeEmployee = value; OnPropertyChanged(nameof(SelectedPartTimeEmployee)); } }
        private NhanVien selectedPartTimeEmployee;

        public DateTime ShiftDate { get => shiftDate; set { shiftDate = value; OnPropertyChanged(nameof(ShiftDate)); } }

        public DateTime ShiftFrom { get => shiftFrom; set { shiftFrom = value; OnPropertyChanged(nameof(ShiftFrom)); } }

        public DateTime ShiftTo { get => shiftTo; set { shiftTo = value; OnPropertyChanged(nameof(ShiftTo)); } }

        private DateTime shiftTo;
        private DateTime shiftDate;
        private DateTime shiftFrom;

        public ICommand SavePartTimeShift { get; set; }
        public ICommand DeletePartTimeShift { get; set; }

        ScheduleAppointment shift;
        DateTime selectedCellDateTime;
        bool isNullCell;

        PartTimeScheduleViewModel scheduleVm;
        public PartTimeShiftEditorViewModel(PartTimeScheduleViewModel scheduleVM, ScheduleAppointment appointment, DateTime cellDateTime)
        {
            this.scheduleVm = scheduleVM;
            this.shift = appointment;
            this.selectedCellDateTime = cellDateTime;
            this.isNullCell = false;

            SavePartTimeShift = new RelayCommand<object>((p) => { return true; }, (p) => { savePartTimeShift(p); });
            DeletePartTimeShift = new RelayCommand<object>((p) => { return true; }, (p) => { deletePartTimeShift(p); });

            loadPartTimeEmployeesSelector();
            loadEditor();
        }

        private void loadEditor()
        {
            if (shift != null)
            {
                loadEditorWithExistedCell(shift);
                isNullCell = false;
            }
            else
            {
                loadEditorWithNullCell(selectedCellDateTime);
                isNullCell = true;
            }
        }
        private void loadPartTimeEmployeesSelector()
        {
            //partTimeEmployees = new List<NhanVien>();
            var employeesData = DataProvider.Ins.DB.NhanViens.Where(employee => employee.ma_loai_nhan_vien == 1).ToList();
            PartTimeEmployeeList = new ObservableCollection<NhanVien>(employeesData);
        }
        private void loadEditorWithExistedCell(ScheduleAppointment shift)
        {
            NhanVien targetEmployee = PartTimeEmployeeList.Where<NhanVien>(employee => employee.ho_ten == shift.Subject).ToList()[0];
            SelectedPartTimeEmployee = targetEmployee;
            ShiftDate = shift.StartTime.Date;
            ShiftFrom = shift.StartTime;
            ShiftTo = shift.EndTime;
        }
        private void loadEditorWithNullCell(DateTime dateTime)
        {
            ShiftDate = dateTime.Date;
            ShiftFrom = dateTime;
            ShiftTo = dateTime.AddHours(1);
        }
        private PartTimeShift exportShift()
        {
            PartTimeShift shift = new PartTimeShift()
            {
                Ten_nhan_vien = SelectedPartTimeEmployee.ho_ten,
                Ma_nhan_vien = SelectedPartTimeEmployee.ma_nhan_vien,
                Bat_dau = ShiftFrom,
                Ket_thuc = ShiftTo,
            };
            return shift;
        }
        //command
        private void savePartTimeShift(object p)
        {
            PartTimeShift editorShift = exportShift();
            if (isNullCell)
            {
                this.scheduleVm.AddPartTimeShift(editorShift);
            }
            else
            {
                editorShift.Ma_ca_partTime = (int)this.shift.Id;
                this.scheduleVm.UpdatePartTimeShift(editorShift);
            }
        }
        private void deletePartTimeShift(object p)
        {
            int shiftId = (int)this.shift.Id;
            this.scheduleVm.DeleteShift(shiftId);
        }
    }
}
