using System;
using System.Globalization;
using System.Windows.Data;

namespace CoffeeStoreManager.Resources.Utils
{
    public class UIMoneyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN"); // try with "en-US"
            string a = double.Parse(value.ToString()).ToString("#,###", cul.NumberFormat);
            return a;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
