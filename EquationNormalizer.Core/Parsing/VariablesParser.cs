using System;
using System.Linq;
using System.Text.RegularExpressions;
using EquationNormalizer.Core.Model;

namespace EquationNormalizer.Core.Parsing
{
    public class VariablesParser
    {
        private const string Pattern = @"([a-z]{1}(\^\d+)?)";

        public Variable[] Parse(string input)
        {
            if (String.IsNullOrEmpty(input))
                return new Variable[0];

            var match = Regex.Match(input, Pattern);

            return match.Groups.Cast<Group>().Select(gr => ParseVariable(gr.Captures[0].Value)).ToArray();
        }

        private Variable ParseVariable(string varString)
        {
            var parts = varString.Split('^');

            return new Variable(
                ParseVariableName(parts[0]), 
                ParsePower(parts.Length > 1 ? parts[1] : String.Empty));
        }

        private string ParseVariableName(string name)
        {
            if (!Variable.IsValidName(name))
                throw new ParsingException("»м€ переменной должно состо€ть из одной буквы.");

            return name;
        }

        private int ParsePower(string powerStr)
        {
            int power;

            if (!int.TryParse(powerStr, out power))
                throw new ParsingException("ѕоказатель степени должен быть целым числом большим 0.");

            return power;
        }
    }
}