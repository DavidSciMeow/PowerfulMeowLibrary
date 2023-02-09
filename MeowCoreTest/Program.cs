
namespace MeowCoreTest
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            _ = args;
            var pf = new Meow.Voice.Silk.Encoder();
            pf.Encode(@"D:\Users\David\Desktop\ASPQR\平原之舞BF4.mp3").GetAwaiter().GetResult().ConvertFile(@"D:\Users\David\Desktop\ASPQR\a.silk");
        }
    }
}

