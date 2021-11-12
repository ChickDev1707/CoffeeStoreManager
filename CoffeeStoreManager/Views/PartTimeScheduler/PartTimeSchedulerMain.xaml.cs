
using System.Windows.Controls;
using CoffeeStoreManager.ViewModels;
using Syncfusion.UI.Xaml.Scheduler;

namespace CoffeeStoreManager.Views.PartTimeScheduler
{
    /// <summary>
    /// Interaction logic for PartTimeSchedulerMain.xaml
    /// </summary>
    public partial class PartTimeSchedulerMain : Page
    {
        PartTimeScheduleViewModel vm;
        public PartTimeSchedulerMain()
        {
            InitializeComponent();
            vm = new PartTimeScheduleViewModel();
            this.DataContext = vm;
        }
        private void Scheduler_AppointmentEditorOpening(object sender, AppointmentEditorOpeningEventArgs e)
        {
            e.Cancel = true;
            var editor = new PartTimeShiftEditorWindow(vm, e.Appointment, e.DateTime);
            editor.ShowDialog();
        }
    }
}
