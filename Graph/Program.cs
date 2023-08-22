using Meow.Util.Math.Graph;

BGraph<string>? bG = null;
Console.WriteLine("按下任意键查看帮助");
while (true)
{
    Console.Write("$ >");
    try
    {
        var crl = (Console.ReadLine() ?? "").Trim();
        Console.Clear();
        Console.WriteLine($"$ >{crl.Trim()}");
        var paralist = crl.Split(' ');
        switch (paralist[0])
        {
            case "/load":
                {
                    if (paralist.Length > 1)
                    {
                        bG = GraphUtil.ReadMappedNode(paralist[1]);
                    }
                    else
                    {
                        Console.WriteLine("路径不正确");
                    }
                }
                break; //载入
            case "/loadbin":
                {
                    if (paralist.Length > 1)
                    {
                        bG = GraphUtil.Read<string>(paralist[1]);
                    }
                    else
                    {
                        Console.WriteLine("路径不正确");
                    }
                }
                break; //载入二进制
            case "/save":
                {
                    if (bG == null)
                    {
                        Console.WriteLine("图未初始化");
                    }
                    else if (paralist.Length < 2)
                    {
                        Console.WriteLine("路径存在问题");
                    }
                    else if (paralist.Length > 1)
                    {
                        bG?.SaveAsMappedNode(paralist[1]);
                        Console.WriteLine($"保存完毕 路径为:{paralist[1]}");
                    }
                }
                break; //保存
            case "/savebin":
                {
                    if (bG == null)
                    {
                        Console.WriteLine("图未初始化");
                    }
                    else if (paralist.Length < 2)
                    {
                        Console.WriteLine("路径存在问题");
                    }
                    else if (paralist.Length > 1)
                    {
                        bG?.Save(paralist[1]);
                        Console.WriteLine($"保存完毕 路径为:{paralist[1]}");
                    }
                }
                break; //保存二进制
            case "/newi":
                {
                    bG = GraphUtil.InteractiveCreate();
                }
                break; //交互创造
            case "/from":
                {
                    if (!bG.HasValue) { Console.WriteLine("图未初始化"); break; }
                    if (paralist.Length < 2) { Console.WriteLine("参数过少"); break; }
                    if (!bG.Value.NodeList.ContainsKey(paralist[1].Trim())) { Console.WriteLine($"节点{paralist[1]}不存在"); break; }
                    bG.Value.InteractiveTrack(paralist[1]);
                }
                break; //单源点遍历
            case "/s":
                {
                    if (!bG.HasValue) { Console.WriteLine("图未初始化"); break; }
                    if (paralist.Length < 3) { Console.WriteLine("参数过少"); break; }
                    if (!bG.Value.NodeList.ContainsKey(paralist[1].Trim())) { Console.WriteLine($"节点{paralist[1]}不存在"); break; }
                    if (!bG.Value.NodeList.ContainsKey(paralist[2].Trim())) { Console.WriteLine($"节点{paralist[2]}不存在"); break; }
                    bG.Value.InteractiveTrack(paralist[1], paralist[2]);
                }
                break; //获取最短路径
            case "/gnl":
                {
                    if (!bG.HasValue) { Console.WriteLine("图未初始化"); break; }
                    Console.WriteLine($"图 [{bG.Value.GraphName}]");
                    Console.WriteLine($"节点名 [路径数]");
                    foreach (var i in bG.Value.NodeList)
                    {
                        Console.WriteLine($"{i.Key}[{i.Value.Count}]");
                    }
                }
                break; //获取全部节点数据
            default:
                {
                    Console.WriteLine("/load <文件名> \t 载入制图文件");
                    Console.WriteLine("/loadbin <文件名> \t 载入保存的二进制制图文件");
                    Console.WriteLine("/save <文件名> \t 保存图为制图文件");
                    Console.WriteLine("/savebin <文件名> \t 保存图为二进制制图文件");
                    Console.WriteLine("/newi \t 创建一个图, 使用交互创建模式");
                    Console.WriteLine("/gnl  \t 获取所有节点数据");
                    Console.WriteLine("/from <节点> \t 单源点遍历全图");
                    Console.WriteLine("/s <节点1> <节点2> \t 计算两个点的最短路径");
                }
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"E >>{ex.Message}");
    }
}