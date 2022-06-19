# 中央气象台天气获取组件

# 0 导 言
本组件包已经上传nuget, 请关注 nuget Electronicute.Weather 包  
使用 Meow.Weather.CN.Interpreter.RectifyWord(string)   
`string` 为下面`1.2`内某个"指令";   
使用 Meow.Weather.CN.Interpreter.RectifyWord(string\[\])   
列表顺序为 \[ 省 , 市 \] 函数来获取此位置的天气详细信息;

# 1.1 简易使用方法

```csharp
public static void Main(string[] args)
{
    while (true)
    {
        //获取一般图像类(检测类)
        var k = Meow.Weather.CN.Interpreter.RectifyWord(Console.ReadLine());//尝试输入 nf4i
        Console.WriteLine(k.Data[0].ImgUrl);//得到图片地址
    }
}
```
```csharp
//获取城市天气
....
var prov = input[1];
var city = input.Length > 2 ? input[2] : null;
JObject jo;
if (city != null)
{
    jo = Meow.Weather.CN.Interpreter.RectifyWord(new string[] { prov, city }).GetAwaiter().GetResult();
}
else
{
    jo = Meow.Weather.CN.Interpreter.RectifyWord(new string[] { prov }).GetAwaiter().GetResult();
}
var stationname = jo?["basic"]?["station"]?["city"]?.ToString();
var publishtime = jo?["basic"]?["publish_time"]?.ToString();
var weather_temp = jo?["basic"]?["weather"]?["temperature"]?.ToString();
var weather_tempdiff = jo?["basic"]?["weather"]?["temperatureDiff"]?.ToString();
var airpressureHpa = jo?["basic"]?["weather"]?["airpressure"]?.ToString();
var humidity = jo?["basic"]?["weather"]?["humidity"]?.ToString();
var rainpred = jo?["basic"]?["weather"]?["rain"]?.ToString();
var weainfo = jo?["basic"]?["weather"]?["info"]?.ToString();
var wind_dir = jo?["basic"]?["wind"]?["direct"]?.ToString();
var wind_pow = jo?["basic"]?["wind"]?["power"]?.ToString();
var warn = jo?["basic"]?["warn"]?["alert"]?.ToString();
var aqi = jo?["aqi"]?["aqi"]?.ToString();
var aqitext = jo?["aqi"]?["text"]?.ToString();
var aqipredtime = jo?["aqi"]?["forecasttime"]?.ToString();
var finstr = $"气象单站:{stationname},\n" +
    $"气象发布时间:{publishtime}\n" +
    $"当前天气:{(weainfo == "9999" ? "/" : weainfo)},降水概率:{(rainpred == "9999" ? "/" : rainpred)}%\n" +
    $"当前温度:{(weather_temp == "9999" ? "/" : weather_temp)},可能的误差:{(weather_tempdiff == "9999" ? "/" : weather_tempdiff)}\n" +
    $"当前气压:{(airpressureHpa == "9999" ? "/" : $"{airpressureHpa}Hpa")}, 湿度:{(humidity == "9999" ? "/" : humidity)}%\n" +
    $"风:{wind_dir}{wind_pow}\n" +
    $"空气质量:{aqi}[{aqitext}] @ {aqipredtime}\n" +
    $"中央气象局提醒:\n{(warn == "9999" ? "/" : warn)}";
Console.WriteLine(finstr);
....
```
# 1.2 功能一览

