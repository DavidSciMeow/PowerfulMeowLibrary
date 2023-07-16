using System;
using System.Numerics;

namespace Meow.Util.Math
{
    /// <summary>
    /// Fraction struct
    /// </summary>
    public readonly struct Fraction : IEquatable<Fraction>, IComparable<Fraction>, ISpanFormattable, IConvertible
    {
        private readonly BigInteger num;
        private readonly BigInteger den;

        /// <summary>
        /// Max Digit the double can hold
        /// </summary>
        public const int MAX_DIGIT = 128;
        /// <summary>
        /// is Fraction Init with a simple form (Default No)
        /// </summary>
        public static bool FractionCreateWithSimp { get; set; } = false;

        public Fraction(double numerator, double denominator)
        {
            if (denominator == 0)
            {
                throw new ArgumentException("Denominator cannot be zero.", nameof(denominator));
            }
            double t = FractionCreateWithSimp ? GCD(numerator, denominator) : 1;
            num = (BigInteger)(numerator / t);
            den = (BigInteger)(denominator / t);
        }
        public Fraction(long numerator, long denominator)
        {
            if (denominator == 0)
            {
                throw new ArgumentException("Denominator cannot be zero.", nameof(denominator));
            }
            double t = FractionCreateWithSimp ? GCD(numerator, denominator) : 1;
            num = (BigInteger)(numerator / t);
            den = (BigInteger)(denominator / t);
        }
        public Fraction(BigInteger numerator, BigInteger denominator)
        {
            if (denominator == 0)
            {
                throw new ArgumentException("Denominator cannot be zero.", nameof(denominator));
            }
            BigInteger t = FractionCreateWithSimp ? BigInteger.GreatestCommonDivisor(numerator, denominator) : 1;
            num = numerator / t;
            den = denominator / t;
        }
        public Fraction(Fraction a) => this = DoSimpFraction(a);

        /// <summary>
        /// convert double number into Fraction of Most Simplify form
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Fraction DoSimpDouble(double d)
        {
            double denominator = 1;
            for (int i = 0; i < MAX_DIGIT; i++)
            {
                if (d != (int)d)
                {
                    denominator *= 10;
                    d *= 10;
                }
            }
            double t = GCD(denominator, d);
            return new Fraction(d / t, denominator / t);
        }
        /// <summary>
        /// convert Fraction itself into Most Simplify form
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Fraction DoSimpFraction(Fraction d)
        {
            BigInteger t = BigInteger.GreatestCommonDivisor(d.num, d.den);
            return new Fraction(d.num / t, d.den / t);
        }

        /// <summary>
        /// GCD Finder
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double GCD(double a, double b)
        {
            if (a % b == 0) return b;
            return GCD(b, a % b);
        }
        /// <summary>
        /// GCD Finder
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static decimal GCD(decimal a, decimal b)
        {
            if (a % b == 0) return b;
            return GCD(b, a % b);
        }
        /// <summary>
        /// GCD Finder
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double GCD(long a, long b) => (long)BigInteger.GreatestCommonDivisor(a, b);

        public static implicit operator Fraction(double d) => DoSimpDouble(d);
        public static implicit operator Fraction(float d) => DoSimpDouble(d);
        public static implicit operator Fraction(long d) => DoSimpDouble(d);

        public static explicit operator double(Fraction b)
        {
            long dev = (long)BigInteger.DivRem(b.num, b.den, out var rem);
            double devr = (double)rem / (double)b.den;
            return dev + devr;
        }
        public static explicit operator (long intg, double remdg)(Fraction b)
        {
            long dev = (long)BigInteger.DivRem(b.num, b.den, out var rem);
            double devr = (double)rem / (double)b.den;
            return (dev, devr);
        }


        /// <summary>
        /// Fraction operator Positive
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Fraction operator +(Fraction a) => a;
        /// <summary>
        /// Fraction operator Negative
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Fraction operator -(Fraction a) => new(-a.num, a.den);
        /// <summary>
        /// Fraction operator Addition
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Fraction operator +(Fraction a, Fraction b) => new(a.num * b.den + b.num * a.den, a.den * b.den);
        /// <summary>
        /// Fraction operator Subtraction
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Fraction operator -(Fraction a, Fraction b) => a + (-b);
        /// <summary>
        /// Fraction operator Multiplication
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Fraction operator *(Fraction a, Fraction b) => new(a.num * b.num, a.den * b.den);
        /// <summary>
        /// Fraction operator Division
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Fraction operator /(Fraction a, Fraction b) => (b.num == 0) ? throw new DivideByZeroException() : new Fraction(a.num * b.den, a.den * b.num);
        /// <summary>
        /// Fraction DoSimpOperator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Fraction operator %(Fraction a, bool b)
        {
            if (b)
            {
                return DoSimpFraction(a);
            }
            else
            {
                return a;
            }
        }


        /// <summary>
        /// returns a num/den type 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{num}/{den}";


        public bool Equals(Fraction other) => num == other.num && den == other.den;
        public override bool Equals(object? obj) => obj is Fraction f && Equals(f);
        public override int GetHashCode() => num.GetHashCode() ^ den.GetHashCode();
        public int CompareTo(Fraction obj) => (double)(this - obj) == 0 ? 0 : (double)(this - obj) < 0 ? -1 : 1;

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public string ToString(string? format, IFormatProvider? formatProvider) => formatProvider is ICustomFormatter customFormatter ? customFormatter.Format(format, this, null) : ToString();
        public static bool operator ==(Fraction left, Fraction right) => left.Equals(right);
        public static bool operator !=(Fraction left, Fraction right) => !(left == right);
    }

    class FractionFormatter : IFormatProvider, ICustomFormatter
    {
        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            if(arg is Fraction a)
            {
                return format switch
                {
                    "Mixed" => $"{((long, double))a}",
                    _ => $"{a}",
                };
            }
            else
            {
                throw new ArgumentException(null, nameof(arg));
            }
        }

        public object? GetFormat(Type? formatType) => (formatType == typeof(ICustomFormatter)) ? this : null;
    }
}