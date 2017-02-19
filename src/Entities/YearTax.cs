using System.Collections.Generic;

namespace AspNetSelfHostDemo.Entities
{
    public class YearTax
    {
        public decimal YearlyTax { get; set; }
        public Dictionary<int, decimal> DayTaxes { get; set; }
        public Dictionary<int, decimal> WeekTaxes { get; set; }
        public Dictionary<int, decimal> MonthTaxes { get; set; }

        public YearTax()
        {
            DayTaxes = new Dictionary<int, decimal>();
            WeekTaxes = new Dictionary<int, decimal>();
            MonthTaxes = new Dictionary<int, decimal>();
        }
    }
}