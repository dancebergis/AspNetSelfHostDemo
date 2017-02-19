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

        [Test]
        public void AddsYearlyTaxCorrectly_WhenYearlyTaxIsAlreadyPresent()
        {
            _sut.AddYearlyTax("a", 2016, 0.1m);
            _sut.AddYearlyTax("a", 2016, 0.5m);
            var result = _sut.GetTax("a", DateTime.Parse("2016.02.02"));

            Assert.That(result, Is.EqualTo(0.5m));
        }

        [Test]
        public void AddsMonthlyTax_WhenYearlyTaxIsNotPresent()
        {
            _sut.AddMonthlyTax("a", 2016, 5, 0.1m);
            var result = _sut.GetTax("a", DateTime.Parse("2016.05.02"));

            Assert.That(result, Is.EqualTo(0.1m));
        }
    }
}