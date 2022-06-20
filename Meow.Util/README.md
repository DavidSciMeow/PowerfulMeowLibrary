# MeowUtil

-----

# 0 引言
本库为基础标准功能库,含有网络处理,解密加密,时间处理,图像处理等库

# 1 内函数一览

## 函数参数请在VisualStudio调用时查看

|命名空间|类|成员名|作用|
|-|-|-|
|Meow.Util.Dir|DirX|GetWebSiteDirs(string)|获取一个网页URL的完整后缀路径|
|----|----|----|----|
|Meow.Util.Encrypt|Hash|`ext` DiscuzMd5(string)|用于特殊加密Discuz的MD5|
|Meow.Util.Encrypt|Hash|`ext` Md5(string)|加密Md5|
|Meow.Util.Encrypt|Hash|`ext` MD5S2ExpressPwd(string)|Md5Salt2加密方案|
|----|----|----|----|
|Meow.Util|Time|`ext` Second(long)|秒制时间戳转换时间类|
|Meow.Util|Time|`ext` MilliSecond(long)|毫秒制时间戳转换时间类|
|Meow.Util|Time|`ext` Ticks(long)|Ticks转换时间类|
|Meow.Util|Time|`ext` ToSecTimeStamp(DateTime)|时间类转换成秒制时间戳|
|Meow.Util|Time|`ext` ToMiSecTimeStamp(DateTime)|时间类转换成毫秒制时间戳|
|----|----|----|----|
|Meow.Util.Network.Http|Client|(HttpClient) Basic| 基础的网络获取Client实例|
|Meow.Util.Network.Http|Client|(HttpClient) Compression| 接受压缩的网络获取Client实例|
|Meow.Util.Network.Http|Get|`async` String(string)| 获取某URL的字符串(通常小于83kb)|
|Meow.Util.Network.Http|Get|`async` Block(string)| 获取某URL的一个块(可以是gzip压缩)|
|Meow.Util.Network.Http|Get|File(string,string)| 获取某个URL的一个文件(当作文件下载)|
|Meow.Util.Network.Http|Post|`async` Create(....)| 朝某个URL进行一次POST|
