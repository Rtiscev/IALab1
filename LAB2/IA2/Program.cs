using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

var citiesIndex = new Dictionary<int, string>();

var something = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));
// Console.WriteLine(something);

string content;
using (StreamReader reader = new StreamReader($"{something}/map.csv"))
{
	content = reader.ReadToEnd();
}
var splitStr = content.Split('\n');

var cities = splitStr[0].Split(',');
int i = 0;
foreach (var country in cities)
{
	var removeSlash = country.Split("\r");
	citiesIndex.Add(i++, removeSlash[0]);
}

Graph graph = new Graph(citiesIndex.Count);

for (int j = 1; j < splitStr.Length; j++)
{
	var splitSecond = splitStr[j].Split(',');
	for (int k = 0; k < splitSecond.Length; k++)
	{
		if (int.Parse(splitSecond[k]) > 0)
		{
			graph.AddEdge(j - 1, k, int.Parse(splitSecond[k]));
		}
	}
}

// print all cities
int l = 0;
foreach (var item in graph.adjList)
{
	Console.Write($"{citiesIndex[l]}({l}):\t\t");
	foreach (var ggg in item)
	{
		Console.Write($"{citiesIndex[ggg.country]}({ggg.country}),");
	}
	Console.WriteLine();
	l++;
}

var startC = citiesIndex.FirstOrDefault(x => x.Value == "Mehedia").Key;
var endC = citiesIndex.FirstOrDefault(x => x.Value == "Fagaras").Key;
var path = BFS.Search(graph, startC, endC);

if (path != null)
{
	Console.WriteLine($"Shortest path from \"{citiesIndex[startC]}\" to \"{citiesIndex[endC]}\":");
	foreach (var city in path)
	{
		Console.Write(citiesIndex[city]);
		Console.Write('-');
	}
}
else
{
	Console.WriteLine("No path exists between city 1 and city 5");
}

Console.WriteLine("\n-------------------");

var dfsSearch = DFS.Search(graph, startC, endC);
if (dfsSearch != null)
{
	Console.WriteLine($"Shortest path from \"{citiesIndex[startC]}\" to \"{citiesIndex[endC]}\":");
	foreach (var city in dfsSearch)
	{
		Console.Write(citiesIndex[city]);
		Console.Write('-');
	}
}
else
{
	Console.WriteLine("No path exists between city 1 and city 5");
}

Console.WriteLine("\n-------------------");

var path4 = BidirectionalSearch.BiDirSearch(graph, startC, endC);
if (path4 != null)
{
	Console.WriteLine($"Shortest path from \"{citiesIndex[startC]}\" to \"{citiesIndex[endC]}\":");
	foreach (var city in path4)
	{
		Console.Write(citiesIndex[city]);
		Console.Write('-');
	}
}
else
{
	Console.WriteLine("No path exists between city 1 and city 5");

}

Console.WriteLine("\n-------------------");

var a = citiesIndex.FirstOrDefault(x => x.Value == "Oradea").Key;
var b = citiesIndex.FirstOrDefault(x => x.Value == "Arad").Key;
var c = citiesIndex.FirstOrDefault(x => x.Value == "Neamt").Key;
// heur

var heuristicDistances = new Dictionary<int, int>()
		{
			{  citiesIndex.FirstOrDefault(x => x.Value == "Arad").Key, 366 },
			{  citiesIndex.FirstOrDefault(x => x.Value == "Bucharest").Key, 0 },
			{  citiesIndex.FirstOrDefault(x => x.Value == "Craiova").Key, 160 },
			{  citiesIndex.FirstOrDefault(x => x.Value == "Drobita").Key, 242 },
			{  citiesIndex.FirstOrDefault(x => x.Value == "Eforie").Key, 161 },
			{  citiesIndex.FirstOrDefault(x => x.Value == "Fagaras").Key, 176 },
			{  citiesIndex.FirstOrDefault(x => x.Value == "Giurgiu").Key, 77 },
			{  citiesIndex.FirstOrDefault(x => x.Value == "Hirsova").Key, 151 },
			{  citiesIndex.FirstOrDefault(x => x.Value == "Iasi").Key, 226 },
			{  citiesIndex.FirstOrDefault(x => x.Value == "Lugoj").Key, 244 },
			{  citiesIndex.FirstOrDefault(x => x.Value == "Mehedia").Key, 241 },
			{  citiesIndex.FirstOrDefault(x => x.Value == "Neamt").Key, 234 },
			{  citiesIndex.FirstOrDefault(x => x.Value == "Oradea").Key, 380 },
			{  citiesIndex.FirstOrDefault(x => x.Value == "Pitesti").Key, 100 },
			{  citiesIndex.FirstOrDefault(x => x.Value == "RM").Key, 193 },
			{  citiesIndex.FirstOrDefault(x => x.Value == "Sibiu").Key, 253 },
			{  citiesIndex.FirstOrDefault(x => x.Value == "Timisoara").Key, 329 },
			{  citiesIndex.FirstOrDefault(x => x.Value == "Urziceni").Key, 80 },
			{  citiesIndex.FirstOrDefault(x => x.Value == "Vaslui").Key, 199 },
			{  citiesIndex.FirstOrDefault(x => x.Value == "Zerind").Key, 374 }
		};



List<int> path9 = GreedyBestFirstSearch.FindPath(graph.adjList, heuristicDistances, startC, endC);

if (path9 != null)
{
	Console.WriteLine($"Shortest path from \"{citiesIndex[startC]}\" to \"{citiesIndex[endC]}\":");
	foreach (var city in path9)
	{
		Console.Write(citiesIndex[city]);
		Console.Write('-');
	}
}
else
{
	Console.WriteLine("No path found!");
}


Console.WriteLine("\n-------------------");


// Call the AStar.FindPath method
var graph2 = new List<List<AStar.Node2>>();
foreach (var ff in graph.adjList)
{
	var minigraph2 = new List<AStar.Node2>();
	foreach (var gg in ff)
	{
		minigraph2.Add(new AStar.Node2() { Country = gg.country, Weight = gg.weight, Id = default, TotalCost = default });
	}
	graph2.Add(minigraph2);
}
List<int> path47 = AStar.FindPath(graph2, heuristicDistances, startC, endC);

// Handle the returned path
if (path47 != null)
{
	Console.WriteLine("Path found:");
	foreach (var node in path47)
	{
		Console.Write(citiesIndex[node] + " ");
	}
	Console.WriteLine();
}
else
{
	Console.WriteLine("No path found.");
}



Console.WriteLine("\n-------------------");
var graph4 = new List<List<Node9>>();
foreach (var ff in graph.adjList)
{
	var minigraph4 = new List<Node9>();
	foreach (var gg in ff)
	{
		minigraph4.Add(new Node9(gg.country, gg.weight));
	}
	graph4.Add(minigraph4);
}
UniformCostSearch ucs = new UniformCostSearch();
List<int> path89 = ucs.FindPath(graph4, startC, endC);

if (path89 != null)
{
	Console.WriteLine("Path found:");
	foreach (var node in path89)
	{
		Console.Write(citiesIndex[node] + " ");
	}
	Console.WriteLine();
}
// Find the path from city 0 (Oradea) to city 13 (Bucharest)
Console.ReadKey();
