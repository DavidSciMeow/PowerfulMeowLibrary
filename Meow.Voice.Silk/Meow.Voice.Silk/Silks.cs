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
        /// <summary>
        /// 音频采样率(最高24000)
        /// </summary>
        public int SampRate { get; protected set; } = 24000;
        /// <summary>
        /// 是否记录转换日志
        /// </summary>
        public bool Log { get; protected set; } = false;
        /// <summary>
        /// FFMPEG使用文件名
        /// </summary>
        protected string FFmpegFileName { get; set; } = "";
        /// <summary>
        /// Convertor使用文件名
        /// </summary>
        protected string ConvertorFileName { get; set; } = "";
        /// <summary>
        /// 平台检查模式
        /// </summary>
        /// <exception cref="Exception">平台支持性错误</exception>
        protected void PreOSFileCheck()
        {
            Platformid = Environment.OSVersion.Platform;
            if (Platformid == PlatformID.Unix)
            {
                if (!File.Exists("./encoder") || !File.Exists("./decoder") || !File.Exists("./ffmpeg"))
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
                        Logs("[PreOSFileCheck - Linux] :: Do not Use Produce One File Mode");
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
                        Logs("[PreOSFileCheck - Windows] :: Do not Use Produce One File Mode");
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
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="s">日志字符串</param>
        protected void Logs(string s)
        {
            if (Log)
            {
                Console.WriteLine($"{s}");
            }
        }
    }

    /// <summary>
    /// Silk编码器
    /// </summary>
    public sealed class Encoder : SilkConvertor, IEncodeable
    {
        /// <summary>
        /// 音频比特率(最高96k)
        /// </summary>
        public int KBitRate { get; } = 96;
        /// <summary>
        /// 单个组包长度(默认20ms)
        /// </summary>
        public int PacketLength { get; } = 20;

        /// <summary>
        /// 构造编码器<br/>如果您不知道参数是什么,请不要修改
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

            FFmpegFileName = "ffmpeg";
            ConvertorFileName = "./encoder";
            if (Platformid == PlatformID.Win32NT)
            {
                FFmpegFileName = "ffmpeg.exe";
                ConvertorFileName = "./encoder.exe";
            }
            else
            {
                if (Platformid != PlatformID.Unix)
                {
                    throw new Exception("System Type Not Supported");
                }
            }
        }

        /// <inheritdoc/>
        public Task<SilkReturn> Encode(string filePath)
        {
            return Task<SilkReturn>.Factory.StartNew(() =>
            {
                try
                {
                    var uuid = Guid.NewGuid().ToString().Replace("-","");

                    {
                        Logs($"[FFMPEG] CONVERTING {filePath}");
                        using SilkUtilProcess p = new(FFmpegFileName, $"-hide_banner -i {filePath} -f s16le -b:a {KBitRate * 1000} -ar {SampRate} -ac 1 ./temp/{uuid}.pcm -y -loglevel panic");
                        p.WaitAndExit();
                    }
                    
                    {
                        Logs($"[ENCODE] CONVERTING {filePath}");
                        using SilkUtilProcess p = new(ConvertorFileName, $"./temp/{uuid}.pcm ./temp/{uuid}.silk -packetlength {PacketLength} -rate {KBitRate * 1000} -Fs_API {SampRate} -Fs_maxInternal {SampRate} -tencent -quiet");
                        p.WaitAndExit();
                    }

                    var pk = File.ReadAllBytes($"./temp/{uuid}.silk");
                    File.Delete($"./temp/{uuid}.pcm");
                    File.Delete($"./temp/{uuid}.silk");
                    Logs($"[ENCODE] COMPLETE {filePath} -> silkV3");
                    return new(pk, Path.GetFileNameWithoutExtension(filePath), FileExtension.silk);
                }
                catch
                {
                    throw;
                }
            });
        }
    }

    
    /// <summary>
    /// Silk解码器
    /// </summary>
    public sealed class Decoder : SilkConvertor, IDecodeable
    {
        /// <summary>
        /// 要转换到的格式
        /// </summary>
        public FileExtension Extension { get; }

        /// <summary>
        /// 构造解码器<br/>如果您不知道参数是什么,请不要修改
        /// </summary>
        /// <param name="ext">要转换到的格式</param>
        /// <param name="sampRate">音频采样率(最高24000)</param>
        /// <param name="log">是否记录转换日志</param>
        public Decoder(FileExtension ext, int sampRate = 24000, bool log = false)
        {
            Extension = ext;
            SampRate = sampRate;
            if (sampRate > 24000)
            {
                throw new Exception("Max SampRate is 24000");
            }
            Log = log;
            PreOSFileCheck();//检测系统模式

            FFmpegFileName = "ffmpeg";
            ConvertorFileName = "./decoder";
            if (Platformid == PlatformID.Win32NT)
            {
                FFmpegFileName = "ffmpeg.exe";
                ConvertorFileName = "./decoder.exe";
            }
            else
            {
                if (Platformid != PlatformID.Unix)
                {
                    throw new Exception("System Type Not Supported");
                }
            }
        }

        /// <inheritdoc/>
        public Task<SilkReturn> Decode(string filePath)
        {
            return Task<SilkReturn>.Factory.StartNew(() =>
            {
                var uuid = Guid.NewGuid().ToString().Replace("-","");
                Logs($"[DECODE] CONVERTING {filePath}");
                using var cv = new SilkUtilProcess(ConvertorFileName, $"{filePath} ./temp/{uuid}.pcm -Fs_API {SampRate} -quiet");
                cv.WaitAndExit();
                //ffmpeg -> (linux)

                Logs($"[FFMPEG] CONVERTING {filePath}");
                //判定文件类型
                switch (Extension)
                {

                    case FileExtension.mp3:
                        {
                            var str = $"-hide_banner -y -f s16le " +
                                    $"-ar {SampRate} -acodec pcm_s16le -ac 1 " +
                                    $"-i {$"./temp/{uuid}.pcm"} {$"./temp/{uuid}.{Extension}"} -loglevel panic";
                            using var p = new SilkUtilProcess(FFmpegFileName, str);
                            p.WaitAndExit();
                        };
                        break;
                    case FileExtension.wav:
                        {
                            var str = $"-hide_banner -y -f s16le " +
                                    $"-ar {SampRate} -v 16 -ac 1 " +
                                    $"-i {$"./temp/{uuid}.pcm"} {$"./temp/{uuid}.{Extension}"} -loglevel panic";
                            using var p = new SilkUtilProcess(FFmpegFileName, str);
                            p.WaitAndExit();
                        };
                        break;
                    case FileExtension.silk: throw new Exception("Do not Supported ReConvert Extension");
                    default: throw new Exception("Do not Supported this Extension");
                }
                //获取文件Byte
                var pk = File.ReadAllBytes($"./temp/{uuid}.{Extension}");
                //删除临时文件
                File.Delete($"./temp/{uuid}.pcm");
                File.Delete($"./temp/{uuid}.{Extension}");
                //生成新文件
                Logs($"[DECODE] COMPLETE {filePath} -> {Extension}");
                return new(pk, Path.GetFileNameWithoutExtension(filePath), Extension);
            });
        }
    }
}