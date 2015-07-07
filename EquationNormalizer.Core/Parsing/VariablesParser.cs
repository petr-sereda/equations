using System;
using System.Collections.Generic;
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

            var matches = Regex.Matches(input, Pattern);

            var result = new List<Variable>();

            int totalMatchesLength = 0;
            
            foreach (Match match in matches)
            {
                var variable = ParseVariable(match.Groups[0].Value);
                result.Add(variable);

                totalMatchesLength += match.Groups[0].Value.Length;
            }

            if (totalMatchesLength != input.Length)
                throw new ParsingException(String.Format("Некорретный формат записи переменных: {0}", input));

            return result.ToArray();
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
                throw new ParsingException("Имя переменной должно состоять из одной буквы.");

            return name;
        }

        private int ParsePower(string powerStr)
        {
            if (String.IsNullOrEmpty(powerStr))
                return 1;

            int power;

            if (!int.TryParse(powerStr, out power))
                throw new ParsingException("Показатель степени должен быть целым числом большим 0.");

            return power;
        }
    }
}