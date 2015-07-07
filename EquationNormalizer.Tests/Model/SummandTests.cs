using EquationNormalizer.Core.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EquationNormalizer.Tests.Model
{
    [TestClass]
    public class SummandTests
    {
        // в стороковом виде гораздо нагляднее создавать инстансы класса для тестирования, пока я не сделаю fluent interface для билдера уравнений и слагаемых
        private readonly SummandParser _parser = new SummandParser(new VariablesParser());

        [TestMethod]
        public void TestEquality()
        {
            var first = _parser.Parse("a^2d^4ee");
            var second = _parser.Parse("a^2d^4ee");

            Assert.AreEqual(first, second);
        }

        [TestMethod]
        public void TestUnequality()
        {
            var first = _parser.Parse("a^2d^4ee");
            var second = _parser.Parse("wa^2d^4");

            Assert.AreNotEqual(first, second);
        }

        [TestMethod]
        public void TestAreVariablesSameWith()
        {
            var first = _parser.Parse("ae^2e");
            var second = _parser.Parse("ae^2e");

            Assert.IsTrue(first.AreVariablesSameWith(second));
        }

        [TestMethod]
        public void TestAreVariablesNotSameWith()
        {
            var first = _parser.Parse("ae^2e");
            var second = _parser.Parse("xe^2e");

            Assert.IsFalse(first.AreVariablesSameWith(second));
        }

        [TestMethod]
        public void TestAreVariablesNotSameWith2()
        {
            var first = _parser.Parse("ae^2e");
            var second = _parser.Parse("e^2e");

            Assert.IsFalse(first.AreVariablesSameWith(second));
        }

        [TestMethod]
        public void AlreadyNormalizedSingleVariable()
        {
            var underTest = _parser.Parse("2x");
            var mustBe = _parser.Parse("2x");

            underTest = underTest.Normalize();

            Assert.AreEqual(underTest, mustBe);
        }

        [TestMethod]
        public void AlreadyNormalizedMultipleVariables()
        {
            var underTest = _parser.Parse("2x^2y^2z^2");
            var mustBe = _parser.Parse("2x^2y^2z^2");

            underTest = underTest.Normalize();

            Assert.AreEqual(underTest, mustBe);
        }

        [TestMethod]
        public void NormalizeSingleVariable()
        {
            var underTest = _parser.Parse("2x^3xxx^2");
            var mustBe = _parser.Parse("2x^7");

            underTest = underTest.Normalize();

            Assert.AreEqual(underTest, mustBe);
        }

        [TestMethod]
        public void NormalizeMultipleVariables()
        {
            var underTest = _parser.Parse("2z^2x^3xzy^10");
            var mustBe = _parser.Parse("2y^10x^4z^3");

            underTest = underTest.Normalize();

            Assert.AreEqual(underTest, mustBe);
        }

        [TestMethod]
        public void NormalizePutsVariablesInCorrectOrder()
        {
            var underTest = _parser.Parse("z^2x^2y^4");
            var mustBe = _parser.Parse("y^4x^2z^2");

            underTest = underTest.Normalize();

            Assert.AreEqual(underTest, mustBe);
        }
    }
}
