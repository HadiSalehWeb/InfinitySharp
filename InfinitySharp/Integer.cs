using System;

//Add modulo and pow
namespace InfinitySharp
{
    public enum OnIntegerOverflow
    {
        Throw,
        Overflow,
        Infinity
    }

    public abstract class Integer
    {
        protected abstract Integer Add(Integer n);
        protected abstract Integer Multiply(Integer n);
        protected abstract Integer Divide(Integer n);
        protected abstract Integer Negate();
        protected abstract bool IsEqualTo(Integer n);
        protected abstract bool IsGreaterThan(Integer n);
        protected abstract bool IsPositive();
        protected abstract bool IsZero();

        public static OnIntegerOverflow onOverflow = OnIntegerOverflow.Infinity;

        class FiniteInteger : Integer
        {
            int value;
            public FiniteInteger(int n)
            {
                value = n;
            }
            public override string ToString()
            {
                return value.ToString();
            }
            public override int GetHashCode()
            {
                return value.GetHashCode();
            }

            protected override Integer Add(Integer n)
            {
                if (n is FiniteInteger fn)
                    switch (onOverflow)
                    {
                        case OnIntegerOverflow.Throw:
                            try { return new FiniteInteger(checked(value + fn.value)); }
                            catch (OverflowException) { throw; }
                        case OnIntegerOverflow.Overflow:
                            return new FiniteInteger(unchecked(value + fn.value));
                        case OnIntegerOverflow.Infinity:
                        default:
                            try { return new FiniteInteger(checked(value + fn.value)); }
                            catch (OverflowException) { return value * Infinity; }
                    }

                else return n.Add(this);
            }

            protected override Integer Multiply(Integer n)
            {
                if (n is FiniteInteger fn)
                    switch (onOverflow)
                    {
                        case OnIntegerOverflow.Throw:
                            try { return new FiniteInteger(checked(value * fn.value)); }
                            catch (OverflowException) { throw; }
                        case OnIntegerOverflow.Overflow:
                            return new FiniteInteger(unchecked(value * fn.value));
                        case OnIntegerOverflow.Infinity:
                        default:
                            try { return new FiniteInteger(checked(value * fn.value)); }
                            catch (OverflowException) { return (value > 0 ? 1 : -1) * (fn.value > 0 ? 1 : -1) * Infinity; }
                    }

                else return n.Multiply(this);
            }

            protected override Integer Divide(Integer n)
            {
                if (n is FiniteInteger fn)
                    if (IsZero() && fn.IsZero()) return NaN;
                    else if (IsZero()) return new FiniteInteger(0);
                    else if (fn.IsZero()) return IsPositive() ? Infinity : -Infinity;
                    else return new FiniteInteger(value / fn.value);
                else if (n is Undefined)
                    return NaN;
                else if (n is InfiniteInteger)
                    return new FiniteInteger(0);

                throw new ArgumentException("Unsupported type " + n.GetType() + ". Value: " + n.ToString());
            }

            protected override Integer Negate()
            {
                switch (onOverflow)
                {
                    case OnIntegerOverflow.Throw:
                        try { return new FiniteInteger(checked(-value)); }
                        catch (OverflowException) { throw; }
                    case OnIntegerOverflow.Overflow:
                        return new FiniteInteger(unchecked(-value));
                    case OnIntegerOverflow.Infinity:
                    default:
                        try { return new FiniteInteger(checked(-value)); }
                        catch (OverflowException) { return (value > 0 ? 1 : -1) * -Infinity; }
                }
            }

            protected override bool IsEqualTo(Integer n)
            {
                if (n is FiniteInteger fn)
                    return value == fn.value;
                return n.IsEqualTo(this);
            }

            protected override bool IsGreaterThan(Integer n)
            {
                if (n is FiniteInteger fn)
                    return value > fn.value;
                return n.IsGreaterThan(this);
            }

            protected override bool IsPositive()
            {
                return value > 0;
            }

            protected override bool IsZero()
            {
                return value == 0;
            }

            public override bool Equals(object obj)
            {
                if (obj is Integer i)
                    return IsEqualTo(i);

                return false;
            }
        }

        class InfiniteInteger : Integer
        {
            public static InfiniteInteger PositiveInfinity;
            public static InfiniteInteger NegativeInfinity;
            static InfiniteInteger()
            {
                PositiveInfinity = new InfiniteInteger();
                NegativeInfinity = new InfiniteInteger();
            }
            public override string ToString()
            {
                return (IsPositive() ? "" : "-") + "Infinity";
            }
            public override int GetHashCode()
            {
                return IsPositive() ? int.MaxValue : int.MinValue;
            }

            protected override Integer Add(Integer n)
            {
                if (n is FiniteInteger)
                    return this;
                else if (n is Undefined)
                    return NaN;
                else if (n is InfiniteInteger nI)
                    if ((IsPositive() && nI.IsPositive()) || (!IsPositive() && !nI.IsPositive()))
                        return this;
                    else return NaN;

                throw new ArgumentException("Unsupported type " + n.GetType() + ". Value: " + n.ToString());
            }

