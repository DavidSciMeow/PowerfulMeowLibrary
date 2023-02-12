# Meow.FlightRadar
## 基于 FlightAware 的航空搜索系统 (API+)
![](https://img.shields.io/nuget/dt/Electronicute.Meow.FlightRadar)
![](https://img.shields.io/nuget/vpre/Electronicute.Meow.FlightRadar?label=NuGet%20Version)
[![MeowFlightRadar](https://github.com/DavidSciMeow/PowerfulMeowLibrary/actions/workflows/FlightRadar.yml/badge.svg?branch=master)](https://github.com/DavidSciMeow/PowerfulMeowLibrary/actions/workflows/FlightRadar.yml)

# 0. 引言
本库为航空基础功能库,含有`航司获取`,`航班查询`,`航班航路查询`,`机场查询`等功能
```
@copy 
{ 
    @FlightAware 
    (仅用于学习研究之用, 获取更加详细实时信息请联系官方)
    LittleSciMeow (Electronicute) 
    2022 dec 27.
}
```

---

## 目录

---

1. 程序大致逻辑
1. 程序内函数和内类列表
1. 一般性(控制台应用程序)的提前实现
1. 复用代码
1. 全局Q&A

---

# 1. 程序大致逻辑

## 1.1 底
### 1.1.1 本程序的逻辑底层
本程序通过使用`HTTPClient`类的Get方法和`HTMLAgilityPack`提供的Load方法,   
通过执行页面逻辑获取网页, 按照网页结构按位解析数据.
### 1.1.2 程序的URL底
参阅程序源码`UrlMapping.cs`文件

## 1.2 上层查询逻辑
程序通过构造的HTTP请求, 发送至FlightAware的站点,   
获取公共的HTML页面, 并且按位解析数据.

## 1.3 客户端/(SE)调用方逻辑
通过对于底的基础逻辑衍生的结构实现,   
含有正常的接口, 外延函数等, 参阅`内函数和内类列表`

# 2. 程序内函数和内类列表

## 2.1 程序内类函数
程序的解析类函数均在命名空间 `Meow.FlightRadar.SBase`,
且均为`静态方法`.
### 2.1.1 前置获取函数
1. LiveAirportDoc(string) 用于获得默认的机场描述文档
1. LiveAirportWeatherDoc(string) 用于获得机场的天气描述文档
1. LiveAirportFBODoc(string) 用于获得机场的管理人[FBO]信息
1. LiveFlightDoc(string) 用于获取一个已经在编的飞机的详细信息
1. LiveAllFleetDoc() 获取所有航司的信息
1. LiveFleetDoc(string) 获取某个航司的信息
### 2.1.2 文档解析函数
1. DeterminAirPortExist(LAD.*doc) 判定机场是否存在
1. DeterminAirportSecondaryLoc(LAD.*doc) 判定机场是否运营在第二飞行管理区
1. DeterminAirportLiveWeather(LAWD.*doc) 判定机场是否支持天气信息
1. DeterminFBOExist(LAFD.*doc) 判定机场是否含有可提取的运营人的信息
### 2.1.3 文档解析模型函数
1. GetFBOs(LAFD.*doc) 获取机场的FBO信息
1. GetBoard(LAD.*doc, BoardType) 获取航班进出港情况
1. GetAllBoard(LAD.*doc) 获取所有航班进出港情况
------
1. GetAirportWeather(LAD.*doc) 获取机场天气(ATIS)
1. GetLiveAirportWeather(LAD.*doc) 获取实时机场天气(通波字符串)
1. GetAirportWeatherMsg(LAD.*doc) 获取机场天气(通波字符串)
------
1. GetFlightTrack(LFD.*doc) 获取飞机实时信息
------
1. GetAllFleet(LFD.*doc) 获取所有航司信息

# 3. 一般性(控制台应用程序)的提前实现

## 3.1 ConsoleStaticUtil.cs
您可以通过使用`Meow.FlightRadar.CUtil.*`类内的函数来完成一般控制台的查询操作.  
CUtil类实现了大部分已经包装的查询操作, 如下:

|方法签名|用途|
|----|----|
|ConsoleEmbedded| 完整的控制台嵌套程序 |
|ConsoleGetAirportWeather|获得机场天气(表格)|
|ConsoleGetLiveAirportBoard|获得机场的进出港安排表|
|ConsoleGetAirportWeatherMsg|获得机场的ATIS通波字符串`(如果支持)`|
|ConsoleSearchAirport|搜索某个机场|
|ConsoleSearchAirline|搜索某个航班|
|UpdateAllConcurrentData|更新缓存数据|
|ConsoleSearchFleet|搜索某个航司|

# 4. 复用代码

## 4.1 

# 5. 全局Q&A

## 5.1 什么是 FlightAware 机场等级
根据 Flightaware 的官方分级, 机场分为第一管制区和第二管制区,   
由于其飞机数据是用户自己的基站获取的, 导致有些数据并不实时,    
在搜索到的机场内可以判断其是否位于第一管制区, 相较于其中的飞机,   
第二管制区的飞机不会直接朝 FlightAware 上报其飞机信息等,  
目前国内航班暂未有第一管制区功能, 大部分均位于第二管制区,   
关于国内航班和国内信息保护规定, 详见CAAC官网.  

## 5.2 能搜到什么样的"航班"
根据民航规定, 任何在空的飞机均需要朝最少一个塔台(机场)报备信息,     
且至少拥有一个A类型应答机, 一般民航还会装有C/S型号,   
所以当民航以ADS-B形式发送信息时, 其信息将会被有些用户的基站捕捉,   
信号解码后将上传FlightAware, 成为我们处理的信息.  
然而军方应答机未必常开, 也未必在我们民航监听的频段,   
但如果在此频段被捕捉, 则可以被搜索.

## 5.3 