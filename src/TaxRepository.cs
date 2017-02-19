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
        decimal GetAllTaxes();
        decimal GetTax(string city, DateTime date);
    }

    public class TaxRepository : ITaxRepository
    {
        private Dictionary<string, YearTax> _cityTaxes;

        public TaxRepository()
        {
            _cityTaxes = new Dictionary<string, YearTax>();
            AddYearlyTax("b", 2016, 0.2m);
        }

        public void AddYearlyTax(string city, int year, decimal tax)
        {
            YearTax value = null;
            if (_cityTaxes.TryGetValue(city, out value))
            {
                value.YearlyTax = tax;
            }
            else
            {
                _cityTaxes.Add(city, new YearTax() { YearlyTax = tax });
            }
            
        }

        public decimal GetTax(string city, DateTime date)
        {
            YearTax taxByYear = null;
            if (_cityTaxes.TryGetValue(city, out taxByYear) == false)
            {
                return 0m;
            }
            else
            {
                // TODO upgrade logic
                return taxByYear.YearlyTax;
            }
        }

        public decimal GetAllTaxes()
        {
            return 555;
        }
    }
}
