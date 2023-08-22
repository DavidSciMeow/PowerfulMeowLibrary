using System;
using Meow.Util.Math.Graph;

string s = "" +
    "n1-n2\n" +
    "n1-n5\n" +
    "n2-n6\n" +
    "n6-n3\n" +
    "n6-n7\n" +
    "n3-n7\n" +
    "n7-n4\n" +
    "n3-n4\n" +
    "n7-n8\n" +
    "n8-n4\n" +
    "";

BGraph<string> g = GraphUtil.ReadMappedNode(s.Split("\n"));
//Console.WriteLine(g.PrintEdge());
Console.WriteLine("--BFS--");
var q = g.BFS("n1");
Console.WriteLine(q);
foreach (var i in q.Value)
{
    Console.WriteLine($"{i}");
}
Console.WriteLine("--DFS--");
var qx = g.DFS("n1");
Console.WriteLine(qx);
foreach (var i in qx.Value)
{
    Console.WriteLine($"{i}");
}
