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
    "";

string st = "" +
    "*n1\n" +
    "n1>n2\n" +
       "n2>n6\n" +
          "n6>n3\n" +
    "n1>n5\n" +
       "n5>n7\n" +
       "n5>n8\n" +
       "n5>n9\n" +
    "";

Tree<string>? gt = TreeUtil.ReadMappedTree(st.Split("\n"));
Console.WriteLine(gt);
//Console.WriteLine(gt);

//BGraph<string> g = GraphUtil.ReadMappedNode(s.Split("\n"));
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
