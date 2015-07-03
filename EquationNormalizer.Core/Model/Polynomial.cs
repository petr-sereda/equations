using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EquationNormalizer.Core.Model
{
    /// <summary>
    /// Многочлен.
    /// </summary>
    public class Polynomial : IEquatable<Polynomial>, IEnumerable<Summand>
    {
        private readonly List<Summand> _summands;

        public Polynomial(IReadOnlyCollection<Summand> summands)
        {
            if (summands == null)
                throw new ArgumentNullException("summands");

            _summands = summands.ToList();
        }

        /// <summary>
        /// Доступ к слагаемым многочлена по их порядковому номеру.
        /// </summary>
        public Summand this[int index]
        {
            get { return _summands[index]; }
        }

        /// <summary>
        /// Количество членов полинома.
        /// </summary>
        public int SummandsCount 
        {
            get { return _summands.Count; }
        }

        /// <summary>
        /// Приводит полином к каннической форме: суммирует все члены с одинаковыми переменными и упорядочивает слагаемые.
        /// </summary>
        public Polynomial ToCanonical()
        {
            var normalized = _summands.Select(s => s.Normalize()).ToList();

            for (int i = 0; i < normalized.Count - 1; i++)
            {
                for (int j = i + 1; j < normalized.Count; j++)
                {
                    if (normalized[i].AreVariablesSameWith(normalized[j]))
                    {
                        normalized[j] = normalized[j].Add(normalized[i]);
                    }
                }
            }

            normalized.Sort(new SummandsComparer());
            
            return new Polynomial(normalized);
        }

        #region Aux methods and interfaces implementations

        public bool Equals(Polynomial other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (_summands.Count != other._summands.Count)
                return false;

            for (int i = 0; i < _summands.Count; i++)
            {
                if (!this._summands[i].Equals(other._summands[i]))
                    return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Polynomial) obj);
        }

        public override int GetHashCode()
        {
            return (_summands != null ? _summands.Aggregate(0, (hash, s) => hash ^ s.GetHashCode()) : 0);
        }

        public IEnumerator<Summand> GetEnumerator()
        {
            return _summands.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return String.Concat(_summands.Select(s => s.ToString() + " "));
        }

        /// <summary>
        /// Сравнивает слагаемые между собой по принципу "самая переменная с самой большой степенью идет первой".
        /// Нужен для упорядочивания слагаемых в полиноме приведенному к каноническому виду. Работает в предположении, что все слагаемые уже нормализованы.
        /// </summary>
        /// <remarks>
        /// Т.к. он используется только в логике приведения к каноническому виду, специфичной для Полинома, 
        /// я включил его в класс Polynom, а не стал реализовывать IComparable в Summand.
        /// </remarks>
        private class SummandsComparer : IComparer<Summand>
        {
            public int Compare(Summand x, Summand y)
            {
                foreach (var pair in x.Variables.Zip(y.Variables, (vx, vy) => new {vx, vy}))
                {
                    if (pair.vx.Name[0] < pair.vy.Name[0])
                        return -1;
                    if (pair.vy.Name[0] < pair.vx.Name[0])
                        return 1;

                    if (pair.vx.Power < pair.vy.Power)
                        return 1;
                    if (pair.vy.Power < pair.vx.Power)
                        return -1;
                }

                return
                    x.Variables.Count == y.Variables.Count ? 0 : x.Variables.Count < y.Variables.Count ? -1 : 1;
            }
        }

        #endregion
    }
}
