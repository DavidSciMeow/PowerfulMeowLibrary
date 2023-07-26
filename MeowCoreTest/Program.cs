using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Meow.Util.Math.Graph;

DMap<string> DS = new();
while (true)
{
    Console.WriteLine();
    Console.WriteLine("任意键查看说明 (任意键继续)");
    Console.WriteLine();
    var crl = Console.ReadLine();
    Console.Clear();
    if (crl?.StartsWith("/lf") ?? false)
    {
        DMap<string>.ReadFileMap(DS,crl[3..]);
    }
    else if (crl?.StartsWith("/fs") ?? false)
    {
        var cl = crl.Split(' ');
        if (cl.Length > 2)
        {
            Console.WriteLine(DS.FindShortest(cl[1], cl[2]));
        }
        else
        {
            Console.WriteLine("参数过少");
        }
    }
    else if (crl?.StartsWith("/link") ?? false)
    {
        var cl = crl.Split(' ');
        if (cl.Length > 2)
        {
            try
            {
                _ = DS.GetNode(cl[1] , out var nodestart);
                try
                {
                    _ = DS.GetNode(cl[2], out var nodeend);
                    if(cl.Length > 3)
                    {
                        if(int.TryParse(cl[3],out var x))
                        {
                            nodestart.InsertNode(nodeend, x);
                        }
                        else
                        {
                            Console.WriteLine($"权重 {x} 不是数字");
                        }
                    }
                    else
                    {
                        nodestart.InsertNode(nodeend, 1);
                    }
                }
                catch
                {
                    Console.WriteLine($"节点 {cl[2]} 不存在");
                }
            }
            catch
            {
                Console.WriteLine($"节点 {cl[1]} 不存在");
            }
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine("参数过少");
        }
    }
    else if (crl?.StartsWith("/del") ?? false)
    {
        var cl = crl.Split(' ');
        if (cl.Length > 2)
        {
            try
            {
                _ = DS.GetNode(cl[1],out var s);
                try
                {
                    _ = DS.GetNode(cl[2], out var e);
                    s.RemoveNode(e);
                }
                catch
                {
                    Console.WriteLine($"节点 {cl[2]} 不存在");
                }
            }
            catch
            {
                Console.WriteLine($"节点 {cl[1]} 不存在");
            }
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine("参数过少");
        }
    }
    else if (crl?.StartsWith("/alt") ?? false)
    {
        var cl = crl.Split(' ');
        if (cl.Length > 3)
        {
            try
            {
                _ = DS.GetNode(cl[1], out var s);
                try
                {
                    _ = DS.GetNode(cl[2], out var e);
                    if(int.TryParse(cl[3],out var x))
                    {
                        s.AltNode(e,x);
                    }
                    else
                    {
                        Console.WriteLine($"权重 {x} 不是数字");
                    }
                }
                catch
                {
                    Console.WriteLine($"节点 {cl[2]} 不存在");
                }
            }
            catch
            {
                Console.WriteLine($"节点 {cl[1]} 不存在");
            }
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine("参数过少");
        }
    }
    //else if (crl?.StartsWith("/save") ?? false)
    //{
    //    var cl = crl.Split(' ');
    //    if (cl.Length > 1)
    //    {
    //        using FileStream fs = new FileStream(Path.Combine(cl[1]), FileMode.Create);
    //        new BinaryFormatter().Serialize(fs, DS);
    //        Console.WriteLine($"保存完成:{cl[1]}");
    //    }
    //    else
    //    {
    //        Console.WriteLine("没有路径");
    //    }
    //}
    //else if (crl?.StartsWith("/load") ?? false)
    //{
    //    var cl = crl.Split(' ');
    //    if (cl.Length > 1)
    //    {
    //        using var fs = new FileStream(Path.Combine(cl[1]), FileMode.Open);
    //        if (new BinaryFormatter().Deserialize(fs) is Dictionary<string, SNode> p)
    //        {
    //            DS.nl = p;
    //            Console.WriteLine($"读取完成:{cl[1]}");
    //        }
    //        else
    //        {
    //            Console.WriteLine("格式不正确");
    //        }
    //    }
    //    else
    //    {
    //        Console.WriteLine("没有路径");
    //    }
    //}
    else
    {
        Console.WriteLine("---");
        Console.WriteLine("使用说明");
        Console.WriteLine("---");
        Console.WriteLine("/lf [path] 读取图文件");
        //Console.WriteLine("/save [path] 保存当前的图到图保存文件");
        //Console.WriteLine("/load [path] 保存当前的图到图保存文件");
        Console.WriteLine("---");
        Console.WriteLine("/fs [node1] [node2] 计算最小路径");
        Console.WriteLine("/del [node1] [node2] 将node1到node2的链接删除");
        Console.WriteLine("/alt [node1] [node2] [weight] 将node1到node2的链接, 更改权重为 weight");
        Console.WriteLine("/link [node1] [node2] 将node1链接到node2, 置权重为 1");
        Console.WriteLine("/link [node1] [node2] [weight] 将node1链接到node2, 置权重为 weight");
        Console.WriteLine("---");
    }
}




