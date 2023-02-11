namespace MeowCoreTest
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Meow.Voice.Silk.Encoder encoder = new();
            var r = encoder.Encode(args[1]).GetAwaiter().GetResult();
            r.ConvertTOBase64();
            System.Console.WriteLine("ends");
        }
    }
}

