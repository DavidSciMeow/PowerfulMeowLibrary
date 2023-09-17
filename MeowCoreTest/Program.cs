using Meow.Math.Graph;
using Meow.Math.Graph.Struct;
using System;

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
    "n111-n112\n" +
    "";

string st = "" +
    "*n1\n" +
    "n1>n2\n" +
       "n2>n3\n" +
          "n3>n4\n" +
             "n4>n5\n" +
                "n5>n6\n" +
    "n1>n122\n" +
       "n122>n21\n" +
       "n122>n22\n" +
       "n122>n23\n" +
            "n23>n31\n" +
                "n31>n141\n" +
            "n23>n32\n" +
    "n1>n123\n" +
    "";

//Tree<string>? gt = Util.ReadMappedTree(st.Split("\n"));
//Console.WriteLine(gt);
//gt?.BFS();

Graph<string> g = Util.ReadMap(s.Split("\n"));
var sx = g.Dijkstra("n1", "n111");
//var l = File.ReadAllLines("./a");
//Graph<string> g = Util.ReadMap(l);
//var sx = g.Dijkstra("台东", "中山公园");
int i = 0;
foreach(var x in sx)
{
    Console.Write($"{x}\t {(i++ % 10 == 0 ? "\n" : "")}");
}
Console.WriteLine();


//while (true)
//{
//    Console.Write("StepInto:");
//    var sk = Console.ReadLine();
//    Console.WriteLine();
//    if (sk != null) Step(sk, q);
//    Console.Write("PressAnyToContinue:");
//    Console.ReadLine();
//    Console.Clear();
//}
