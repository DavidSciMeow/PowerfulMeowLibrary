using Meow.Util.Math.Graph;

BGraph<string> bG = new();
bG.Insert("n1", "n2", "n3", "n4", "n5", "n6", "n7");
bG.Link("n1", "n2", 1);
bG.Link("n1", "n3", 1);
bG.Link("n2", "n3", 1);
bG.Link("n2", "n4", 1);
bG.Link("n2", "n6", 1);
bG.Link("n6", "n4", 1);
bG.Link("n4", "n3", 1);
bG.Link("n3", "n5", 1);
bG.Link("n4", "n5", 1);
bG.Link("n4", "n7", 1);

bG.InteractiveTrack("n7", "n1");


