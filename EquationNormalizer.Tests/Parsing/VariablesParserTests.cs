using System.Linq;
using EquationNormalizer.Core.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EquationNormalizer.Tests.Parsing
{
    [TestClass]
    public class VariablesParserTests
    {
        private VariablesParser _parser;

        [TestInitialize]
        public void Init()
        {
            _parser = new VariablesParser();
        }

        [TestMethod]
        [ExpectedException(typeof(ParsingException))]
        public void IncorrectPower()
        {
            _parser.Parse("xyx^");
        }

        [TestMethod]
        [ExpectedException(typeof(ParsingException))]
        public void NonLatinCharsInName()
        {
            _parser.Parse("dёa");
        }

        [TestMethod]
        public void SingleVariableWithoutPower()
        {
            var variables = _parser.Parse("x");

            Assert.IsNotNull(variables);
            Assert.AreEqual(1, variables.Length);
            Assert.AreEqual(1, variables.First().Power);            
        }

        [TestMethod]
        public void MultipleVariablesWithoutPower()
        {
            var variables = _parser.Parse("xyz");

            Assert.AreEqual(3, variables.Count());
            Assert.IsFalse(variables.Any(v => v.Power != 1));
            Assert.AreEqual("x", variables.ElementAt(0).Name);
            Assert.AreEqual("y", variables.ElementAt(1).Name);
            Assert.AreEqual("z", variables.ElementAt(2).Name);
        }

        [TestMethod]
        public void SingleVariableWithPower()
        {
            var variables = _parser.Parse("x^8");

            Assert.IsNotNull(variables);
            Assert.AreEqual(1, variables.Length);
            Assert.AreEqual(8, variables.First().Power);
        }

        [TestMethod]
        public void MultipleVariablesWithPower()
        {
            var variables = _parser.Parse("z^2yx^8");

            Assert.AreEqual(3, variables.Count());            
            Assert.AreEqual("z", variables.ElementAt(0).Name);
            Assert.AreEqual(2, variables.ElementAt(0).Power);
            Assert.AreEqual("y", variables.ElementAt(1).Name);
            Assert.AreEqual(1, variables.ElementAt(1).Power);
            Assert.AreEqual("x", variables.ElementAt(2).Name);
            Assert.AreEqual(8, variables.ElementAt(2).Power);
        }

        [TestMethod]
        public void PowerThen10()
        {
            var variables = _parser.Parse("x^876");

            Assert.IsNotNull(variables);
            Assert.AreEqual(1, variables.Length);
            Assert.AreEqual("x", variables.ElementAt(0).Name);
            Assert.AreEqual(876, variables.First().Power);
        }
    }
}
