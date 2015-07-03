using System;
using System.Linq;

namespace EquationNormalizer.Core.Model
{
    /// <summary>
    /// Алгебраическое уравнение из условия задачи. Состоит из двух частей: левого и правого многочленов (Polynomial).
    /// </summary>
    public class Equation : IEquatable<Equation>
    {
        public Polynomial Left { get; private set; }

        public Polynomial Right { get; private set; }

        public Equation(Polynomial left, Polynomial right)
        {
            Left = left;
            Right = right;
        }

        public Equation ToCanonical()
        {
            // просто переносим все члены в левую часть и далее приводим ее к каноническому виду
            var allMemebers = Right.Select(s => s.Multiply(-1)).Concat(Left).ToArray();            
            var newLeft = new Polynomial(allMemebers).ToCanonical();

            // правая часть канонического уравнения - константа 0
            var newRight = new Polynomial(new[] {new Summand(0)});

            return new Equation(newLeft, newRight);
        }

        #region Equality and HasCode
		
        public bool Equals(Equation other)
        {
            return Equals(Left, other.Left) && Equals(Right, other.Right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Equation) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Left != null ? Left.GetHashCode() : 0)*397) ^ (Right != null ? Right.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return Left.ToString() + " = " + Right.ToString();
        }

        #endregion    
    }
}
