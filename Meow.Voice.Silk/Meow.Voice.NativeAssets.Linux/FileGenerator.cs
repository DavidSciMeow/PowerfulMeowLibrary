using System.Diagnostics;

namespace Meow.Voice.NativeAssets.Linux
{
    static class StaticFile
    {
        static void GenerateFile(string path = "./")
        {
            OutputEncoderFile(path);
            OutputDecoderFile(path); 
            OutputFFmpegFile(path);
        }
        static void OutputFFmpegFile(string path = "./")
        {
            File.WriteAllBytes(path + "ffmpeg", Properties.Resources.ffmpeg);
            Chmods(path + "ffmpeg");
        }
        static void OutputEncoderFile(string path = "./")
        {
            File.WriteAllBytes(path + "encoder", Properties.Resources.encoder);
            Chmods(path + "encoder");
        }
        static void OutputDecoderFile(string path = "./")
        {
            File.WriteAllBytes(path + "decoder", Properties.Resources.encoder);
            Chmods(path + "decoder");
        }
        static void Chmods(string path)
        {
            try
            {
                var str = $"chmod +x {path}";
                using Process p = new();
                p.StartInfo.FileName = "bash";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = false;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.StandardInput.WriteLine(str);
                p.StandardInput.Close();
                var fd = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                p.Close();
                Console.WriteLine(fd);
            }
            catch
            {
                throw;
            }
        }
    }
}