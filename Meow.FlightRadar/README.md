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
    @FlightAware (仅用于学习研究之用, 正版获取信息请联系官方)
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