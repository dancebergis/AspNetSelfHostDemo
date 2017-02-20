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

        [TestCase(null)]
        [TestCase(0)]
        public void ThrowsWhenIncorrectTaxRecord_InvalidTax(decimal? tax)
        {
            var taxRecord = new TaxRecord
            {
                City = "a",
                Year = 2016
            };

            Assert.That(() => _sut.UpdateTaxes(taxRecord),
                Throws.TypeOf<ArgumentException>().With.Message.Contain("Tax must have a non-zero value"));
        }

        [TestCase(-1)]
        [TestCase(null)]
        public void ThrowsWhenIncorrectTaxRecord_InvalidYear(int? year)
        {
            var taxRecord = new TaxRecord
            {
                City = "a",
                Tax = 0.1m,
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
                Tax = 0.1m,
                Year = 2222,
                Month = month
            };

            Assert.That(() => _sut.UpdateTaxes(taxRecord),
                Throws.TypeOf<ArgumentException>().With.Message.Contain("Month property"));
        }

        [TestCase(2016, 0)]
        [TestCase(2016, -1)]
        [TestCase(2016, 54)]
        public void ThrowsWhenIncorrectTaxRecord_InvalidWeek(int year, int? week)
        {
            var taxRecord = new TaxRecord
            {
                City = "a",
                Tax = 0.1m,
                Year = year,
                WeekOfYear = week
            };

            Assert.That(() => _sut.UpdateTaxes(taxRecord),
                Throws.TypeOf<ArgumentException>().With.Message.Contain("Week property"));
        }

        [TestCase("xxx")]
        [TestCase("2016.13.01")]
        public void ThrowsWhenIncorrectTaxRecord_InvalidDay(string day)
        {
            var taxRecord = new TaxRecord
            {
                City = "a",
                Tax = 0.1m,
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
                Tax = 0.1m,
                Year = year,
                Month = month,
                WeekOfYear = weekNr,
                Day = "2016.10.10"
            };

            Assert.That(() => _sut.UpdateTaxes(taxRecord),
                Throws.TypeOf<ArgumentException>().With.Message.Contain("one type of tax"));
        }

        [Test]
        public void ThrowsWhenIncorrectTaxRecord_MonthAndWeekTypeIsPresent()
        {
            var taxRecord = new TaxRecord
            {
                City = "a",
                Tax = 0.1m,
                Year = 2222,
                Month = 1,
                WeekOfYear = 1
            };

            Assert.That(() => _sut.UpdateTaxes(taxRecord),
                Throws.TypeOf<ArgumentException>().With.Message.Contain("one type of tax"));
        }

        [Test]
        public void UpdatesTaxesByDay()
        {
            var taxRecord = new TaxRecord { City = "a", Tax = 0.1m, Day = "2016.02.02" };
            _sut.UpdateTaxes(taxRecord);
            _taxRepository.Verify(m => m.AddDailyTax("a", DateTime.Parse("2016.02.02"), 0.1m), Times.Once);
        }

        [Test]
        public void UpdatesTaxesByYear()
        {
            var taxRecord = new TaxRecord { City = "a", Tax = 0.1m, Year = 2016 };
            _sut.UpdateTaxes(taxRecord);
            _taxRepository.Verify(m => m.AddYearlyTax("a", 2016, 0.1m), Times.Once);
            // TODO check that "AddMonthly/Weekly/DaylyTask" was called 0 times
        }

        [Test]
        public void UpdatesTaxesByMonth()
        {
            var taxRecord = new TaxRecord { City = "a", Tax = 0.1m, Year = 2016, Month = 2};
            _sut.UpdateTaxes(taxRecord);
            _taxRepository.Verify(m => m.AddMonthlyTax("a", 2016, 2, 0.1m), Times.Once);
        }

        [Test]
        public void UpdatesTaxesByWeek()
        {
            var taxRecord = new TaxRecord { City = "a", Tax = 0.1m, Year = 2016, WeekOfYear = 3 };
            _sut.UpdateTaxes(taxRecord);
            _taxRepository.Verify(m => m.AddWeeklyTax("a", 2016, 3, 0.1m), Times.Once);
        }
    }
}
