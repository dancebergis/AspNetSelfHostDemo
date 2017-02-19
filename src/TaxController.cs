using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AspNetSelfHostDemo.Entities;

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
            try
            {
                var result = _taxService.GetTax(municipality, date);
                return result == 0 ? "No taxes for this date found" : $"Tax for {municipality}: {result}";
            }
            catch (ArgumentException e)
            {
                StatusCode(HttpStatusCode.BadRequest);
                return e.Message;
            }
            catch (Exception e)
            {
                StatusCode(HttpStatusCode.InternalServerError);
                return $"Sorry, smth unexpected & unhandled happened. Details: {e.Message}";
            }
        }

        public string Post([FromBody] TaxRecord taxRecord)
        {
            try
            {
                _taxService.UpdateTaxes(taxRecord);
                return "OK";
            }
            catch (ArgumentException e)
            {
                StatusCode(HttpStatusCode.BadRequest);
                return e.Message;
            }
        }
    }
}