            protected override Integer Multiply(Integer n)
            {
                if (n is FiniteInteger)
                    if (n.IsZero())
                        return NaN;
                    else return n.IsPositive() ? this : -this;
                else if (n is Undefined)
                    return NaN;
                else if (n is InfiniteInteger nI)
                    return nI.IsPositive() ? this : -this;

                throw new ArgumentException("Unsupported type " + n.GetType() + ". Value: " + n.ToString());
            }

            protected override Integer Divide(Integer n)
            {
                if (n is FiniteInteger)
                    return (n.IsZero() || n.IsPositive()) ? this : -this;
                else if (n is Undefined || n is InfiniteInteger)
                    return NaN;

                throw new ArgumentException("Unsupported type " + n.GetType() + ". Value: " + n.ToString());
            }

            protected override Integer Negate()
            {
                return Equals(PositiveInfinity) ? NegativeInfinity : PositiveInfinity;
            }

            protected override bool IsEqualTo(Integer n)
            {
                if (n is FiniteInteger || n is Undefined)
                    return false;
                else if (n is InfiniteInteger nI)
                    return (IsPositive() && nI.IsPositive()) || (!IsPositive() && !nI.IsPositive());

                throw new ArgumentException("Unsupported type " + n.GetType() + ". Value: " + n.ToString());
            }

            protected override bool IsGreaterThan(Integer n)
            {
                if (n is FiniteInteger)
                    return IsPositive();
                else if (n is Undefined)
                    return false;
                else if (n is InfiniteInteger nI)
                    return IsPositive() && !nI.IsPositive();

                throw new ArgumentException("Unsupported type " + n.GetType() + ". Value: " + n.ToString());
            }

            protected override bool IsPositive()
            {
                return Equals(PositiveInfinity);
            }

            protected override bool IsZero()
            {
                return false;
            }

            public override bool Equals(object obj)
            {
                return (object)this == obj;
            }
        }

        class Undefined : Integer
        {
            public static Undefined UndefinedNumber;
            static Undefined()
            {
                UndefinedNumber = new Undefined();
            }
            public override string ToString()
            {
                return "NaN";
            }
            public override int GetHashCode()
            {
                return 0;
            }

            protected override Integer Add(Integer n)
            {
                return UndefinedNumber;
            }

            protected override Integer Multiply(Integer n)
            {
                return UndefinedNumber;
            }

            protected override Integer Divide(Integer n)
            {
                return UndefinedNumber;
            }

            protected override Integer Negate()
            {
                return UndefinedNumber;
            }

            protected override bool IsEqualTo(Integer n)
            {
                return false;
            }

            protected override bool IsGreaterThan(Integer n)
            {
                return false;
            }

            protected override bool IsPositive()
            {
                return false;
            }

            protected override bool IsZero()
            {
                return false;
            }

            public override bool Equals(object obj)
            {
                return (object)this == obj;
            }
        }

        public static implicit operator Integer(int n)
        {
            return new FiniteInteger(n);
        }

        public static Integer operator -(Integer n1)
        {
            return n1.Negate();
        }

        public static Integer operator +(Integer n1, Integer n2)
        {
            return n1.Add(n2);
        }

        public static Integer operator -(Integer n1, Integer n2)
        {
            return n1.Add(-n2);
        }

        public static Integer operator *(Integer n1, Integer n2)
        {
            return n1.Multiply(n2);
        }

        public static Integer operator /(Integer n1, Integer n2)
        {
            return n1.Divide(n2);
        }

        public static bool operator ==(Integer n1, Integer n2)
        {
            return n1.IsEqualTo(n2);
        }

        public static bool operator !=(Integer n1, Integer n2)
        {
            return !n1.IsEqualTo(n2);
        }

        public static bool operator >(Integer n1, Integer n2)
        {
            return n1.IsGreaterThan(n2);
        }

        public static bool operator <(Integer n1, Integer n2)
        {
            return n2.IsGreaterThan(n1);
        }

        public static Integer Pow(Integer x, Integer y)
        {
            throw new NotImplementedException();
        }

        public static Integer Infinity
        {
            get
            {
                return InfiniteInteger.PositiveInfinity;
            }
        }

        public static Integer NaN
        {
            get
            {
                return Undefined.UndefinedNumber;
            }
        }

        public bool IsFinite()
        {
            return this is FiniteInteger;
        }

        public bool IsInfinite()
        {
            return this is InfiniteInteger;
        }

        public bool IsNaN()
        {
            return this is Undefined;
        }

        public static Integer Finite(int n)
        {
            return new FiniteInteger(n);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Integer i)
                return IsEqualTo(i);

            return false;
        }
    }
}
