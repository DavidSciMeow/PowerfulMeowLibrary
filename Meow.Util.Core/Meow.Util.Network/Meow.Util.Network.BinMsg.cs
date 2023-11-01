using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Meow.Util.Network
{
    /// <summary>
    /// 二进制报文生成类 (使用Marshal)
    /// </summary>
    /// <typeparam name="T">转换的内定类的类型</typeparam>
    public static class BinMsg<T>
    {
        /// <summary>
        /// 构建信息
        /// </summary>
        /// <param name="obj">需要转换的对象</param>
        /// <returns></returns>
        public static byte[] Build(T obj)
        {
#if DEBUG
            DateTime dt1 = DateTime.Now;
#endif
            byte[] buff;
            var size = Marshal.SizeOf(obj);
            buff = new byte[size];
            var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buff, 0);
            if(obj != null)
            {
                Marshal.StructureToPtr(obj, ptr, true);
#if DEBUG
                DateTime dt2 = DateTime.Now;
                Console.WriteLine($":: Marshal 序列化时间{dt2 - dt1} | PKG大小: {size} Bytes");
#endif
                return buff;
            }
            else
            {
                throw new Exception($"Object is null when Marshal Parsing") { HResult = -441 };
            }
            
        }
        /// <summary>
        /// 解密信息
        /// </summary>
        /// <param name="buff">缓存的byte</param>
        /// <returns></returns>
        public static T Decon(byte[] buff)
        {
#if DEBUG
            DateTime dt1 = DateTime.Now;
#endif
            var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buff, 0);
            var obj = Marshal.PtrToStructure<T>(ptr);
#if DEBUG
            DateTime dt2 = DateTime.Now;
            Console.WriteLine($":: Marshal 解析时间{dt2 - dt1} | Marshal类型: {typeof(T)}");
#endif
            return obj;
        }
    }
}
