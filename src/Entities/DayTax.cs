using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetSelfHostDemo.Entities
{
    class YearTax
    {
        public decimal Tax { get; set; }
        public Dictionary<int, decimal> DayTaxes { get; set; }
        public Dictionary<int, decimal> MonthTaxes { get; set; }
    }

    class CityTax
    {
        public string CityName { get; set; }
        public Dictionary<int, YearTax> YearTaxes { get; set; }
    }

}
