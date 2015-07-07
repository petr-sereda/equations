using System;
using EquationNormalizer.Core.Parsing;

namespace EquationNormalizer.Runner
{
    static class Program
    {
        static void Main(string[] args)
        {                       
            var parser = 
                new EquationParser(
                    new PolynomialParser(
                        new SummandParser(
                            new VariablesParser())));

            // исходное уравнение
            var equation = parser.Parse("-y + 17x^2 - y + xyz^2 = z^3 + xyz - x^2 + 8 - xyz^2");

            // преобразуем уравнение к каноническому виду
            var canonical = equation.ToCanonical();

            // печатаем его на экране
            Console.WriteLine(canonical);

            Console.ReadLine();
        }
    }
}
