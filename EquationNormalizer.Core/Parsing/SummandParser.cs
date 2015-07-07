using System;
using System.Globalization;
using System.Text.RegularExpressions;
using EquationNormalizer.Core.Model;

namespace EquationNormalizer.Core.Parsing
{
    /// <summary>
    /// Парсер слагаемых многочлена "в лоб".
    /// На более продвинутую реализацию (например, через конечные автоматы) не хватило времени отведенного на выполнение задания.
    /// </summary>
    public class SummandParser
    {
        private static Group _varsGroup;
        private static Group _coefGroup;
        
        private const string Pattern = @"^(?<coef>[^a-z]*)(?<vars>[a-z\^0-9]*)$";

        private readonly VariablesParser _variablesParser;

        public SummandParser(VariablesParser variablesParser)
        {
            _variablesParser = variablesParser;
        }

        public Summand Parse(string summandString)
        {
            // Arggh, I NEVER write a parser on Regexps again

            var match = Regex.Match(summandString, Pattern);

            _varsGroup = match.Groups["vars"];
            _coefGroup = match.Groups["coef"];            

            if (_varsGroup.Value == String.Empty && _coefGroup.Value == String.Empty)
                throw new ParsingException("Некорректный формат слагаемого.");

            var coef = ParseCoef();
            var variables = ParseVariables();
            
            return variables == null ? new Summand(coef) : new Summand(coef, variables);
        }

        private static double ParseCoef()
        {            
            // перед переменной не указан числовой коэффициент - значит это +1            
            if (_coefGroup.Value == String.Empty || _coefGroup.Value == "+")
                return 1;
            if (_coefGroup.Value == "-")
                return -1;

            double coef;
            if (!double.TryParse(_coefGroup.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out coef))
                throw new ParsingException("Числовой коэффициент слагаемого в некорретном формате.");

            return coef;
        }

        private Variable[] ParseVariables()
        {
            if (!_varsGroup.Success || _varsGroup.Value == String.Empty)
                return new Variable[0];

            return _variablesParser.Parse(_varsGroup.Value);
        }
    }
}
