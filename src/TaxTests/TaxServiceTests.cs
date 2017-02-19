using System;
using AspNetSelfHostDemo;
using AspNetSelfHostDemo.Entities;
using Moq;
using NUnit.Framework;

namespace TaxTests
{
    [TestFixture]
    public class TaxServiceTests
    {
        private TaxService _sut;
        private Mock<ITaxRepository> _taxRepository;

        [SetUp]
        public void SetUp()
        {
            _taxRepository = new Mock<ITaxRepository>();
            _taxRepository.Setup(m => m.GetTax(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(1);
            _sut = new TaxService(_taxRepository.Object);
        }

        [Test]
        public void ReturnsTaxWithCorrectDateInput()
        {
            var result = _sut.GetTax(It.IsAny<string>(), "2016.01.01");
            Assert.AreEqual(1, result);
        }

        [Test]
        public void ThrowsWithIncorrectDateInput()
        {
            Assert.That(() => _sut.GetTax(It.IsAny<string>(), "2016.xx.01"),
                Throws.TypeOf<ArgumentException>().With.Message.Contain("Invalid date provided"));
        }

        [Test]
        public void ThrowsWhenIncorrectTaxRecord_Null()
        {
            Assert.That(() => _sut.UpdateTaxes(null),
                Throws.TypeOf<ArgumentException>().With.Message.Contain("Input data is invalid"));
        }

        [TestCase(null)]
        [TestCase("")]
        public void ThrowsWhenIncorrectTaxRecord_InvalidCity(string city)
        {
            var taxRecord = new TaxRecord
            {
                Year = 100,
                City = city
            };

            Assert.That(() => _sut.UpdateTaxes(taxRecord),
                Throws.TypeOf<ArgumentException>().With.Message.Contain("City property"));
        }

        [TestCase(-1)]
        [TestCase(null)]
        public void ThrowsWhenIncorrectTaxRecord_InvalidYear(int? year)
        {
            var taxRecord = new TaxRecord
            {
                City = "a",
                Year = year
            };

            Assert.That(() => _sut.UpdateTaxes(taxRecord),
                Throws.TypeOf<ArgumentException>().With.Message.Contain("Year property"));
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(13)]
        public void ThrowsWhenIncorrectTaxRecord_InvalidMonth(int? month)
        {
            var taxRecord = new TaxRecord
            {
                City = "a",
                Year = 2222,
                Month = month
            };

            Assert.That(() => _sut.UpdateTaxes(taxRecord),
                Throws.TypeOf<ArgumentException>().With.Message.Contain("Month property"));
        }

        [TestCase("xxx")]
        [TestCase("2016.13.01")]
        public void ThrowsWhenIncorrectTaxRecord_InvalidDay(string day)
        {
            var taxRecord = new TaxRecord
            {
                City = "a",
                Year = 2222,
                Day = day
            };

            Assert.That(() => _sut.UpdateTaxes(taxRecord),
                Throws.TypeOf<ArgumentException>().With.Message.Contain("Day property"));
        }

        [TestCase(1, null, null)]
        [TestCase(null, 1, null)]
        [TestCase(null, null, 1)]
        [TestCase(1, 1, null)]
        [TestCase(1, null, 1)]
        [TestCase(null, 1, 1)]
        public void ThrowsWhenIncorrectTaxRecord_DayAndOtherTypeIsPresent(int? year, int? month, int? weekNr)
        {
            var taxRecord = new TaxRecord
            {
                City = "a",
                Year = year,
                Month = month,
                WeekOfYear = weekNr,
                Day = "2016.10.10"
            };

            Assert.That(() => _sut.UpdateTaxes(taxRecord),
                Throws.TypeOf<ArgumentException>().With.Message.Contain("one type of tax"));
        }

        [Test]
        public void UpdatesTaxesByDay()
        {
            var taxRecord = new TaxRecord
            {
                City = "a",
                Day = "2016.02.02"
            };
            _sut.UpdateTaxes(taxRecord);
        }
    }
}
