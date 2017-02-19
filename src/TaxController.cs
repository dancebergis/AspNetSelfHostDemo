using System.Web.Http;

namespace AspNetSelfHostDemo
{
    public class TaxController : ApiController
    {
        // GET api/tax/Vilnius/2016.01.01
        public string Get(string municipality, string date)
        {
            return municipality + date;
        }

        //// POST api/tax ???
        //public void Post([FromBody]string value)
        //{
        //}
    }
}
