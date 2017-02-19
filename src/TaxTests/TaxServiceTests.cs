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

        [SetUp]
        public void SetUp()
        {
            _sut = new TaxService();
        }

        [Test]
        public void ReturnsCorrectTax()
        {
            var result = _sut.GetTax(It.IsAny<string>(), It.IsAny<DateTime>());
            Assert.AreEqual(1, result);
        }
    }
}
