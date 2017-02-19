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
        }

        public void AddYearlyTax(string city, int year, decimal tax)
        {
            return;
        }

        public decimal GetTax(string city, DateTime date)
        {
            return 1m;
        }

        public decimal GetAllTaxes()
        {
            return 555;
        }
    }
}
