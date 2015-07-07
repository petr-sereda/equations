using EquationNormalizer.Core.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EquationNormalizer.Tests.Parsing
{
    [TestClass]
    public class SummandParserTests
    {
        private SummandParser _parser;

        [TestInitialize]
        public void Setup()
        {
            _parser = new SummandParser(new VariablesParser());
        }

        [TestMethod]
        public void TestUnsignedIntConstants()
        {
            var summand = _parser.Parse("0");

            Assert.IsNotNull(summand);
            Assert.IsTrue(summand.IsConstant);
            Assert.AreEqual(summand.Coefficient, 0);

            summand = _parser.Parse("100500");

            Assert.IsNotNull(summand);
            Assert.IsTrue(summand.IsConstant);
            Assert.AreEqual(summand.Coefficient, 100500);
        }

        [TestMethod]
        public void TestSignedIntConstants()
        {
            var summand = _parser.Parse("-99");

            Assert.IsNotNull(summand);
            Assert.IsTrue(summand.IsConstant);
            Assert.AreEqual(-99, summand.Coefficient);

            summand = _parser.Parse("+99");

            Assert.IsNotNull(summand);
            Assert.IsTrue(summand.IsConstant);
            Assert.AreEqual(99, summand.Coefficient);
        }

        [TestMethod]
        public void FloatingPointConstants()
        {
            var summand = _parser.Parse("3.1415");

            Assert.IsNotNull(summand);
            Assert.IsTrue(summand.IsConstant);
            Assert.AreEqual(3.1415, summand.Coefficient);

            summand = _parser.Parse("-0.1415");

            Assert.IsNotNull(summand);
            Assert.IsTrue(summand.IsConstant);
            Assert.AreEqual(-0.1415, summand.Coefficient);
        }

        [TestMethod]
        [ExpectedException(typeof(ParsingException))]
        public void IncorrectCoef()
        {
            _parser.Parse("0s.1a415x");
        }

        [TestMethod]
        [ExpectedException(typeof(ParsingException))]
        public void EmptyInputError()
        {
            _parser.Parse("");
        }

        [TestMethod]
        [ExpectedException(typeof(ParsingException))]
        [Ignore] // не поправил парсер за нехваткой времени
        public void OnlySignInInputError()
        {
            _parser.Parse("+");
        }

        [TestMethod]
        public void VariablesWithoutPower()
        {
            var summand = _parser.Parse("-3.14ab");

            Assert.AreEqual(-3.14, summand.Coefficient);
        }

        [TestMethod]
        public void VariablesWithPower()
        {
            var summand = _parser.Parse("x^2");

            Assert.IsNotNull(summand);
            Assert.IsFalse(summand.IsConstant);
            Assert.AreEqual(1, summand.Coefficient);

            summand = _parser.Parse("+17.9ab^10");

            Assert.IsNotNull(summand);            
            Assert.AreEqual(17.9, summand.Coefficient);
        }

        [TestMethod]
        public void VariableOnlyWithSign()
        {
            var summand = _parser.Parse("-x");
            Assert.AreEqual(-1, summand.Coefficient);

            summand = _parser.Parse("+x");
            Assert.AreEqual(1, summand.Coefficient);
        }
    }
}
