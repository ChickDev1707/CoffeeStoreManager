
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using CoffeeStoreManager.Models;
using CoffeeStoreManager.ViewModels;
using Syncfusion.UI.Xaml.Scheduler;

namespace CoffeeStoreManager.Views.PartTimeScheduler
{
    /// <summary>
    /// Interaction logic for PartTimeShiftEditorWindow.xaml
    /// </summary>
    public partial class PartTimeShiftEditorWindow : Window
    {
        public PartTimeShiftEditorWindow(PartTimeScheduleViewModel scheduleVM, ScheduleAppointment appointment, DateTime cellDateTime) { 
            InitializeComponent();
            PartTimeShiftEditorViewModel editorVm = new PartTimeShiftEditorViewModel(scheduleVM, appointment, cellDateTime);
            this.DataContext = editorVm;
        }
        
    }
    
}
