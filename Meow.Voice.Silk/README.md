# Meow.Voice.Silk

![](https://img.shields.io/nuget/dt/Electronicute.Meow.Voice.Silk)
![](https://img.shields.io/nuget/vpre/Electronicute.Meow.Voice.Silk?label=NuGet%20Version)

## 功能表
| --          | 编码器 | 解码器 |
|-------------|-----|-----|
| Window.x64  <br/> ![](https://img.shields.io/nuget/dt/Electronicute.Meow.Voice.NativeAssets.Windows) | Voice.Encoder   |  正在开发  | 
| Linux.Amd64 <br/> ![](https://img.shields.io/nuget/dt/Electronicute.Meow.Voice.NativeAssets.Linux) | Voice.Encoder   |  正在开发  |
| Windows.x86 | 不支持  |  不支持  |
| Linux.i386  | 不支持  |  不支持  |

## 简易使用方法
1. nuget包 Electronicute.Voice.Silk
1. nuget包 Electronicute.Voice.NativeAssets. *系统版本*
1. 使用内构函数编码Silk文件(如下)
```Csharp
Meow.Voice.Silk.Encoder encoder = new(log:false);
var r = encoder.Encode("完全限定文件名").GetAwaiter().GetResult();
r.ConvertFile("完全限定文件目录"); //转换成文件
var a = r.ConvertTOBase64(); //转换成Base64
```