| 功能                 | 指令      |
|--------------------|---------|
| 基本天气分析std.hpa中国    | cdwh0   |
| 基本天气分析925hpa中国     | cdwh9   |
| 基本天气分析850hpa中国     | cdwh8   |
| 基本天气分析700hpa中国     | cdwh7   |
| 基本天气分析500hpa中国     | cdwh5   |
| 基本天气分析200hpa中国     | cdwh2   |
| 基本天气分析100hpa中国     | cdwh1   |
| 基本天气分析std.hpa亚欧    | adwh0   |
| 基本天气分析925hpa亚欧     | adwh0   |
| 基本天气分析850hpa亚欧     | adwh8   |
| 基本天气分析700hpa亚欧     | adwh7   |
| 基本天气分析500hpa亚欧     | adwh5   |
| 基本天气分析200hpa亚欧     | adwh2   |
| 基本天气分析100hpa亚欧     | adwh1   |
| 基本天气分析std.hpa北半球   | ndwh0   |
| 基本天气分析925hpa北半球    | ndwh9   |
| 基本天气分析850hpa北半球    | ndwh8   |
| 基本天气分析700hpa北半球    | ndwh7   |
| 基本天气分析500hpa北半球    | ndwh5   |
| 基本天气分析200hpa北半球    | ndwh2   |
| 基本天气分析100hpa北半球    | ndwh1   |
| 叠加卫星云图std.hpa中国    | cdrh0   |
| 叠加卫星云图925hpa中国     | cdrh9   |
| 叠加卫星云图850hpa中国     | cdrh8   |
| 叠加卫星云图700hpa中国     | cdrh7   |
| 叠加卫星云图500hpa中国     | cdrh5   |
| 叠加卫星云图200hpa中国     | cdrh2   |
| 叠加卫星云图100hpa中国     | cdrh1   |
| 叠加卫星云图stdhpa亚欧     | adch0   |
| 叠加卫星云图925hpa亚欧     | adch9   |
| 叠加卫星云图850hpa亚欧     | adch8   |
| 叠加卫星云图700hpa亚欧     | adch7   |
| 叠加卫星云图500hpa亚欧     | adch5   |
| 叠加卫星云图200hpa亚欧     | adch2   |
| 叠加卫星云图100hpa亚欧     | adch1   |
| 近1小时降水量            | r1hp    |
| 近6小时降水量            | r6hp    |
| 近24小时降水量           | r24hp   |
| 近10天降水量            | r10dp   |
| 近20天降水量            | r20dp   |
| 近30天降水量            | r30dp   |
| 近10天降水量距平          | r10pa   |
| 近20天降水量距平          | r20pa   |
| 近30天降水量距平          | r30pa   |
| 全国逐时气温             | t1hp    |
| 全国逐日气温平均           | tdta    |
| 全国逐日气温最高           | tdtmx   |
| 全国逐日气温最低           | tdtmn   |
| 近10天最高气温           | th10d   |
| 近20天最高气温           | th20d   |
| 近30天最高气温           | th30d   |
| 近10天最低气温           | tl10d   |
| 近20天最低气温           | tl20d   |
| 近30天最低气温           | tl30d   |
| 近10天距平             | tm10d   |
| 近20天距平             | tm20d   |
| 近30天距平             | tm30d   |
| 逐小时极大风速            | owd     |
| 地闪(雷电)             | oltng   |
| 短时强降雨              | ohvr    |
| 雷暴大风               | og      |
| 冰雹                 | hail    |
| 风云4A真彩色            | sf4tc   |
| 风云4A红外             | sf4i    |
| 风云4A可见光            | sf4v    |
| 风云4A水汽             | sf4wv   |
| 风云2G可见光            | sf2v    |
| 风云2G增强水汽           | sf2wv   |
| 风云2G增强红外一          | sf2i1   |
| 风云2G增强红外二          | sf2i2   |
| 风云2G黑白可见光          | sf2bvl  |
| 风云2G黑白中红外          | sf2bwmi |
| 风云2G圆盘彩色           | sf2dc   |
| 风云2G圆盘红外一          | sf2di1  |
| 风云2G圆盘红外二          | sf2di2  |
| 风云2G圆盘中红外          | sf2di3  |
| 风云2G圆盘水汽           | sf2dwv  |
| 风云2G圆盘可见光          | sf2dv   |
| 全国                 | rdch    |
| 华北                 | rdhb    |
| 东北                 | rddb    |
| 华东                 | rdhd    |
| 华中                 | rdhz    |
| 华南                 | rdhn    |
| 西南                 | rdxn    |
| 西北                 | rdxb    |
| 能见度                | spf     |
| 雾预报                | efog    |
| 霾预报                | ehaze   |
| 沙尘天气               | edust   |
| 污染气象条件 - 24小时      | eap24   |
| 污染气象条件 - 48小时      | eap48   |
| 污染气象条件 - 72小时      | eap72   |
| 土壤水分10cm           | asmm10  |
| 土壤水分20cm           | asmm20  |
| 土壤水分30cm           | asmm30  |
| 土壤水分40cm           | asmm40  |
| 土壤水分50cm           | asmm50  |
| 农业干旱检测             | adm     |
| 大豆发育期              | aib     |
| 油菜发育期              | aio     |
| 牧草发育期              | aipa    |
| 马铃薯发育期             | aipo    |
| 夏玉米发育期             | aisuc   |
| 冬小麦发育期             | aiww    |
| 春小麦发育期             | aisw    |
| 棉花发育期              | aic     |
| 花生发育期              | aip     |
| 早稻发育期              | aier    |
| 晚稻发育期              | ailr    |
| 春玉米发育期             | aispc   |
| 一季稻发育期             | airq    |
| 北半球500hpa          | nn5hh   |
| 北半球850hpa          | nn8ht   |
| 北半球海平面气压           | nnslp   |
| 亚欧500高800风         | na5     |
| 东亚海平面气压            | neslp   |
| 东亚500高800风         | ne5h8w  |
| 东亚24小时降水           | ne24hr  |
| 东亚积累降水             | necp    |
| 东亚2m温度             | nt2m    |
| 东亚2m最高温度           | nt2mmx  |
| 东亚2m最低温度           | nt2mmn  |
| 东亚2m相对湿度           | nt2mrh  |
| 东亚雷达组合反射率          | neldfsl |
| FY4A红外亮温           | nf4i    |
| FY4A水汽亮温           | nf4wv   |
| 累积降水24h概率预报1mm     | ngsr1   |
| 累积降水24h概率预报10mm    | ngsr10  |
| 累积降水24h概率预报25mm    | ngsr25  |
| 累积降水24h概率预报50mm    | ngsr50  |
| 累积降水24h概率预报100mm   | ngsr100 |
| 累积降水24h极端预报指数      | ngsrjd  |
| 累积降水24h集合平均和离散度    | ngsra   |
| 累积降水24h邮票图         | ngsryp  |
| 累积降水24h分位数最大值      | ngsrmx  |
| 位势高度500hPa面条图      | ngsmt   |
| 位势高度500hPa集合平均和离散度 | ngsa    |
| 温度2米极端预报指数         | ngst2mf |
| 温度2米集合平均和离散度       | ngst2ma |
| 全风速10米极端预报指数       | ngs10jd |
| 全风速10米集合平均和离散度     | ngs10a  |
| 全风速10米概率预报10.8m/s  | ngsp10  |
| 全风速10米概率预报17.2m/s  | ngsp17  |
| 台风强度               | nsci    |
| 累计降水24h            | nsrain  |
| 累计大风24h            | nswind  |
| 台风路径               | nsrt    |
| 风场10m              | nsw10m  |
| 组合雷达反射率            | nsrdr   |
| FY4A云图模拟红外亮温       | nsf4i   |
| FY4A云图模拟水汽亮温       | nsf4wv  |
| 全球浪高浪向叠加图          | ogbw    |
| 大西洋浪高浪向叠加图         | oatlw   |
| 印度洋浪高浪向叠加图         | oindw   |
| 太平洋浪高浪向叠加图         | opacw   |
