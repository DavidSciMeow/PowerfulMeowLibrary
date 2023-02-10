
namespace MeowCoreTest
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var pf = new Meow.Voice.Silk.Encoder(log:true);
            pf.Encode(args[0]).GetAwaiter().GetResult().ConvertFile(args[1]);
        }
    }
}

