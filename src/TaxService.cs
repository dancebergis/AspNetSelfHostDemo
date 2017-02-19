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
            if (string.IsNullOrEmpty(taxRecord.City))
            {
                throw new ArgumentException("City property is missing or invalid");
            }
            if (string.IsNullOrEmpty(taxRecord.Day) == false)
            {
                // TODO check for year/month/week input
                DateTime dayDateTime;
                var parseResult = DateTime.TryParse(taxRecord.Day, out dayDateTime);
                if (!parseResult)
                    throw new ArgumentException("Day property is invalid. Expected date in acceptable format (e.g. YYYY.MM.DD)");
            }


            if (taxRecord.Year.HasValue == false || taxRecord.Year.Value < 0)
            {
                throw new ArgumentException("Year property is missing or invalid");
            }
            // TODO if no month/week value, update by year


            // TODO check xor for month & week
            if (taxRecord.Month.HasValue && (taxRecord.Month.Value < 1 || taxRecord.Month.Value > 12))
            {
                throw new ArgumentException("Month property is invalid. Valid range: 1-12");
            }

        }
    }
}
