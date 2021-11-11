using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CoffeeStoreManager.Models;

namespace CoffeeStoreManager.ViewModels
{
    public class PartTimeScheduleViewModel: BaseViewModel
    {
        public ObservableCollection<PartTimeShift> ShiftList { get => shiftList; set { shiftList = value; OnPropertyChanged(nameof(ShiftList)); } }

        private ObservableCollection<PartTimeShift> shiftList;

        public PartTimeScheduleViewModel()
        {
            loadShiftList();
        }

        public void loadShiftList()
        {
            List<CaLamPartTime> dbShifts = DataProvider.Ins.DB.CaLamPartTimes.ToList();
            
            ShiftList = new ObservableCollection<PartTimeShift>();
            foreach(CaLamPartTime dbShift in dbShifts)
            {
                PartTimeShift shift = PartTimeShift.fromDbShift(dbShift);
                ShiftList.Add(shift);
            }
        }
        public void AddPartTimeShift(PartTimeShift shift)
        {
            int hourDiff = shift.Ket_thuc.TimeOfDay.Hours - shift.Bat_dau.TimeOfDay.Hours;
            CaLamPartTime dbShift = CaLamPartTime.fromShift(shift);
            DataProvider.Ins.DB.CaLamPartTimes.Add(dbShift);
            DataProvider.Ins.DB.SaveChanges();

            shift.Ma_ca_partTime = dbShift.ma_ca_partTime;
            //get shift id after insert to db
            ShiftList.Add(shift);
        }
        public void UpdatePartTimeShift(PartTimeShift shift)
        {
            CaLamPartTime dbShift = DataProvider.Ins.DB.CaLamPartTimes.Where(s => s.ma_ca_partTime == shift.Ma_ca_partTime).Single();
            updateDateDbShift(ref dbShift, shift);
            updateShiftInList(shift);
        }
        private void updateDateDbShift(ref CaLamPartTime dbShift, PartTimeShift shift)
        {
            int hourDiff = shift.Ket_thuc.TimeOfDay.Hours - shift.Bat_dau.TimeOfDay.Hours;
            dbShift.ma_nhan_vien = shift.Ma_nhan_vien;
            dbShift.ngay_lam = shift.Bat_dau.Date;
            dbShift.gio_bat_dau = shift.Bat_dau.TimeOfDay;
            dbShift.gio_ket_thuc = shift.Ket_thuc.TimeOfDay;
            dbShift.so_gio_lam = hourDiff;
            DataProvider.Ins.DB.SaveChanges();
        }
        private void updateShiftInList(PartTimeShift shift)
        {
            PartTimeShift oldShift = ShiftList.FirstOrDefault(s => s.Ma_ca_partTime == shift.Ma_ca_partTime);
            oldShift.Ma_nhan_vien = shift.Ma_nhan_vien;
            oldShift.Bat_dau = shift.Bat_dau;
            oldShift.Ket_thuc = shift.Ket_thuc;
            oldShift.Ten_nhan_vien = shift.Ten_nhan_vien;
            
        }

        public void DeleteShift(int shiftId)
        {
            deleteDbShift(shiftId);
            deleteShiftInList(shiftId);
        }
        private void deleteDbShift(int shiftId)
        {
            var dbContext = DataProvider.Ins.DB;
            CaLamPartTime dbShift = dbContext.CaLamPartTimes.Where(s => s.ma_ca_partTime == shiftId).Single();
            dbContext.CaLamPartTimes.Remove(dbShift);
            dbContext.SaveChanges();
        }
        private void deleteShiftInList(int shiftId)
        {
            PartTimeShift target = ShiftList.FirstOrDefault(s => s.Ma_ca_partTime == shiftId);
            ShiftList.Remove(target);
        }
            
    }
}
