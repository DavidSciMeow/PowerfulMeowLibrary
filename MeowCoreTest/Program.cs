using Meow.Math.Graph;
using Meow.Math.Graph.Struct;
using System;
using System.Threading.Tasks;

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
       "n2>n3\n" +
          "n3>n4\n" +
             "n4>n5\n" +
                "n5>n6\n" +
    "n1>n111\n" +
    "n1>n1112\n" +
    "";
Tree<string>? gt = Util.ReadMappedTree(st.Split("\n"));
Console.WriteLine(gt);

//Console.WriteLine();

//BGraph<string> g = GraphUtil.ReadMappedNode(s.Split("\n"));
//Console.WriteLine(g);



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
