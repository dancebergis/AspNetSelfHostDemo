using System;
using System.Collections.Generic;
using AspNetSelfHostDemo.Entities;

namespace AspNetSelfHostDemo
{
    public interface ITaxRepository
    {
        decimal GetTax(string city, DateTime date);
    }

    public class TaxRepository : ITaxRepository
    {
        private readonly Dictionary<string, CityTax> _cityTaxesCache;

        public TaxRepository()
        {
            _cityTaxesCache = new Dictionary<string, CityTax>();
        }

        public void AddYearlyTax(string city, int year, decimal tax)
        {
            var cityTaxes = GetCityTaxes(city);

            YearTax yearlyTax;
            if (cityTaxes.YearTaxes.TryGetValue(year, out yearlyTax))
            {
                yearlyTax.YearlyTax = tax;
            }
            else
            {
                yearlyTax = new YearTax {YearlyTax = tax};
                cityTaxes.YearTaxes.Add(year, yearlyTax);
            }
        }

        public void AddMonthlyTax(string city, int year, int month, decimal tax)
        {
            var cityTaxes = GetCityTaxes(city);

            YearTax yearlyTax;
            if (cityTaxes.YearTaxes.TryGetValue(year, out yearlyTax) == false)
            {
                yearlyTax = new YearTax();
                cityTaxes.YearTaxes.Add(year, yearlyTax);
            }

            decimal monthlyTax;
            if (yearlyTax.MonthTaxes.TryGetValue(month, out monthlyTax))
            {
                yearlyTax.MonthTaxes.Remove(month);
            }
            yearlyTax.MonthTaxes.Add(month, tax);
        }

        public decimal GetTax(string city, DateTime date)
        {
            CityTax cityTaxes = null;
            if (_cityTaxesCache.TryGetValue(city, out cityTaxes) == false)
            {
                return 0m;
            }

            // TODO upgrade logic: weeks, days
            YearTax cityTax;
            if (cityTaxes.YearTaxes.TryGetValue(date.Year, out cityTax))
            {
                decimal monthlyTax;
                if (cityTax.MonthTaxes.TryGetValue(date.Month, out monthlyTax))
                {
                    return monthlyTax;
                }
                return cityTax.YearlyTax;
            }
            return 0m;
        }

        private CityTax GetCityTaxes(string city)
        {
            CityTax cityTaxes;
            _cityTaxesCache.TryGetValue(city, out cityTaxes);
            if (cityTaxes == null)
            {
                cityTaxes = new CityTax();
                _cityTaxesCache.Add(city, cityTaxes);
            }
            return cityTaxes;
        }
    }
}
