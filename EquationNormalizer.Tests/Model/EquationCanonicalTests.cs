using EquationNormalizer.Core.Model;
using EquationNormalizer.Core.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EquationNormalizer.Tests.Model
{
    [TestClass]
    public class EquationCanonicalTests
    {
        readonly EquationParser _parser = 
            new EquationParser(
                new PolynomialParser(
                    new SummandParser(
                        new VariablesParser())));

        [TestMethod]
        public void DifferentVariables()
        {
            var eq = _parser.Parse("x = y");

            var result = eq.ToCanonical();

            Assert.AreEqual(2, result.Left.SummandsCount);
            Assert.AreEqual(1, result.Right.SummandsCount);
            Assert.AreEqual(new Summand(0), result.Right[0]);
            Assert.AreEqual(new Summand(1, new Variable("x")), result.Left[0]);
            Assert.AreEqual(new Summand(-1, new Variable("y")), result.Left[1]);
        }

        [TestMethod]
        public void Constants()
        {
            var eq = _parser.Parse("14 + 9 = 7");

            var result = eq.ToCanonical();

            Assert.AreEqual(1, result.Left.SummandsCount);
            Assert.AreEqual(1, result.Right.SummandsCount);
            Assert.AreEqual(new Summand(0), result.Right[0]);
            Assert.AreEqual(new Summand(16), result.Left[0]);            
        }

        [TestMethod]
        public void ZeroAtRightPart()
        {
            var eq = _parser.Parse("x^2 - y = 0");

            var result = eq.ToCanonical();

            Assert.AreEqual(eq, result);
        }

        [TestMethod]
        public void ComplexCase()
        {
            var eq = _parser.Parse("-y + 17x^2 - y + xyz^2 = z^3 + xyz - x^2 + 8 - xyz^2");
            var canEq = _parser.Parse("-z^3 + 18x^2 + 2xyz^2 - 8 - xyz - 2y = 0");
            
            var result = eq.ToCanonical();

            Assert.AreEqual(canEq, result);
        }
    }
}
