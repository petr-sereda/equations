using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EquationNormalizer.Core.Model
{
    /// <summary>
    /// Representation of Polynomial. For example x^2 + y^3 - 12
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
        public Summand this[int index] => _summands[index];

        /// <summary>
        /// Количество членов полинома.
        /// </summary>
        public int SummandsCount => _summands.Count;

        /// <summary>
        /// Приводит полином к каннической форме: суммирует все члены с одинаковыми переменными и упорядочивает слагаемые.
        /// </summary>
        public Polynomial ToCanonical()
        {            
            // I dislike the code of this method, would be great to rewrite in future

            if (_summands.Count <= 1)
                return this;

            var summandList = new LinkedList<Summand>(_summands.Select(s => s.Normalize()));

            var current = summandList.First;

            // O(n^2) - bad
            while (current != summandList.Last)
            {
                for (var node = current.Next; node != null; node = node.Next)
                {
                    if (current.Value.AreVariablesSameWith(node.Value))
                    {
                        node.Value = node.Value.Add(current.Value);
                        current = current.Next;
                        break;
                    }
                }
            }

            var sorted = summandList.ToList();
            sorted.Sort(new SummandsComparer());

            return new Polynomial(sorted);
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
            return (_summands.Aggregate(0, (hash, s) => hash ^ s.GetHashCode()));
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
        /// Сравнивает слагаемые между собой по принципу "a variable with max power goes first, then order by alphabet, a constant goes last".
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
                if (x.IsConstant)
                    return 1;
                if (y.IsConstant)
                    return -1;

                foreach (var pair in x.Variables.Zip(y.Variables, (vx, vy) => new {vx, vy}))
                {                    
                    if (pair.vx.Power < pair.vy.Power)
                        return 1;
                    if (pair.vy.Power < pair.vx.Power)
                        return -1;

                    if (pair.vx.Name[0] < pair.vy.Name[0])
                        return -1;
                    if (pair.vy.Name[0] < pair.vx.Name[0])
                        return 1;
                }

                return
                    x.Variables.Count == y.Variables.Count ? 0 : x.Variables.Count < y.Variables.Count ? -1 : 1;
            }
        }

        #endregion
    }
}
