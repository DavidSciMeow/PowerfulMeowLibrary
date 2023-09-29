using Meow.Math.Graph.Struct;

namespace Meow.Math.Graph
{
    /// <summary>
    /// 
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="seplines"></param>
        /// <returns></returns>
        public static Tree<string>? ReadMappedTree(string[] seplines)
        {
            Tree<string>? tree = null;
            bool _isRootDefine = false;
            foreach (string line in seplines)
            {
                if (line.Contains('*'))
                {
                    if (!_isRootDefine)
                    {
                        tree = new(line.Replace("*", ""));
                        _isRootDefine = true;
                    }
                    else
                    {
                        Console.WriteLine($"{line} err by only one tree root can be define");
                    }
                }
                else if (line.Contains('>'))
                {
                    if (!_isRootDefine)
                    {
                        Console.WriteLine($"No Root Node Define. >skip");
                    }
                    else
                    {
                        var spl = line.Split('>');
                        var fn = spl[0];
                        var sn = spl[1];
                        if (!string.IsNullOrEmpty(fn) && !string.IsNullOrEmpty(sn) && tree is Tree<string> st)
                        {
                            st.AddNode(sn, fn);
                        }
                        else
                        {
                            Console.WriteLine($"{line} have no splitable node");
                        }
                    }
                }
            }
            return tree;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="seplines"></param>
        /// <returns></returns>
        public static Graph<string> ReadMap(string[] seplines)
        {
            Graph<string> ms = new();
            foreach (string line in seplines)
            {
                int weight = 1;
                string node1;
                string node2;
                if (line.Contains('-'))
                {
                    var n1 = line.Split('-');
                    node1 = n1[0];

                    if (n1[1].Contains(':'))
                    {
                        var n2 = n1[1].Split(':');
                        if (int.TryParse(n2[1], out var _w))
                        {
                            node2 = n2[0];
                            weight = _w;
                        }
                        else
                        {
                            Console.WriteLine($"{n2[1]} is not a num.");
                            continue;
                        }
                    }
                    else
                    {
                        node2 = n1[1];
                    }

                    if (!ms.Exist(node1)) ms.Add(node1);
                    if (!ms.Exist(node2)) ms.Add(node2);
                    ms[node1].Add(node2, weight);
                    ms[node2].Add(node1, weight);
                }
                else if (line.Contains('>'))
                {
                    var n1 = line.Split('>');
                    node1 = n1[0];

                    if (n1[1].Contains(':'))
                    {
                        var n2 = n1[1].Split(':');
                        if (int.TryParse(n2[1], out var _w))
                        {
                            node2 = n2[0];
                            weight = _w;
                        }
                        else
                        {
                            Console.WriteLine($"{n2[1]} is not a num.");
                            continue;
                        }
                    }
                    else
                    {
                        node2 = n1[1];
                    }

                    if (!ms.Exist(node1)) ms.Add(node1);
                    if (!ms.Exist(node2)) ms.Add(node2);
                    ms[node1].Add(node2, weight);
                }
            }
            return ms;
        }
    }
}


