using System.Reflection;

namespace Meow.Voice.Silk
{
    /// <summary>
    /// 抽象类::Silk转换器
    /// </summary>
    public abstract class SilkConvertor
    {
        /// <summary>
        /// 平台类型
        /// </summary>
        protected PlatformID Platformid { get; set; }
    }
    /// <summary>
    /// 编码器接口
    /// </summary>
    public interface IEncodeable
    {
        /// <summary>
        /// 编码逻辑
        /// </summary>
        /// <param name="filePath">文件路径(全限定)</param>
        /// <returns></returns>
        public Task<SilkReturn> Encode(string filePath);
    }


    /// <summary>
    /// Silk编码器
    /// </summary>
    public class Encoder : SilkConvertor, IEncodeable
    {
        void PreOSFileCheck()
        {
            Platformid = Environment.OSVersion.Platform;
            if (Platformid == PlatformID.Unix)
            {
                if (!File.Exists("./encoder") || !File.Exists("./decoder"))
                {
                    try
                    {
                        var a = Assembly.LoadFile(Path.Combine(AppContext.BaseDirectory, "Meow.Voice.NativeAssets.Linux.dll"));
                        var type = a.GetType("Meow.Voice.NativeAssets.Linux.StaticFile");
                        var methods = type?.GetMethod("GenerateFile", BindingFlags.Static | BindingFlags.NonPublic);
                        if (methods != null)
                        {
                            methods.Invoke(null, new string[] { "./" });
                        }
                        else
                        {
                            throw new Exception("Nuget Or Lib Not Loaded");
                        }
                    }
                    catch
                    {
                        Console.WriteLine("[PreOSFileCheck - Linux] :: Do not Use Produce One File Mode");
                        throw new Exception("Do not Use Produce One File Mode");
                    }
                }
            }
            else if (Platformid == PlatformID.Win32NT)
            {
                if (!File.Exists("./encoder.exe") || !File.Exists("./decoder.exe") || !File.Exists("./ffmpeg.exe"))
                {
                    try
                    {
                        var a = Assembly.LoadFile(Path.Combine(AppContext.BaseDirectory, "Meow.Voice.NativeAssets.Windows.dll"));
                        var type = a.GetType("Meow.Voice.NativeAssets.Windows.StaticFile");
                        var methods = type?.GetMethod("GenerateFile", BindingFlags.Static | BindingFlags.NonPublic);
                        if (methods != null)
                        {
                            methods.Invoke(null, new string[] { "./" });
                        }
                        else
                        {
                            throw new Exception("Nuget Or Lib Not Loaded");
                        }
                    }
                    catch
                    {
                        Console.WriteLine("[PreOSFileCheck - Windows] :: Do not Use Produce One File Mode");
                        throw new Exception("Do not Use Produce One File Mode");
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
        Task<SilkReturn> WindowsEncodingProcess(string filePath)
        {
            return Task<SilkReturn>.Factory.StartNew(() =>
            {
                try
                {
                    var uuid = Guid.NewGuid().ToString();
                    Console.WriteLine("[FFMPEG] CONVERTING");
                    SilkUtilProcess p1 = new($"./ffmpeg.exe", $"-hide_banner -i {filePath} -f s16le -b:a {KBitRate * 1000} -ar {SampRate} -ac 1 ./temp/{uuid}.pcm -y {(Log ? "" : "-loglevel panic")}");
                    p1.WaitAndExit();
                    Console.WriteLine("[ENCODE] CONVERTING");
                    SilkUtilProcess p2 = new("./encoder.exe", $"./temp/{uuid}.pcm ./temp/{uuid}.silk -packetlength {PacketLength} -rate {KBitRate * 1000} -Fs_API {SampRate} -Fs_maxInternal {SampRate} -tencent {(Log ? "" : "-quiet")}");
                    p2.WaitAndExit();
                    Task.Delay(100).GetAwaiter().GetResult();
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
        Task<SilkReturn> LinuxEncodingProcess(string filePath)
        {
            return Task<SilkReturn>.Factory.StartNew(() =>
            {
                try
                {
                    var uuid = Guid.NewGuid().ToString();
                    Console.WriteLine("[FFMPEG] CONVERTING");
                    SilkUtilProcess p1 = new($"ffmpeg", $"-hide_banner -i {filePath} -f s16le -b:a {KBitRate * 1000} -ar {SampRate} -ac 1 ./temp/{uuid}.pcm -y -loglevel panic");
                    p1.WaitAndExit();
                    Console.WriteLine("[ENCODE] CONVERTING");
                    SilkUtilProcess p2 = new("./encoder", $"./temp/{uuid}.pcm ./temp/{uuid}.silk -packetlength {PacketLength} -rate {KBitRate * 1000} -Fs_API {SampRate} -Fs_maxInternal {SampRate} -tencent -quiet");
                    p2.WaitAndExit();
                    Task.Delay(100).GetAwaiter().GetResult();
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
            if(sampRate > 24000)
            {
                throw new Exception("Max SampRate is 24000");
            }
            KBitRate = kBitRate;
            if (kBitRate > 96)
            {
                throw new Exception("Max KBitRate is 96");
            }
            PacketLength = packetLength;
            if (packetLength != 20)
            {
                Console.WriteLine("[CONV] PacketLength is Not 20, becareful what u have done");
            }
            Log = log;
            PreOSFileCheck();//检测系统模式
        }

        /// <inheritdoc/>
        public Task<SilkReturn> Encode(string filePath)
        {
            if (Platformid == PlatformID.Unix)
            {
                return LinuxEncodingProcess(filePath);
            }
            else if (Platformid == PlatformID.Win32NT)
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