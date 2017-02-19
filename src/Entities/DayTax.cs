using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetSelfHostDemo.Entities
{
    public class YearTax
    {
        public decimal YearlyTax { get; set; }
        public Dictionary<int, decimal> DayTaxes { get; set; }
        public Dictionary<int, decimal> MonthTaxes { get; set; }

        public YearTax()
        {
            DayTaxes = new Dictionary<int, decimal>();
            MonthTaxes = new Dictionary<int, decimal>();
        }
    }

    public class CityTax
    {
        public string CityName { get; set; }
        public Dictionary<int, YearTax> YearTaxes { get; set; }
    }

}
