
namespace MeowCoreTest
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            using var p = new LinuxCommand("mkdir", "a");
        }
    }
}

