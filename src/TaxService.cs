using System;
using System.Globalization;

namespace AspNetSelfHostDemo
{
    public interface ITaxService
    {
        decimal GetTax(string city, string dateString);
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
                throw new ArgumentException("Invalid date provided " + e.GetType());
            }
        }
    }
}
