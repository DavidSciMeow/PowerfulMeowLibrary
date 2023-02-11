namespace Meow.Voice.NativeAssets.Windows
{
    static class StaticFile
    {
        static void GenerateFile(string path = "./")
        {
            OutputEncoderFile(path);
            OutputDecoderFile(path);
        }
        static void OutputEncoderFile(string path = "./")
        {
            File.WriteAllBytes(path + "encoder.exe", Properties.Resources.encode);
        }
        static void OutputDecoderFile(string path = "./")
        {
            File.WriteAllBytes(path + "decoder.exe", Properties.Resources.decode);
        }
    }
}