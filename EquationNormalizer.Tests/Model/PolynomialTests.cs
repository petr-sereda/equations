using EquationNormalizer.Core.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EquationNormalizer.Tests.Model
{
    [TestClass]
    public class PolynomialTests
    {
        private readonly PolynomialParser _parser = new PolynomialParser(new SummandParser(new VariablesParser()));

        [TestMethod]
        public void AlreadyCanonical()
        {
            var p = _parser.Parse("x^2+y+8");

            var canonical = p.ToCanonical();

            Assert.AreEqual(p, canonical);
        }

        [TestMethod]
        public void CanonicalConstants()
        {
            var p = _parser.Parse("8-19");

            var canonical = p.ToCanonical();

            Assert.AreNotEqual(p, canonical);
            Assert.AreEqual(1, canonical.SummandsCount);
            Assert.AreEqual(-11, canonical[0].Coefficient);
        }

        [TestMethod]
        public void SeveralMembersWithSameVariable()
        {
            var p = _parser.Parse("8xy^2-19xy^2+6xy^2");

            var canonical = p.ToCanonical();

            var expected = _parser.Parse("-5xy^2");

            Assert.AreEqual(expected, canonical);
            Assert.AreEqual(1, canonical.SummandsCount);
            Assert.AreEqual(-5, canonical[0].Coefficient);
        }

        [TestMethod]
        public void SameVariableWithDifferentPowers()
        {
            var p = _parser.Parse("8xy^3-19xy^2+6xy");

            var canonical = p.ToCanonical();

            Assert.AreEqual(p, canonical);
            Assert.AreEqual(3, canonical.SummandsCount);
        }

        [TestMethod]
        public void SeveralMemebrsWithSeveralVariables()
        {
            var p = _parser.Parse("8x^2-19z+6x^2+7-z-z-z+x^2");

            var canonical = p.ToCanonical();

            var expected = _parser.Parse("15x^2-22z+7");

            Assert.AreEqual(expected, canonical);
        }
    }
}
