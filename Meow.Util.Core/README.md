# MeowUtil
## 本仓库为Meow.Util的详细分包便于订阅使用.
-----

# 0 引言
本库目前分为四个分包,含有`网络处理`,`解密加密,时间处理,二进制处理`,`图像处理`,`进程处理`

## `进程处理库还在编写中`

# 1 内函数一览
## 函数参数请在VisualStudio调用时查看

```CSharp
//标注 ext 的是静态扩展方法, 可以使用第一参数直接后接其他参数. 
//例如 DiscuzMd5(string)
var pwd = "abc".DiscuzMd5();
//标注 async 的是异步方法, 可以不带异步同步调用, 但也可以使用异步交换. 
var html = Client.MString("www.github.com").GetAwaiter().GetResult();
//or
Task.Factory.StartNew(
	async () => { 
		var html = await Meow.Util.Network.Http.Get.String("www.github.com"); 
	}
);
```
## 包 Electronicute.Util.Convert
|命名空间|类|成员名|作用|
|----|----|----|----|
|Meow.Util.Encrypt|Hash|`ext` DiscuzMd5(string)|用于特殊加密Discuz的MD5|
|Meow.Util.Encrypt|Hash|`ext` Md5(string)|加密Md5|
|Meow.Util.Encrypt|Hash|`ext` MD5S2ExpressPwd(string)|Md5Salt2加密方案|
|----|----|----|----|
|***Meow.Util.Bit***|***BitSource***|***BitSource(byte)/BitSource(bool[])/BitSource()***|***比特类帮助结构体***|
|Meow.Util.Bit|BitUtil|`ext` ToBit(byte)|转换成二进制帮助类|
|Meow.Util.Bit|BitUtil|`ext` SetBit(byte, int,bool)|设置Byte的某一位到某个状态|
|Meow.Util.Bit|BitUtil|`ext` SetBit(byte, bool[])|设置Byte的所有位|
|Meow.Util.Bit|BitUtil|`ext` GetBit(byte)|获取Byte的所有位|
|Meow.Util.Bit|BitUtil|`ext` GetBit(byte, int)|获取Byte的某一位|
|----|----|----|----|
|Meow.Util|Time|`ext` Second(long)|秒制时间戳转换时间类|
|Meow.Util|Time|`ext` MilliSecond(long)|毫秒制时间戳转换时间类|
|Meow.Util|Time|`ext` Ticks(long)|Ticks转换时间类|
|Meow.Util|Time|`ext` ToSecTimeStamp(DateTime)|时间类转换成秒制时间戳|
|Meow.Util|Time|`ext` ToMiSecTimeStamp(DateTime)|时间类转换成毫秒制时间戳|

## 包 Electronicute.Util.Network
|命名空间|类|成员名|作用|
|----|----|----|----|
|***Meow.Util.Network***|***BinMsg\<T\>***|***`static` BinMsg***|***比特信息报文组织包***|
|----|----|----|----|
|Meow.Util.Network.Http|HttpUtil|`ext` `async` MString(string)| 获取某URL的字符串(通常小于83kb)|
|Meow.Util.Network.Http|HttpUtil|`ext` `async` MBlock(string)| 获取某URL的一个块(可以是gzip压缩)|
|Meow.Util.Network.Http|HttpUtil|`ext` MFile(string,string)| 获取某个URL的一个文件(当作文件下载)|
|Meow.Util.Network.Http|HttpUtil|`ext` `async` MPost(....)| 朝某个URL进行一次POST|

## 包 Electronicute.Util.Imaging
|命名空间|类|成员名|作用|
|----|----|----|----|
|Meow.Util.Imaging|Skia|FileToBase64(string)| 将文件转换成Base64格式|
|Meow.Util.Imaging|Skia|Read(string)| 读取一个文件|
|Meow.Util.Imaging|Skia|`ext` Save(SKBitMap,string,Format,int)| 保存一个文件|
|Meow.Util.Imaging|Skia|`ext` ToBase64String(SKBitMap)| 转换一个SKbitmap到Base64|
|Meow.Util.Imaging|Skia|`ext` Base64ToSKBitmap(string)| 转换一个base64编码字符串到SKbitmap|
|Meow.Util.Imaging|Skia|`ext` ToBase64String(SKImage)| 转换一个SKImage到Base64|

## 包 Electronicute.Util.Proc
|命名空间|类|成员名|作用|
|***Meow.Util.Proc***|***ProcBase***|***`Abstract` ProcBase***|***程序组基类***|
|***Meow.Util.Proc***|***Linux***|***Linux(string, DataReceivedEventHandler, EventHandler, string)***|***创建Linux进程监视组***|

# 2 更新计划
1. 网络图片下载解析  
1. 标准流Skia图片绘制  
1. 解密加密库  
1. 其他协议网络收发报文  
