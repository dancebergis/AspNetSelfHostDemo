using System;
using System.Globalization;
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


        [Test]
        public void AddsWeeklyTax()
        {
            _sut.AddYearlyTax("a", 2016, 0.1m);
            _sut.AddMonthlyTax("a", 2016, 2, 0.2m);
            _sut.AddDailyTax("a", 2016, DateTime.Parse("2016.02.10").DayOfYear, 0.5m);
            _sut.AddDailyTax("a", 2016, DateTime.Parse("2016.02.14").DayOfYear, 0.7m);
            _sut.AddWeeklyTax("a", 2016, 2, 2.1m);  //2nd week: 01.04 - 01.10
            _sut.AddWeeklyTax("a", 2016, 7, 2.2m);  //7th week: 02.08 - 02.14

            var result = _sut.GetTax("a", DateTime.Parse("2016.02.07"));
            Assert.That(result, Is.EqualTo(0.2m));

            result = _sut.GetTax("a", DateTime.Parse("2016.02.08"));
            Assert.That(result, Is.EqualTo(2.2m));

            result = _sut.GetTax("a", DateTime.Parse("2016.02.09"));
            Assert.That(result, Is.EqualTo(2.2m));

            result = _sut.GetTax("a", DateTime.Parse("2016.02.10"));    // daytax overrides weektax
            Assert.That(result, Is.EqualTo(0.5m));

            result = _sut.GetTax("a", DateTime.Parse("2016.02.11"));
            Assert.That(result, Is.EqualTo(2.2m));

            result = _sut.GetTax("a", DateTime.Parse("2016.02.13"));
            Assert.That(result, Is.EqualTo(2.2m));

            result = _sut.GetTax("a", DateTime.Parse("2016.02.14"));    // daytax overrides weektax (sunday)
            Assert.That(result, Is.EqualTo(0.7m));

            result = _sut.GetTax("a", DateTime.Parse("2016.01.03"));
            Assert.That(result, Is.EqualTo(0.1m));

            result = _sut.GetTax("a", DateTime.Parse("2016.01.04"));
            Assert.That(result, Is.EqualTo(2.1m));

            result = _sut.GetTax("a", DateTime.Parse("2016.01.10"));
            Assert.That(result, Is.EqualTo(2.1m));

            result = _sut.GetTax("a", DateTime.Parse("2016.01.11"));
            Assert.That(result, Is.EqualTo(0.1m));
        }

    
        [TestCase("2016.01.01", 1)]     // friday
        [TestCase("2016.01.03", 1)]     // sunday
        [TestCase("2016.01.04", 2)]     // next monday
        [TestCase("2016.01.10", 2)]     // next sunday
        [TestCase("2016.02.07", 6)]     // sunday of 6th week
        [TestCase("2016.02.08", 7)]     // monday of 7th week
        [TestCase("2015.12.31", 53)]
        
        public void TestCalendar(string date, int expectedWeekOfTheYear)
        {
            Calendar cal = DateTimeFormatInfo.InvariantInfo.Calendar;
            var testDate = DateTime.Parse(date);
            var result = cal.GetWeekOfYear(testDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            //Assert.That(dfi.FirstDayOfWeek, Is.EqualTo(DayOfWeek.Monday));
            Assert.That(result, Is.EqualTo(expectedWeekOfTheYear));
        }

    }
}