using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStoreManager.Models
{
    public class LineChartModel
    {
        public decimal Value { get; set; }
        public string Month { get; set; }
        public LineChartModel(decimal X, int Y)
        {
            Value = X;
            Month = "Tháng " + Y.ToString();
        }
    }
}
