using System;
using AspNetSelfHostDemo;
using Moq;
using NUnit.Framework;

namespace TaxTests
{
    [TestFixture]
    public class TaxRepositoryTests
    {
        private TaxRepository _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new TaxRepository();
        }

        [Test]
        public void AddsYearlyTaxCorrectly()
        {
             _sut.AddYearlyTax("a", 2016, 0.1m);
            var result = _sut.GetTax("a", DateTime.Parse("2016.02.02"));

            Assert.That(result, Is.EqualTo(0.1m));
        }
    }
}