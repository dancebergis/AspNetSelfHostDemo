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
}
