using System.Reflection;

namespace Meow.Voice.Silk
{
    public class Encoder
    {
        PlatformID platformid;
        void PreOSFileCheck()
        {
            platformid = Environment.OSVersion.Platform;
            if (platformid == PlatformID.Unix)
            {
                if (!File.Exists("./encoder") || !File.Exists("./decoder"))
                {
                    var a = Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, "Meow.Voice.NativeAssets.Linux.dll"));
                    var methods = a.GetType("Meow.Voice.NativeAssets.Windows.StaticFile")?.GetMethod("GenerateFile");
                    if (methods != null)
                    {
                        methods.Invoke(null, new string[] { "./" });
                    }
                    else
                    {
                        throw new Exception("Nuget Or Lib Not Loaded");
                    }
                }
            }
            else if (platformid == PlatformID.Win32NT)
            {
                if (!File.Exists("./encoder.exe") || !File.Exists("./decoder.exe") || !File.Exists("./decoder.exe"))
                {
                    var a = Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, "Meow.Voice.NativeAssets.Windows.dll"));
                    var methods = a.GetType("Meow.Voice.NativeAssets.Windows.StaticFile")?.GetMethod("GenerateFile");
                    if (methods != null)
                    {
                        methods.Invoke(null, new string[] { "./" });
                    }
                    else
                    {
                        throw new Exception("Nuget Or Lib Not Loaded");
                    }
                }
            }
            else
            {
                throw new Exception("System Type Not Supported");
            }

            if (!Directory.Exists("./temp"))
            {
                Directory.CreateDirectory("./temp");
            }

        }
        Task<SilkReturn> WindowsEncodingProcess(string FilePath)
        {
            return Task<SilkReturn>.Factory.StartNew(() =>
            {
                try
                {
                    var uuid = Guid.NewGuid().ToString();
                    Console.WriteLine("[FFMPEG] CONVERTING");
                    SilkUtilProcess p1 = new($"./ffmpeg.exe", $"-hide_banner -i {FilePath} -f s16le -b:a {KBitRate * 1000} -ar {SampRate} -ac 1 ./temp/{uuid}.pcm -y {(Log ? "" : "-loglevel panic")}");
                    p1.WaitAndExit();
                    Console.WriteLine("[ENCODE] CONVERTING");
                    SilkUtilProcess p2 = new("./encoder.exe", $"./temp/{uuid}.pcm ./temp/{uuid}.silk -packetlength {PacketLength} -rate {KBitRate * 1000} -Fs_API {SampRate} -Fs_maxInternal {SampRate} -tencent {(Log ? "" : "-quiet")}");
                    p2.WaitAndExit();
                    File.Delete($"./temp/{uuid}.pcm");
                    var pk = File.ReadAllBytes($"./temp/{uuid}.silk");
                    File.Delete($"./temp/{uuid}.silk");
                    Console.WriteLine("[ENCODE] COMPLETE");
                    return new(pk);
                }
                catch
                {
                    throw;
                }
            });
        }
        Task<SilkReturn> LinuxEncodingProcess(string FilePath)
        {
            return Task<SilkReturn>.Factory.StartNew(() =>
            {
                try
                {
                    var uuid = Guid.NewGuid().ToString();
                    Console.WriteLine("[FFMPEG] CONVERTING");
                    SilkUtilProcess p1 = new($"ffmpeg", $"-hide_banner -i {FilePath} -f s16le -b:a {KBitRate * 1000} -ar {SampRate} -ac 1 ./temp/{uuid}.pcm -y -loglevel panic");
                    p1.WaitAndExit();
                    Console.WriteLine("[ENCODE] CONVERTING");
                    SilkUtilProcess p2 = new("./encoder", $"./temp/{uuid}.pcm ./temp/{uuid}.silk -packetlength {PacketLength} -rate {KBitRate * 1000} -Fs_API {SampRate} -Fs_maxInternal {SampRate} -tencent -quiet");
                    p2.WaitAndExit();
                    File.Delete($"./temp/{uuid}.pcm");
                    var pk = File.ReadAllBytes($"./temp/{uuid}.silk");
                    File.Delete($"./temp/{uuid}.silk");
                    Console.WriteLine("[ENCODE] COMPLETE");
                    return new(pk);
                }
                catch
                {
                    throw;
                }
            });
        }

        /// <summary>
        /// 音频采样率(最高24000)
        /// </summary>
        public int SampRate { get; } = 24000;
        /// <summary>
        /// 音频比特率(最高96k)
        /// </summary>
        public int KBitRate { get; } = 96;
        /// <summary>
        /// 单个组包长度(默认20ms)
        /// </summary>
        public int PacketLength { get; } = 20;
        /// <summary>
        /// 是否记录转换日志
        /// </summary>
        public bool Log { get; } = false;
        /// <summary>
        /// 构造解码器<br/>如果您不知道参数是什么,请不要修改
        /// </summary>
        /// <param name="sampRate">音频采样率(最高24000)</param>
        /// <param name="kBitRate">音频比特率(最高96k)</param>
        /// <param name="packetLength">单个组包长度(默认20ms)</param>
        /// <param name="log">是否记录转换日志</param>
        public Encoder(int sampRate = 24000, int kBitRate = 96, int packetLength = 20, bool log = false)
        {
            SampRate = sampRate;
            KBitRate = kBitRate;
            PacketLength = packetLength;
            Log = log;
            PreOSFileCheck();//检测系统模式
        }
        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="filePath">文件位置</param>
        /// <returns></returns>
        /// <exception cref="Exception">系统类型模式支持</exception>
        public Task<SilkReturn> Encode(string filePath)
        {
            if (platformid == PlatformID.Unix)
            {
                return LinuxEncodingProcess(filePath);
            }
            else if (platformid == PlatformID.Win32NT)
            {
                return WindowsEncodingProcess(filePath);
            }
            else
            {
                throw new Exception("System Type Not Supported");
            }
        }
    }
}