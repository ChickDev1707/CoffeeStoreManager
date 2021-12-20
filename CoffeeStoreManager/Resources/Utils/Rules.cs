using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CoffeeStoreManager.Resources.Utils
{
    public class NonEmptyRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            if (value.ToString() == "") return new ValidationResult(false, "Không được để trống");
            return ValidationResult.ValidResult;
        }
    }
    public class PositiveNumberRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                decimal val = Convert.ToDecimal(value);
                if (val < 0) return new ValidationResult(false, "Giá trị phải lớn hơn 0");
                return ValidationResult.ValidResult;
            }
            catch
            {
                return new ValidationResult(false, "Dữ liệu phải là kiểu số");
            }
        }
    }
    public class TextRule : ValidationRule
    {
        private static readonly Regex regex = new Regex(@"^\d+$");
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            if (regex.IsMatch(value.ToString())) return new ValidationResult(false, "Dữ liệu phải là kiểu chuỗi");
            return ValidationResult.ValidResult;
        }
    }
    public class EmailRule : ValidationRule
    {
        private static readonly Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!regex.IsMatch(value.ToString())) return new ValidationResult(false, "Email không hợp lệ");
            return ValidationResult.ValidResult;
        }
    }

}
