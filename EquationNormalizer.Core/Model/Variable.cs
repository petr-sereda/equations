using System;

namespace EquationNormalizer.Core.Model
{
    /// <summary>
    /// Представляет переменную входящую в состав слагаемого многочлена и целочисленный показатель ее степени.
    /// </summary>
    /// <remarks>
    /// Immutable, hence thread-safe.
    /// </remarks>
    public class Variable : IEquatable<Variable>
    {
        /// <summary>
        /// Однобуквенное имя переменной.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Целочисленный показатель степени.
        /// </summary>
        public int Power { get; }

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

        public bool Equals(Variable other)
        {
            return string.Equals(Name, other.Name) && Power == other.Power;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Variable) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0)*397) ^ Power;
            }
        }
    }
}
