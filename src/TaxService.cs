using System;

namespace AspNetSelfHostDemo
{
    public interface ITaxService
    {
        decimal GetTax(string municipality, DateTime date);
    }

    public class TaxService : ITaxService
    {
        public decimal GetTax(string municipality, DateTime date)
        {
            return 5;
        }
    }
}
