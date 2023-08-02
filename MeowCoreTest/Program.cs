using Meow.Util.Math.Graph;


string s = "" +
    "#n1\n" +
    "#n2\n" +
    "#n3\n" +
    "#n4\n" +
    "#n5\n" +
    "#n6\n" +
    "#n7\n" +
    "$\n" +
    "n1->n2:2\n" +
    "n1-n3\n" +
    "n2-n3\n" +
    "n2-n4\n" +
    "n2-n6\n" +
    "n6-n4\n" +
    "n4-n3\n" +
    "n3-n5\n" +
    "n4-n5\n" +
    "n4-n7\n" +
    "";

BGraph<string> bG = GraphUtil.ReadMappedStringNode(s.Split("\n"));
bG.InteractiveTrack("n7", "n1");


