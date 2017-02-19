using System;
using System.Collections.Generic;

namespace AspNetSelfHostDemo.Entities
{
    public class CityTax
    {
        public CityTax()
        {
            YearTaxes = new Dictionary<int, YearTax>();
        }

        public Dictionary<int, YearTax> YearTaxes { get; set; }
    }

    public class TaxRecord
    {
        public string City { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? WeekOfYear { get; set; }
        public string Day { get; set; }
        public decimal? Tax { get; set; }
    }

}
