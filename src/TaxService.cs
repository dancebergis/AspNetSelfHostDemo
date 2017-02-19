using System;

namespace AspNetSelfHostDemo
{
    public interface ITaxService
    {
        decimal GetTax(string municipality, DateTime date);
    }

    public class TaxService : ITaxService
    {
        private readonly ITaxRepository _taxRepository;

        public TaxService(ITaxRepository taxRepository)
        {
            _taxRepository = taxRepository;
        }

        public decimal GetTax(string municipality, DateTime date)
        {
            return _taxRepository.GetAllTaxes();
        }
    }
}
