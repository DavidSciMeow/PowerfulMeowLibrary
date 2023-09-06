using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Meow.Math.Graph
{
    /// <summary>
    /// 创建图工具
    /// </summary>
    public static class GraphUtil
    {
        /// <summary>
        /// 读制图行
        /// </summary>
        /// <param name="Seplines"></param>
        /// <returns>生成的图</returns>
        public static BGraph<string> ReadMappedNode(string[] Seplines)
        {
            BGraph<string> s = new();
            foreach (var i in Seplines)
            {
                var dk = i.Split(':');
                var sx = i.Contains('-') ? dk[0].Split('-') : dk[0].Split('>');
                if(sx.Length > 1)
                {
                    var start = sx[0].Trim();
                    var end = sx[1].Trim();
                    if (!s.ExistNodeById(start)) s.Insert(start);
                    if (!s.ExistNodeById(end)) s.Insert(end);
                    if (int.TryParse(dk.Length > 1 ? dk[1].Trim() : "1", out var weig))
                    {
                        if (i.Contains('>'))
                        {
                            s.LinkTo(start, end, weig);
                        }
                        else
                        {
                            s.Link(start, end, weig);
                        }
                    }
                }
            }
            return s;
        }
        /// <summary>
        /// 以sepnode模式读制图行
        /// </summary>
        /// <param name="path"></param>
        /// <param name="sepnode"></param>
        /// <returns></returns>
        public static BGraph<string> ReadMappedNode(string path, string sepnode = "\n") => ReadMappedNode(File.ReadAllText(path).Split(sepnode));
        /// <summary>
        /// 动态作图
        /// </summary>
        /// <returns>生成的图</returns>
        public static BGraph<string> InteractiveCreate()
        {
            BGraph<string> bG = new();
            Console.WriteLine("每行输入节点名, 输入$停止");
            while (true)
            {
                var cl = Console.ReadLine();
                if (cl == "$") break;
                if (string.IsNullOrEmpty(cl))
                {
                    Console.WriteLine("字符串名称不合法");
                }
                else
                {
                    if (bG.Insert(cl.Trim()))
                    {
                        Console.WriteLine($"节点{cl}已添加");
                    }
                    else
                    {
                        Console.WriteLine($"节点{cl}重复, 未更新");
                    }
                }
            }
            Console.WriteLine("每行输入节点关系, 输入!停止");
            while (true)
            {
                var cl = Console.ReadLine();
                if (cl == "!") break;
                bool direction = false;
                if (cl?.Contains('>') ?? false)
                {
                    cl = cl.Replace(">", "");
                    direction = true; 
                }
                if (cl?.Contains('-') ?? false)
                {
                    var tdx = cl.Split('-');
                    if(tdx.Length > 1)
                    {
                        if (cl.Contains(':'))//带权
                        {
                            var kdx = tdx[1].Split(':');
                            if(bG.ExistNodeById(tdx[0]) && bG.ExistNodeById(kdx[0]))
                            {
                                if(int.TryParse(kdx[1], out var i))
                                {
                                    if (direction)
                                    {
                                        bG.LinkTo(tdx[0], kdx[0], i);
                                        Console.WriteLine($"有向路径 {tdx[0]}-[{i}]->{kdx[0]} 已连接");
                                    }
                                    else
                                    {
                                        bG.Link(tdx[0], kdx[0], i);
                                        Console.WriteLine($"无向路径 {tdx[0]}-[{i}]-{kdx[0]} 已连接");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("路径权重不是数字");
                                }
                            }
                            else
                            {
                                Console.WriteLine("路径节点不存在");
                            }
                        }
                        else
                        {
                            if (bG.ExistNodeById(tdx[0]) && bG.ExistNodeById(tdx[1]))
                            {
                                if (direction)
                                {
                                    bG.LinkTo(tdx[0], tdx[1]);
                                    Console.WriteLine($"有向路径 {tdx[0]}-[1]->{tdx[1]} 已连接");
                                }
                                else
                                {
                                    bG.Link(tdx[0], tdx[1]);
                                    Console.WriteLine($"无向路径 {tdx[0]}-[1]-{tdx[1]} 已连接");
                                }
                            }
                            else
                            {
                                Console.WriteLine("路径节点不存在");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"节点关系{cl}不合法");
                    }
                }
                else
                {
                    Console.WriteLine($"节点关系{cl}不合法");
                }
            }
            Console.WriteLine("路径已录入, 图已保存");
            return bG;
        }
    }
}


