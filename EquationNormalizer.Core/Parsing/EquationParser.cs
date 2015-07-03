using System;
using EquationNormalizer.Core.Model;

namespace EquationNormalizer.Core.Parsing
{
    public class EquationParser
    {
        private readonly PolynomialParser _polynomialParser;

        public EquationParser(PolynomialParser polynomialParser)
        {
            _polynomialParser = polynomialParser;
        }

        public Equation Parse(string equationString)
        {
            equationString = SanitizeInput(equationString);

            var parts = equationString.Split('=');

            if (parts.Length != 2)
                throw new ParsingException("В уравнении должен быть в точности один знак равенства.");

            var left = _polynomialParser.Parse(parts[0]);
            var right = _polynomialParser.Parse(parts[1]);

            return new Equation(left, right);
        }

        private static string SanitizeInput(string expressionString)
        {
            // т.к. сложение - симметричная операция, скобки в уравнении не окажут никакой роли и их можно удалить
            return expressionString.Replace('(', ' ').Replace(')', ' ').Replace(" ", String.Empty);
        }
    }
}