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
            _taxRepository.Setup(m => m.GetAllTaxes()).Returns(1);
             _sut = new TaxService(_taxRepository.Object);
        }

        [Test]
        public void ReturnsCorrectTax()
        {
            var result = _sut.GetTax(It.IsAny<string>(), It.IsAny<DateTime>());
            Assert.AreEqual(1, result);
        }
    }
}
