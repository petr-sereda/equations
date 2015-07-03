using System;

namespace EquationNormalizer.Core.Model
{
    /// <summary>
    /// Представляет переменную входящую в состав слагаемого многочлена и целочисленный показатель ее степени.
    /// </summary>
    public class Variable
    {
        /// <summary>
        /// Однобуквенное имя переменной.
        /// </summary>
        public string Name { get; private set; }

        public int Power { get; private set; }

        public Variable(string name, int power = 1)
        {
            if (!IsValidName(name))
                throw new ArgumentException("Имя пременной должно состоять из одной буквы", "name");
            if (power < 1)
                throw new ArgumentException("Степень переменной должна быть целым числом больим нуля.");

            Name = name;
            Power = power;
        }

        public static bool IsValidName(string name)
        {
            if (String.IsNullOrEmpty(name))
                return false;
            if (name.Length > 1)
                return false;
            if (!Char.IsLetter(name[0]))
                return false;

            return true;
        }
    }
}
