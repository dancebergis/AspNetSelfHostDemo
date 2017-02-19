using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetSelfHostDemo
{
    public interface ITaxRepository
    {
        decimal GetAllTaxes();
    }

    public class TaxRepository : ITaxRepository
    {
        public decimal GetAllTaxes()
        {
            return 555;
        }
    }
}
