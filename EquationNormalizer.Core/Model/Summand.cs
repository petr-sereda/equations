using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace EquationNormalizer.Core.Model
{
    /// <summary>
    /// Слагаемое полинома. Например, 4x^2yz. Может быть константой.
    /// </summary>
    /// <remarks>
    /// Immutable, hence thread-safe.
    /// </remarks>
    public class Summand : IEquatable<Summand>
    {
        private readonly Variable[] _variables;

        /// <summary>
        /// Коэффициент перед слагаемым
        /// </summary>
        public double Coefficient { get; }

        /// <summary>
        /// Список переменных данного слагаемого с их показателями степени.
        /// </summary>
        public IReadOnlyCollection<Variable> Variables => _variables;

        public bool IsConstant => _variables.Length == 0;

        /// <summary>
        /// Создает слагаемое полинома представляющее переменную вовзеденную в указанную степень и с указанным коеффициентом.
        /// </summary>
        public Summand(double coefficient, params Variable[] variables)
        {
            if (variables == null)
                throw new ArgumentNullException("variables");

            Coefficient = coefficient;
            _variables = variables.OrderBy(v => v.Name).ToArray();
        }

        /// <summary>
        /// Конструктор для создания константного выражения.
        /// </summary>
        public Summand(double constant) : this(constant, new Variable[0])
        {
        }

        /// <summary>
        /// Скалдывает два слагаемых проверяя, можно ли их сложить.
        /// </summary>
        public Summand Add(Summand other)
        {
            // проверяем, что оба слагаемых имеют одинаковые наборы переменных
            if (!AreVariablesSameWith(other))
                throw new ArgumentException("Нельзя складывать слагаемые с разными наборами переменных");
            
            return new Summand(this.Coefficient + other.Coefficient, _variables);
        }

        /// <summary>
        /// Домножает слагаемое на указанное число.
        /// </summary>
        public Summand Multiply(double d)
        {
            return new Summand(Coefficient * d, _variables);
        }

        public bool AreVariablesSameWith(Summand other)
        {
            if (this._variables.Length != other._variables.Length)
                return false;
            if (this._variables.Zip(other._variables, (v1, v2) => new {v1, v2}).Any(pair => !pair.v1.Equals(pair.v2)))
                return false;

            return true;
        }

        /// <summary>
        /// Нормализует слагаемое многочлена - объединяет одинаковые переменные и упорядочивает переменные по степени и имени.
        /// Например, y^3xzx^2y будет приведено к y^4x^3z.
        /// </summary>
        public Summand Normalize()
        {
            var normalizedVars = _variables
                .GroupBy(v => v.Name, v => v.Power)
                .Select(grouped => new Variable(grouped.Key, grouped.Aggregate(0, (a, b) => a + b)))
                .OrderByDescending(v => v.Power)
                .ThenBy(v => v.Name)
                .ToArray();

            return new Summand(this.Coefficient, normalizedVars);
        }

        #region Equality and HasCode

        public bool Equals(Summand other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!AreVariablesSameWith(other))
                return false;

            return this.Coefficient == other.Coefficient;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Summand) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return _variables.Aggregate((int)Math.Floor(Coefficient), (a, b) => a ^ b.GetHashCode());
            }
        }

        public override string ToString()
        {
            // возможность интерполирования строк из будущего C# 6.0 тут была бы очень кстати
            return 
                (Coefficient > 0 ? "+ " : "- ") + 
                Math.Abs(Coefficient).ToString(CultureInfo.InvariantCulture) + 
                String.Concat(_variables.Select(v => v.ToString()));
        }

        #endregion
    }
}
