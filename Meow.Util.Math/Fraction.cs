using System;
using System.Numerics;

namespace Meow.Util.Math
{
    /// <summary>
    /// Fraction struct
    /// </summary>
    public readonly struct Fraction : IEquatable<Fraction>, IComparable<Fraction>
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

        /// <summary>
        /// generate a Fraction
        /// </summary>
        /// <param name="numerator">numerator</param>
        /// <param name="denominator">denominator</param>
        /// <exception cref="ArgumentException"></exception>
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
        /// <summary>
        /// generate a Fraction
        /// </summary>
        /// <param name="numerator">numerator</param>
        /// <param name="denominator">denominator</param>
        /// <exception cref="ArgumentException"></exception>
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
        /// <summary>
        /// generate a Fraction
        /// </summary>
        /// <param name="numerator">numerator</param>
        /// <param name="denominator">denominator</param>
        /// <exception cref="ArgumentException"></exception>
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


        public static (long, Fraction)GetMixedFraction(Fraction b)
        {
            long dev = (long)BigInteger.DivRem(b.num, b.den, out var rem);
            double devr = (double)rem / (double)b.den;
            return (dev, devr);
        }
        public static (long, double) GetMixedFractionDoubleForm(Fraction b)
        {
            long dev = (long)BigInteger.DivRem(b.num, b.den, out var rem);
            double devr = (double)rem / (double)b.den;
            return (dev, devr);
        }
        public static double GetDoubleForm(Fraction b)
        {
            long dev = (long)BigInteger.DivRem(b.num, b.den, out var rem);
            double devr = (double)rem / (double)b.den;
            return dev + devr;
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

        public static explicit operator double(Fraction b) => GetDoubleForm(b);
        public static explicit operator (long intg, double remdg)(Fraction b) => GetMixedFractionDoubleForm(b);
        public static explicit operator (long intg, Fraction remdg)(Fraction b) => GetMixedFraction(b);

        /// <inheritdoc/>
        public static Fraction operator +(Fraction a) => a;
        /// <inheritdoc/>
        public static Fraction operator -(Fraction a) => new(-a.num, a.den);
        /// <inheritdoc/>
        public static Fraction operator +(Fraction a, Fraction b) => new(a.num * b.den + b.num * a.den, a.den * b.den);
        /// <inheritdoc/>
        public static Fraction operator -(Fraction a, Fraction b) => a + (-b);
        /// <inheritdoc/>
        public static Fraction operator *(Fraction a, Fraction b) => new(a.num * b.num, a.den * b.den);
        /// <inheritdoc/>
        public static Fraction operator /(Fraction a, Fraction b) => (b.num == 0) ? throw new DivideByZeroException() : new Fraction(a.num * b.den, a.den * b.num);
        /// <summary>
        /// Fraction DoSimpOperator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Fraction operator !(Fraction a) => DoSimpFraction(a);
        /// <summary>
        /// Fraction GetMixed Operator
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static (long intg, double remdg) operator ~(Fraction a) => GetMixedFractionDoubleForm(a);

        /// <inheritdoc/>
        public static bool operator ==(Fraction left, Fraction right) => left.Equals(right);
        /// <inheritdoc/>
        public static bool operator !=(Fraction left, Fraction right) => !(left == right);

        /// <summary>
        /// returns a num/den type 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{num}/{den}";
        /// <inheritdoc/>
        public bool Equals(Fraction other) => num == other.num && den == other.den;
        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is Fraction f && Equals(f);
        /// <inheritdoc/>
        public override int GetHashCode() => num.GetHashCode() ^ den.GetHashCode();
        /// <inheritdoc/>
        public int CompareTo(Fraction obj) => (double)(this - obj) == 0 ? 0 : (double)(this - obj) < 0 ? -1 : 1;




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