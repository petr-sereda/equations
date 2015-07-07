using System;
using System.Collections.Generic;
using System.Linq;
using EquationNormalizer.Core.Model;

namespace EquationNormalizer.Core.Parsing
{
    /// <summary>
    /// Простейшая реализация парсера многочленов "в лоб".
    /// </summary>
    public class PolynomialParser
    {
        private readonly SummandParser _summondParser;

        public PolynomialParser(SummandParser summondParser)
        {
            _summondParser = summondParser;
        }

        public Polynomial Parse(string polynomialString)
        {
            var summandSubstrs = BreakdownToSubstrs(polynomialString).ToArray();

            var summands = summandSubstrs.Select(substr => _summondParser.Parse(substr.Trim())).ToArray();

            return new Polynomial(summands);
        }

        private IEnumerable<string> BreakdownToSubstrs(string polynomialString)
        {
            string substr = String.Empty;

            foreach (var c in polynomialString)
            {
                if ((c == '+' || c == '-') && substr != String.Empty)
                {
                    yield return substr;
                    substr = "" + c;
                }
                else
                {
                    substr += c;
                }
            }
            
            yield return substr;
        }
    }
}