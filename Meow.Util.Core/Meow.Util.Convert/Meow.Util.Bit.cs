using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.Util.Convert.Bit
{
    /// <summary>
    /// 二进制帮助类
    /// <para> 本二进制帮助类采用如下索引方式 </para>
    /// <para>0x [7][6][5][4][3][2][1][0] </para>
    /// </summary>
    public struct BitSource : ISpanFormattable, IFormattable
    {
        private byte output = 0;
        /// <summary>
        /// 生成一个二进制帮助类(默认)
        /// <para> 本二进制帮助类采用如下索引方式 </para>
        /// <para>0x [7][6][5][4][3][2][1][0] </para>
        /// </summary>
        public BitSource() { output = 0; }
        /// <summary>
        /// 生成一个二进制帮助类(使用bool[])
        /// <para> 本二进制帮助类采用如下索引方式 </para>
        /// <para>0x [7][6][5][4][3][2][1][0] </para>
        /// </summary>
        /// <param name="flags">源(8位)</param>
        public BitSource(params bool[] flags) => output.SetBit(flags);
        /// <summary>
        /// 生成一个二进制帮助类(使用Byte)
        /// <para> 本二进制帮助类采用如下索引方式 </para>
        /// <para>0x [7][6][5][4][3][2][1][0] </para>
        /// </summary>
        /// <param name="data">源</param>
        public BitSource(byte data) => output = data;

        /// <summary>
        /// 返回Byte
        /// </summary>
        /// <returns></returns>
        public byte ToByte() => output;

        /// <summary>
        /// 索引位
        /// <para> 本二进制帮助类采用如下索引方式 </para>
        /// <para>0x [7][6][5][4][3][2][1][0] </para>
        /// </summary>
        /// <param name="index">第n位</param>
        /// <returns></returns>
        public bool this[int index]
        {
            get {
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
        public override string? ToString() => null;
        /// <inheritdoc/>
        public string ToString(string format) => output.ToString(format);
        /// <inheritdoc/>
        public string ToString(IFormatProvider provider) => output.ToString(provider);
        /// <inheritdoc/>
        public string ToString(string? format, IFormatProvider? provider) => output.ToString(format, provider);
        /// <inheritdoc/>
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => output.TryFormat(destination, out charsWritten, format, provider);
    }
    /// <summary>
    /// 比特工具类
    /// </summary>
    public static class BitUtil
    {
        /// <summary>
        /// 转换成二进制帮助类
        /// </summary>
        /// <param name="data">要转换的Byte数值</param>
        /// <returns></returns>
        public static BitSource ToBit(ref this byte data) => new(data);
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
