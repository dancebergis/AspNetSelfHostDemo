using System;

namespace AspNetSelfHostDemo
{
    public class TaxManagerService
    {

        public TaxManagerService()
        {
        }

        public bool Start()
        {
            Console.WriteLine("TaxManager Service Started.");
            return true;
        }

        public bool Stop()
        {
            return true;
        }
    }
}