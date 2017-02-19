using System;
using System.Web.Http;

namespace AspNetSelfHostDemo
{
    public class TaxController : ApiController
    {
        private readonly ITaxService _taxService;

        public TaxController(ITaxService taxService)
        {
            _taxService = taxService;
        }

        // GET api/tax/Vilnius/2016.01.01
        public string Get(string municipality, string date)
        {
            return _taxService.GetTax(municipality, DateTime.UtcNow).ToString();
        }

        //// POST api/tax ???
        //public void Post([FromBody]string value)
        //{
        //}
    }
}
