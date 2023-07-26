using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Meow.Util.Convert.Bit
{
    /// <summary>
    /// 
    /// </summary>
    public class LargeBit : IDisposable, IEquatable<LargeBit>
    {
        /*-locals-*/
        /// <summary>
        /// 逻辑指针
        /// </summary>
        public readonly bool[] Ptr;

        /*-private locals-*/
        private bool disposedValue;

        /*-Init-*/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="blist"></param>
        public LargeBit(bool[] blist) => Ptr = BitUtil.ReferenceFalse(blist);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        public LargeBit(string s)
        {
            var p = new bool[s.LongCount()];
            for (int i = 0; i < s.Length; i++)
            {
                p[i] = s[i] != '0';
            }
            Ptr = BitUtil.ReferenceFalse(p);
        }

        /*-methods-*/
        
        /// <summary>
        /// 设置指针的某一位, 本方法时间复杂度为 <b>O(1)</b> <br/>
        /// Set one <b>Bit</b> in specific position of this Ptrs, which have a <i>O(1)</i> Time complexity 
        /// </summary>
        /// <param name="position">
        /// 指针的某一位,<b>高位为0</b> <br/>
        /// the position you want to alt, <i>which High bit is 0</i>
        /// </param>
        /// <param name="state">
        /// 要设置到的状态 <b>(true=1,false=0)</b> <br/>
        /// the state you want to set, which <i>true=1</i> and <i>false=0</i> bitwise.
        /// </param>
        public void SetBit(long position, bool state) => Ptr[position] = state;
        /// <summary>
        /// 获取在某位置的一个位, 本方法时间复杂度为 <b>O(1) <br/>
        /// Get the bit in Position that you select, which have a <i>O(1)</i> Time complexity 
        /// </summary>
        /// <param name="position">
        /// 指针的某一位,<b>高位为0</b> <br/>
        /// the position you want to alt, <i>which High bit is 0</i>
        /// </param>
        /// <returns>
        /// 获取到的位 <br/>
        /// the bit in Position that you select
        /// </returns>
        public bool GetBit(long position) => Ptr[position];
        /// <summary>
        /// 获取整个数值组, 本方法时间复杂度为 <b>O(1)</b> <br/>
        /// Get the whole Set of this Instance, which have a <i>O(1)</i> Time complexity 
        /// </summary>
        /// <returns>
        /// 获取的数值组
        /// The Sets (which in list of boolean)
        /// </returns>
        public bool[] GetGroupList() => Ptr;
        /// <summary>
        /// 转换成字节数组
        /// </summary>
        /// <param name="input">
        /// 输入的布尔数组
        /// </param>
        /// <returns>
        /// 比特字节数组
        /// </returns>
        /// <exception cref="ArgumentException">
        /// 
        /// </exception>
        public byte[] ToByteArray()
        {
            if (Ptr.LongLength % 8 != 0)
            {
                throw new ArgumentException(null, nameof(Ptr));
            }
            byte[] ret = new byte[(long)Math.Ceiling(Ptr.LongLength / (double)8)];
            for (int i = 0; i < Ptr.LongLength; i += 8)
            {
                int value = 0;
                for (int j = 0; j < 8; j++)
                {
                    if (Ptr[i + j])
                    {
                        value += 1 << (7 - j);
                    }
                }
                ret[i / 8] = (byte)value;
            }
            return ret;
        }

        /*-operations-*/
        /// <summary>
        /// 获取或设置本类型的寄存指针的某一位 <br/>
        /// Get/Set the Exist bit of this Type Instance
        /// </summary>
        /// <param name="pos">
        /// 位,<b>高位为0</b> <br/>
        /// Bits Position, <i>which High bit is 0</i>
        /// </param>
        /// <returns>
        /// 获得的布尔值 <br/>
        /// The boolean Value that retrive
        /// </returns>
        public bool this[long pos] { get { return GetBit(pos); } set { SetBit(pos, value); } }
        /// <summary>
        /// 默认以[0],[1]表示的二进制值 <br/>
        /// use default display to show the sequence with 0,1 which is binary
        /// </summary>
        /// <returns>
        /// 以[0],[1]表示的二进制值字符串 <br/>
        /// the string contains the 0,1
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new();
            if(Ptr.LongLength > 0)
            {
                for (int i = 0; i < Ptr.LongLength; i++)
                {
                    sb.Append(Ptr[i] ? '1' : '0');
                }
            }
            else
            {
                sb.Append('0');
            }
            return sb.ToString();
        }

        /*-Interfaces-*/
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="disposing"><inheritdoc/></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }
                disposedValue = true;
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="other"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool Equals(LargeBit? other)
        {
            if (other == null)
            {
                return false;
            }
            else if (other.Ptr.LongLength != Ptr.LongLength)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < Ptr.LongLength; i++)
                {
                    if (Ptr[i] != other.Ptr[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /*-operators-*/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static LargeBit operator +(LargeBit a, LargeBit b) => new(BitUtil.Add(a.Ptr,b.Ptr));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static LargeBit operator <<(LargeBit a, int k) => new(BitUtil.LeftPad(a.Ptr,k));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static LargeBit operator *(LargeBit a, LargeBit b) => new(BitUtil.Mul(a.Ptr,b.Ptr));

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="b"><inheritdoc/></param>
        public static explicit operator bool[](LargeBit b) => b.GetGroupList();

    }

    /// <summary>
    /// 二进制帮助类
    /// <para> 本二进制帮助类采用如下索引方式 </para>
    /// <para>0x [7][6][5][4][3][2][1][0] </para>
    /// </summary>
    public struct Bit : ISpanFormattable, IFormattable
    {
        private byte output = 0;
        /// <summary>
        /// 生成一个二进制帮助类(默认) <br/>
        /// 本二进制帮助类采用如下索引方式 <br/>
        /// 0x [7][6][5][4][3][2][1][0]
        /// </summary>
        public Bit() { output = 0; }
        /// <summary>
        /// 生成一个二进制帮助类(使用bool[])
        /// 本二进制帮助类采用如下索引方式 <br/>
        /// 0x [7][6][5][4][3][2][1][0]
        /// </summary>
        /// <param name="flags">源(8位)</param>
        public Bit(params bool[] flags) => output.SetBit(flags);
        /// <summary>
        /// 生成一个二进制帮助类(使用Byte)
        /// 本二进制帮助类采用如下索引方式 <br/>
        /// 0x [7][6][5][4][3][2][1][0]
        /// </summary>
        /// <param name="data">源</param>
        public Bit(byte data) => output = data;

        /// <summary>
        /// 返回Byte
        /// </summary>
        /// <returns></returns>
        public readonly byte ToByte() => output;

        /// <summary>
        /// 索引位
        /// 本二进制帮助类采用如下索引方式 <br/>
        /// 0x [7][6][5][4][3][2][1][0]
        /// </summary>
        /// <param name="index">第n位</param>
        /// <returns></returns>
        public bool this[int index]
        {
            readonly get {
                if (index > 7 || index < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                return output.GetBit(index + 1); 
            }
            set {
                if (index > 7 || index < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                output.SetBit(index + 1, value);
            }
        }

        /// <inheritdoc/>
        public static implicit operator Bit(byte d) => new(d);
        /// <inheritdoc/>
        public static explicit operator byte(Bit d) => d.output;

        /// <inheritdoc/>
        public override readonly string? ToString() => null;
        /// <inheritdoc/>
        public readonly string ToString(string format) => output.ToString(format);
        /// <inheritdoc/>
        public readonly string ToString(IFormatProvider provider) => output.ToString(provider);
        /// <inheritdoc/>
        public readonly string ToString(string? format, IFormatProvider? provider) => output.ToString(format, provider);
        /// <inheritdoc/>
        public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => output.TryFormat(destination, out charsWritten, format, provider);
    }

    /// <summary>
    /// 比特工具类
    /// </summary>
    public static class BitUtil
    {
        /// <summary>
        /// 半加器
        /// </summary>
        /// <param name="a">真值A</param>
        /// <param name="b">真值B</param>
        /// <returns>和/进位</returns>
        public static (bool Sum, bool Carry) HalfAdder(bool a, bool b) => (a != b, a && b);
        /// <summary>
        /// 全加器
        /// </summary>
        /// <param name="a">真值A</param>
        /// <param name="b">真值B</param>
        /// <param name="c">上位进位</param>
        /// <returns>和/进位</returns>
        public static (bool Sum, bool Carry) FullAdder(bool a, bool b, bool c = false)
        {
            var (Sum, Carry) = HalfAdder(a, b);
            var (Sumi, Carryi) = HalfAdder(Sum, c);
            var Rcarry = Carryi || Carry;
            return (Sumi, Rcarry);
        }
        /// <summary>
        /// 二进制加法
        /// </summary>
        /// <param name="a">
        /// 加数
        /// </param>
        /// <param name="b">
        /// 被加数
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static bool[] Add(bool[] a, bool[] b)
        {
            long maxlength = a.LongLength > b.LongLength ? a.LongLength : b.LongLength;
            bool[] ret = new bool[maxlength + 1];
            bool carry = false;
            for (long i = maxlength - 1; i >= 0; i--)
            {
                var (s, c) = FullAdder(
                    a.LongLength > i && a[a.LongLength - 1 - i],
                    b.LongLength > i && b[b.LongLength - 1 - i],
                    carry);
                carry = c;
                ret[i] = s;
            }
            ret[0] = carry;
            return ret;
        }
        /// <summary>
        /// 二进制左移(进位)
        /// </summary>
        /// <param name="a">原数组</param>
        /// <param name="k">左移位数</param>
        /// <returns>结果组</returns>
        public static bool[] LeftPad(bool[] a, long k = 1)
        {
            bool[] b = new bool[a.LongLength + k];
            for (long i = 0; i < a.LongLength; i++)
            {
                b[i] = a[i];
            }
            return b;
        }
        /// <summary>
        /// 二进制乘法
        /// </summary>
        /// <param name="a">
        /// 
        /// </param>
        /// <param name="b">
        /// 
        /// </param>
        public static bool[] Mul(bool[] a, bool[] b)
        {
            bool[] temp = new bool[] { false };
            for (long i = b.LongLength - 1; i >= 0; i--)
            {
                if (b[i]) //被乘数为1, 乘数左移
                {
                    var r = LeftPad(a, b.LongLength - 1 - i);
                    temp = Add(temp, r);
                }
            }
            return temp;
        }

        /// <summary>
        /// 对比化简
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static bool[] ReferenceFalse(bool[] a)
        {
            long dd = a.LongLength;
            for (long i = 0; i < a.LongLength; i++) // O(N/2)
            {
                if (a[i])
                {
                    dd = i;
                    break;
                }
            }
            bool[] ret = new bool[a.LongLength - dd];
            for (long i = a.LongLength - 1; i >= dd; i--) // O(N-k)
            {
                ret[i - dd] = a[i];
            }
            return ret;
        }

        /// <summary>
        /// 设置Byte的某一位到某个状态
        /// </summary>
        /// <param name="data">源</param>
        /// <param name="index">位(1-8)</param>
        /// <param name="flag">位状态</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void SetBit(ref this byte data, int index, bool flag)
        {
            if (index > 8 || index < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            int v = index < 2 ? index : (2 << (index - 2));
            data = flag ? (byte)(data | v) : (byte)(data & ~v);
        }
        /// <summary>
        /// 设置Byte的所有位
        /// </summary>
        /// <param name="data">源</param>
        /// <param name="flags">位状态</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void SetBit(ref this byte data, params bool[] flags)
        {
            if (flags.Length > 8 || flags.Length < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(flags));
            }
            for (int i = 0; i < flags.Length; i++)
            {
                data.SetBit(i + 1, flags[i]);
            }
        }
        /// <summary>
        /// 获取Byte的所有位
        /// </summary>
        /// <param name="this">源</param>
        /// <returns></returns>
        public static bool[] GetBit(this byte @this)
        {
            bool[] vs = new bool[8];
            for (int i = 0; i < 7; i++)
            {
                vs[i] = @this.GetBit(i);
            }
            return vs;
        }
        /// <summary>
        /// 获取Byte的某一位
        /// </summary>
        /// <param name="this">源</param>
        /// <param name="index">位(1-8)</param>
        /// <returns></returns>
        public static bool GetBit(this byte @this, int index)
        {
            byte x;
            switch (index)
            {
                case 1: { x = 0x01; } break;
                case 2: { x = 0x02; } break;
                case 3: { x = 0x04; } break;
                case 4: { x = 0x08; } break;
                case 5: { x = 0x10; } break;
                case 6: { x = 0x20; } break;
                case 7: { x = 0x40; } break;
                case 8: { x = 0x80; } break;
                default: { return false; }
            }
            return (@this & x) == x;
        }
    }
}
