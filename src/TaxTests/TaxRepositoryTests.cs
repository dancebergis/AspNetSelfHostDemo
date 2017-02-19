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
        public void AddsYearlyTaxCorrectly_ForDifferentCities()
        {
            _sut.AddYearlyTax("a", 2016, 0.1m);
            _sut.AddYearlyTax("b", 2016, 0.7m);

            var result = _sut.GetTax("a", DateTime.Parse("2016.02.02"));
            Assert.That(result, Is.EqualTo(0.1m));

            result = _sut.GetTax("b", DateTime.Parse("2016.12.02"));
            Assert.That(result, Is.EqualTo(0.7m));
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

        [Test]
        public void AddsMonthlyTax_WhenYearlyTaxIsPresent()
        {
            _sut.AddYearlyTax("a", 2016, 0.1m);
            _sut.AddMonthlyTax("a", 2016, 5, 0.2m);
            _sut.AddMonthlyTax("b", 2016, 4, 0.5m);
            var result = _sut.GetTax("a", DateTime.Parse("2016.05.02"));
            Assert.That(result, Is.EqualTo(0.2m));

            result = _sut.GetTax("a", DateTime.Parse("2016.04.02"));
            Assert.That(result, Is.EqualTo(0.1m));

            result = _sut.GetTax("a", DateTime.Parse("2016.06.02"));
            Assert.That(result, Is.EqualTo(0.1m));
        }

        [Test]
        public void AddsDailyTax()
        {
            _sut.AddYearlyTax("a", 2016, 0.1m);
            _sut.AddMonthlyTax("a", 2016, 5, 0.2m);
            _sut.AddMonthlyTax("a", 2016, 7, 0.5m);
            _sut.AddDailyTax("a", 2016, DateTime.Parse("2016.07.07").DayOfYear, 0.7m);
            _sut.AddDailyTax("a", 2016, 40, 0.8m);
            var result = _sut.GetTax("a", DateTime.Parse("2016.05.02"));
            Assert.That(result, Is.EqualTo(0.2m));

            result = _sut.GetTax("a", DateTime.Parse("2016.07.06"));
            Assert.That(result, Is.EqualTo(0.5m));

            result = _sut.GetTax("a", DateTime.Parse("2016.07.08"));
            Assert.That(result, Is.EqualTo(0.5m));

            result = _sut.GetTax("a", DateTime.Parse("2016.07.07"));
            Assert.That(result, Is.EqualTo(0.7m));

            result = _sut.GetTax("a", DateTime.Parse("2016.02.09"));
            Assert.That(result, Is.EqualTo(0.8m));
        }

    }
}