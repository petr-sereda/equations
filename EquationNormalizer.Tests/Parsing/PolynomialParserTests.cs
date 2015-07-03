using EquationNormalizer.Core.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EquationNormalizer.Tests.Parsing
{
    [TestClass]
    public class PolynomialParserTests
    {
        private PolynomialParser _parser;

        [TestInitialize]
        public void Setup()
        {
            _parser = 
                new PolynomialParser(
                    new SummandParser(
                        new VariablesParser()));
        }

        [TestMethod]
        public void TestSingleConstant()
        {
            var polynomial = _parser.Parse("0");

            Assert.AreEqual(1, polynomial.SummandsCount);
            Assert.IsTrue(polynomial[0].IsConstant);
        }

        [TestMethod]
        public void TestSingleMember()
        {
            var polynomial = _parser.Parse("7x^2");

            Assert.AreEqual(1, polynomial.SummandsCount);
            
            var summand = polynomial[0];
            Assert.IsFalse(summand.IsConstant);
            Assert.AreEqual(7, summand.Coefficient);
        }

        [TestMethod]
        public void TestMembersPlusConstant()
        {
            var polynomial = _parser.Parse("7x^2 +8");

            Assert.AreEqual(2, polynomial.SummandsCount);
            
            var first = polynomial[0];
            Assert.IsFalse(first.IsConstant);
            Assert.AreEqual(7, first.Coefficient);

            Assert.IsTrue(polynomial[1].IsConstant);
            Assert.AreEqual(8, polynomial[1].Coefficient);
        }

        [TestMethod]
        public void TestSeveralMembers()
        {
            var polynomial = _parser.Parse("7x^2 +8y -z^3");

            Assert.AreEqual(3, polynomial.SummandsCount);

            var first = polynomial[0];
            Assert.IsFalse(first.IsConstant);
            Assert.AreEqual(7, first.Coefficient);
            
            var second = polynomial[1];
            Assert.IsFalse(second.IsConstant);

            var third = polynomial[2];
            Assert.AreEqual(-1, third.Coefficient);
        }

        [TestMethod]
        [ExpectedException(typeof(ParsingException))]
        public void EmptyInputRaisesError()
        {
            _parser.Parse("");
        }

        [TestMethod]
        [ExpectedException(typeof(ParsingException))]
        public void IncorrectOperationError()
        {
            _parser.Parse("67df * 9c");
        }

        [TestMethod]
        [ExpectedException(typeof(ParsingException))]
        public void MissingMemberError()
        {
            _parser.Parse("67df + - 9c");
        }
    }
}
