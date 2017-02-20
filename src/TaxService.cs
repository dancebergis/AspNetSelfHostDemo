using System;
using System.Globalization;
using AspNetSelfHostDemo.Entities;

namespace AspNetSelfHostDemo
{
    public interface ITaxService
    {
        decimal GetTax(string city, string dateString);

        void UpdateTaxes(TaxRecord taxRecord);
    }

    public class TaxService : ITaxService
    {
        private readonly ITaxRepository _taxRepository;

        public TaxService(ITaxRepository taxRepository)
        {
            _taxRepository = taxRepository;
        }

        public decimal GetTax(string city, string dateString)
        {
            try
            {
                var date = DateTime.Parse(dateString);
                return _taxRepository.GetTax(city, date);
            }
            catch (Exception e)
            {
                throw new ArgumentException($"Invalid date provided ({dateString})");
            }
        }

        public void UpdateTaxes(TaxRecord taxRecord)
        {
            if (taxRecord == null)
            {
                throw new ArgumentException("Input data is invalid. See manual (Readme) for expected input formats");
            }
            if (string.IsNullOrEmpty(taxRecord.City))
            {
                throw new ArgumentException("City property is missing or invalid");
            }
            if (taxRecord.Tax.HasValue == false || taxRecord.Tax.Value == 0)
            {
                throw new ArgumentException("Tax must have a non-zero value");
            }
            if (string.IsNullOrEmpty(taxRecord.Day) == false)
            {
                DateTime dayDateTime;
                var parseResult = DateTime.TryParse(taxRecord.Day, out dayDateTime);
                if (!parseResult)
                    throw new ArgumentException("Day property is invalid. Expected date in acceptable format (e.g. YYYY.MM.DD)");

                if (taxRecord.Year.HasValue || taxRecord.Month.HasValue || taxRecord.WeekOfYear.HasValue)
                    throw new ArgumentException("Only one type of tax can be entered in one request.");

                _taxRepository.AddDailyTax(taxRecord.City, dayDateTime, taxRecord.Tax.Value);
                return;
            }


            var year = taxRecord.Year ?? -1;
            if (year < 0)
            {
                throw new ArgumentException("Year property is missing or invalid");
            }
            
            if (taxRecord.Month.HasValue)
            {
                if (taxRecord.Month.Value < 1 || taxRecord.Month.Value > 12)
                    throw new ArgumentException("Month property is invalid. Valid range: 1-12");
                if (taxRecord.WeekOfYear.HasValue)
                    throw new ArgumentException("Only one type of tax can be entered in one request.");
                _taxRepository.AddMonthlyTax(taxRecord.City, year, taxRecord.Month.Value, taxRecord.Tax.Value);
                return;
            }
            if (taxRecord.WeekOfYear.HasValue)
            {
                var weekOfYear = taxRecord.WeekOfYear.Value;
                var lastWeekOfYear = CalendarHelper.GetWeekOfYear(new DateTime(year, 12, 31));
                if (weekOfYear < 1 || weekOfYear > lastWeekOfYear)
                    throw new ArgumentException("Week property is invalid. Valid range: 1..52 (or 53, or 54 - depends on year)");
                _taxRepository.AddWeeklyTax(taxRecord.City, year, weekOfYear, taxRecord.Tax.Value);
            }

            _taxRepository.AddYearlyTax(taxRecord.City, year, taxRecord.Tax.Value);
        }
    }
}
