using EquationNormalizer.Core.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EquationNormalizer.Tests
{
    [TestClass]
    public class EquationParserTests
    {
        private EquationParser _parser;

        [TestInitialize]
        public void Setup()
        {
            _parser =
                new EquationParser(
                    new PolynomialParser(
                        new SummandParser(
                            new VariablesParser())));
        }

        [TestMethod]
        public void TwoConstants()
        {
            var equation = _parser.Parse("5 = 5");

            Assert.IsNotNull(equation);
            Assert.IsTrue(equation.Left[0].IsConstant);
            Assert.IsTrue(equation.Right[0].IsConstant);
        }

        [TestMethod]
        public void TwoVariables()
        {
            var equation = _parser.Parse("x = y");

            Assert.IsNotNull(equation);
            Assert.IsTrue(!equation.Left[0].IsConstant);
            Assert.IsTrue(!equation.Right[0].IsConstant);
        }

        [TestMethod]
        public void ComplexExpressions()
        {
            var equation = _parser.Parse("4x - z^4 + 6 = y^2 + x^2");

            Assert.IsNotNull(equation);
            Assert.AreEqual(3, equation.Left.SummandsCount);
            Assert.AreEqual(2, equation.Right.SummandsCount);
        }

        [TestMethod]
        public void ParanthesisIgnored()
        {
            var equation1 = _parser.Parse("4x - z^4 + 6 = y^2 + x^2");
            var equation2 = _parser.Parse("((4x - z^4) + 6) = (y^2 + x^2)");

            Assert.AreEqual(equation1, equation2);
        }

        [TestMethod]
        [ExpectedException(typeof(ParsingException))]
        public void NoEqualityOperator()
        {
            _parser.Parse("4x - z^4 + 6");
        }

        [TestMethod]
        [ExpectedException(typeof(ParsingException))]
        public void TwoEqualityOperators()
        {
            _parser.Parse("4x - z^4 + 6 = x = u^2");
        }

        [TestMethod]
        [ExpectedException(typeof(ParsingException))]
        public void MessInput()
        {
            _parser.Parse("dskjc nwd = 78c7&*Yhi usc");
        }
    }
}
