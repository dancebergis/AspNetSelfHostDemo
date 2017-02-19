using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNetSelfHostDemo.Entities;

namespace AspNetSelfHostDemo
{
    public interface ITaxRepository
    {
        decimal GetTax(string city, DateTime date);
    }

    public class TaxRepository : ITaxRepository
    {
        private Dictionary<string, YearTax> _cityTaxesCache;

        public TaxRepository()
        {
            _cityTaxesCache = new Dictionary<string, YearTax>();
            AddYearlyTax("b", 2016, 0.2m);
        }

        public void AddYearlyTax(string city, int year, decimal tax)
        {
            var cityTaxes = GetTaxData(city);
            if (cityTaxes == null)
            {
                _cityTaxesCache.Add(city, new YearTax() {YearlyTax = tax});
            }
            else
            {
                cityTaxes.YearlyTax = tax;
            }
        }

        public void AddMonthlyTax(string city, int year, int month, decimal tax)
        {
            var cityTaxes = GetTaxData(city);
            if (cityTaxes == null)
            {
                cityTaxes = new YearTax();
                _cityTaxesCache.Add(city, cityTaxes);
            }

            decimal monthlyTax;
            if (cityTaxes.MonthTaxes.TryGetValue(month, out monthlyTax))
            {
                cityTaxes.MonthTaxes.Remove(month);
            }
            cityTaxes.MonthTaxes.Add(month, tax);
        }

        public decimal GetTax(string city, DateTime date)
        {
            YearTax taxByYear = null;
            if (_cityTaxesCache.TryGetValue(city, out taxByYear) == false)
            {
                return 0m;
            }
            else
            {
                // TODO upgrade logic
                return taxByYear.YearlyTax;
            }
        }

        private YearTax GetTaxData(string city)
        {
            YearTax value = null;
            _cityTaxesCache.TryGetValue(city, out value);
            return value;
        }
    }
}
