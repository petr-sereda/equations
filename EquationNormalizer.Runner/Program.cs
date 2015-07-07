using System;
using EquationNormalizer.Core.Parsing;

namespace EquationNormalizer.Runner
{
    /// <summary>
    /// Later I'll implement a file and interactive modes as explained in Readme.txt.
    /// For now Program.cs contains only a sample code of how to use classes for equation normalization.
    /// </summary>
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
