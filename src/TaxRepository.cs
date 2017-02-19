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
            var yearlyTax = GetYearlyTaxesOrInit(city, year);
            yearlyTax.YearlyTax = tax;
        }

        public void AddMonthlyTax(string city, int year, int month, decimal tax)
        {
            var yearlyTax = GetYearlyTaxesOrInit(city, year);

            decimal monthlyTax;
            if (yearlyTax.MonthTaxes.TryGetValue(month, out monthlyTax))
            {
                yearlyTax.MonthTaxes.Remove(month);
            }
            yearlyTax.MonthTaxes.Add(month, tax);
        }

        public void AddDailyTax(string city, int year, int dayOfYear, decimal tax)
        {
            var yearlyTax = GetYearlyTaxesOrInit(city, year);

            decimal daylyTax;
            if (yearlyTax.DayTaxes.TryGetValue(dayOfYear, out daylyTax))
            {
                yearlyTax.DayTaxes.Remove(dayOfYear);
            }
            yearlyTax.DayTaxes.Add(dayOfYear, tax);
        }

        public decimal GetTax(string city, DateTime date)
        {
            CityTax cityTaxes = null;
            if (_cityTaxesCache.TryGetValue(city, out cityTaxes) == false)
            {
                return 0m;
            }

            // TODO upgrade logic: weeks
            YearTax cityTax;
            if (cityTaxes.YearTaxes.TryGetValue(date.Year, out cityTax))
            {
                decimal dailyTax, monthlyTax;
                if (cityTax.DayTaxes.TryGetValue(date.DayOfYear, out dailyTax))
                {
                    return dailyTax;
                }
                return cityTax.MonthTaxes.TryGetValue(date.Month, out monthlyTax) ? monthlyTax : cityTax.YearlyTax;
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

        private YearTax GetYearlyTaxesOrInit(string city, int year, decimal tax = 0m)
        {
            var cityTaxes = GetCityTaxes(city);

            YearTax yearlyTax;
            if (cityTaxes.YearTaxes.TryGetValue(year, out yearlyTax))
            {
                return yearlyTax;
            }
            yearlyTax = new YearTax() {YearlyTax = tax};
            cityTaxes.YearTaxes.Add(year, yearlyTax);
            return yearlyTax;
        }

    }
}
