using System;
using AspNetSelfHostDemo;
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
            try
            {
                var result = _sut.GetTax(It.IsAny<string>(), "2016.xx.01");
            }
            catch (ArgumentException e)
            {
                Assert.That(e.Message, Does.Contain("Invalid date"));
                return;
            }

            Assert.Fail("expected to catch argument exception");
        }
    }
}
