using System;
using System.Collections.Generic;
using System.Windows.Controls;
using CoffeeStoreManager.Views.Account;
using CoffeeStoreManager.Views.Dashboard;
using CoffeeStoreManager.Views.ManageEmployee;
using CoffeeStoreManager.Views.ManageFood;
using CoffeeStoreManager.Views.MangeSource.Item;
using CoffeeStoreManager.Views.MonthReport;
using CoffeeStoreManager.Views.PartTimeScheduler;
using CoffeeStoreManager.Views.Regulation;
using CoffeeStoreManager.Views.Statistic;

namespace CoffeeStoreManager.Views
{
    static class Pages
    {
        public static List<Page> pages = new List<Page>();
        public static Page FoodListPage { get => pages[0]; }
        public static Page FoodTypePage { get => pages[1]; }
        public static Page EmployeePage { get => pages[2]; }
        public static Page EmployeeTypePage { get => pages[3]; }
        public static Page PartTimeSchedulerPage { get => pages[4]; }
        public static Page SourcePage { get => pages[5]; }
        public static Page StatisticRevenuePage { get => pages[6]; }
        public static Page StatisticFoodTypePage { get => pages[7]; }
        public static Page RegulationPage { get => pages[8]; }
        public static Page AccountPage { get => pages[9]; }
        public static Page DashboardPage { get => new DashboardMain(); }
        static Pages()
        {
            pages.Add(new ManageFoodMain());
            pages.Add(new FoodTypePage());
            //food
            pages.Add(new ManageEmployeeMain());
            pages.Add(new EmployeeTypeMain());

            pages.Add(new PartTimeSchedulerMain());
            //employee
            pages.Add(new ManageSourceMain());
            //source
            pages.Add(new StatisticMain());
            pages.Add(new MonthReportMain());
            //statistic
            pages.Add(new RegulationMain());
            //regulation
            pages.Add(new AccountMain());
            //account
            //pages.Add(new DashboardMain());

        }

    }
}
