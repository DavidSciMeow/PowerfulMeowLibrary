using System;

namespace WeatherTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var k = new Meow.Weather.CN.主要功能.GRAPES台风预报.台风路径();
            Console.WriteLine(k.Data[0].ImgUrl);
        }
    }
}
