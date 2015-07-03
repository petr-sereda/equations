using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace EquationNormalizer.Core.Model
{
    /// <summary>
    /// Слагаемое полинома. Например, 4*x^2*y*z. Может быть константой.
    /// </summary>
    /// <remarks>
    /// Immutable, hence thread-safe.
    /// </remarks>
    public class Summand : IEquatable<Summand>
    {
        /// <summary>
        /// Коэффициент перед слагаемым
        /// </summary>
        public double Coefficient { get; private set; }

        /// <summary>
        /// Список переменных данного слагаемого с их показателями степени.
        /// </summary>
        public IReadOnlyCollection<Variable> Variables { get { return _variables;} }

        private readonly Variable[] _variables;

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

        public bool IsConstant
        {
            get { return _variables.Length == 0; }
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
            if (this._variables.Zip(other._variables, (v1, v2) => new {v1, v2}).Any(pair => pair.v1 != pair.v2))
                return false;

            return true;
        }

        public Summand Normalize()
        {
            throw new NotImplementedException();
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
                var hashCode = (_variables != null ? _variables.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Coefficient.GetHashCode();
                hashCode = (hashCode*397) ^ (Variables != null ? Variables.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            // возможность интерполирования строк из будущего C# 6.0 тут была бы очень кстати :)
            return 
                (Coefficient > 0 ? "+ " : "- ") + 
                Math.Abs(Coefficient).ToString(CultureInfo.InvariantCulture) + 
                String.Concat(_variables.Select(v => v.ToString()));
        }

        #endregion
    }
}
