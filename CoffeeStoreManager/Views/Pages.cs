using System;
using System.Collections.Generic;
using System.Windows.Controls;
using CoffeeStoreManager.Views.Account;
using CoffeeStoreManager.Views.ManageEmployee;
using CoffeeStoreManager.Views.ManageFood;
using CoffeeStoreManager.Views.MangeSource.Item;
using CoffeeStoreManager.Views.PartTimeScheduler;
using CoffeeStoreManager.Views.Statistic;

namespace CoffeeStoreManager.Views
{
    static class Pages
    {
        public static List<Page> pages = new List<Page>();
        public static Page FoodPage { get => pages[0]; }
        public static Page EmployeePage { get => pages[1]; }
        public static Page SourcePage { get => pages[2]; }
        public static Page StatisticPage { get => pages[3]; }
        public static Page AccountPage { get => pages[4]; }
        public static Page PartTimeSchedulerPage { get => pages[5]; }
        static Pages()
        {
            pages.Add(new ManageFoodMain());
            pages.Add(new ManageEmployeeMain());
            pages.Add(new ManageSourceMain());
            pages.Add(new StatisticMain());
            pages.Add(new AccountMain());
            pages.Add(new PartTimeSchedulerMain());
        }

    }
}
