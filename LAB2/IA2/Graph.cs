public struct Node
{
	public int country;
	public int weight;
	public Node() { }
	public Node(int c, int w)
	{
		country = c; weight = w;
	}
}
public class Graph
{
	public int numVertices;
	public List<List<Node>> adjList;

	public Graph(int numVertices)
	{
		this.numVertices = numVertices;
		adjList = new List<List<Node>>(numVertices);

		// Initialize adjacency list with empty lists
		for (int i = 0; i < numVertices; i++)
		{
			adjList.Add(new List<Node>());
		}
	}

	public void AddEdge(int source, int destination, int weight)
	{
		adjList[source].Add(new Node(destination, weight));
	}
}
public static class BFS
{
	public static List<int> Search(Graph graph, int start, int goal)
	{
		var visited = new bool[graph.numVertices];
		var queue = new Queue<int>();
		var parent = new int[graph.numVertices]; // Added for path reconstruction

		visited[start] = true;
		queue.Enqueue(start);
		parent[start] = -1; // Parent of starting vertex is -1 (no parent)

		while (queue.Count > 0)
		{
			var current = queue.Dequeue();

			if (current == goal)
			{
				// Path found, reconstruct the path from goal to start
				return ReconstructPath(parent, goal);
			}

			foreach (var neighbor in graph.adjList[current])
			{
				if (!visited[neighbor.country])
				{
					visited[neighbor.country] = true;
					queue.Enqueue(neighbor.country);
					parent[neighbor.country] = current; // Track the parent of the neighbor for path reconstruction
				}
			}
		}

		return null; // No path found
	}

	private static List<int> ReconstructPath(int[] parent, int goal)
	{
		var path = new List<int>();
		var current = goal;

		while (current != -1)
		{
			path.Insert(0, current);
			current = parent[current];
		}

		return path;
	}
}


public static class DFS
{
	public static List<int> Search(Graph graph, int start, int end)
	{
		var visited = new bool[graph.numVertices];
		var path = new List<int>(); // Stores the path

		if (SearchHelper(graph, start, end, visited, path))
		{
			return path; // Path found, return it
		}

		return null; // No path found
	}

	private static bool SearchHelper(Graph graph, int current, int end, bool[] visited, List<int> path)
	{
		visited[current] = true;
		path.Add(current); // Add current vertex to the path

		if (current == end)
		{
			return true; // Goal reached, path found
		}

		foreach (var neighbor in graph.adjList[current])
		{
			if (!visited[neighbor.country])
			{
				if (SearchHelper(graph, neighbor.country, end, visited, path)) // Recursive call for unvisited neighbors
				{
					return true; // Path found through a neighbor
				}
			}
		}

		// Backtrack if no path found through current neighbors
		path.RemoveAt(path.Count - 1); // Remove current vertex from path (backtracking)
		return false;
	}
}


public static class BidirectionalSearch
{
	public static void BFS2(Graph graph1, Queue<int> queue, bool[] visited, int[] parent)
	{
		int current = queue.Dequeue();
		foreach (var i in graph1.adjList[current])
		{
			// If adjacent vertex is not visited earlier
			// mark it visited by assigning true country
			if (!visited[i.country])
			{
				// set current as parent of this vertex
				parent[i.country] = current;

				// Mark this vertex visited
				visited[i.country] = true;

				// Push to the end of queue
				queue.Enqueue(i.country);
			}
		}
	}
	// check for intersecting vertex
	public static int IsIntersecting(Graph graph1, bool[] s_visited,
							  bool[] t_visited)
	{
		for (int i = 0; i < graph1.numVertices; i++)
		{
			// if a vertex is visited by both front
			// and back BFS search return that node
			// else return -1
			if (s_visited[i] && t_visited[i])
				return i;
		}
		return -1;
	}
	// Print the path from source to target
	public static List<int> PrintPath(int[] s_parent, int[] t_parent,
						  int s, int t, int intersectNode)
	{
		List<int> path = new List<int>();
		path.Add(intersectNode);
		int i = intersectNode;
		while (i != s)
		{
			path.Add(s_parent[i]);
			i = s_parent[i];
		}
		path.Reverse();
		i = intersectNode;
		while (i != t)
		{
			path.Add(t_parent[i]);
			i = t_parent[i];
		}
		return path;
		//Console.WriteLine("*****Path*****");
		//foreach (int it in path) Console.Write(it + " ");
		//Console.WriteLine();
	}
	// Method for bidirectional searching
	public static List<int> BiDirSearch(Graph graph1, int s, int t)
	{
		int V = graph1.numVertices;
		// boolean array for BFS started from
		// source and target(front and backward BFS)
		// for keeping track on visited nodes
		bool[] s_visited = new bool[V];
		bool[] t_visited = new bool[V];

		// Keep track on parents of nodes
		// for front and backward search
		int[] s_parent = new int[V];
		int[] t_parent = new int[V];

		// queue for front and backward search
		Queue<int> s_queue = new Queue<int>();
		Queue<int> t_queue = new Queue<int>();

		int intersectNode = -1;

		// necessary initialization
		for (int i = 0; i < V; i++)
		{
			s_visited[i] = false;
			t_visited[i] = false;
		}

		s_queue.Enqueue(s);
		s_visited[s] = true;

		// parent of source is set to -1
		s_parent[s] = -1;

		t_queue.Enqueue(t);
		t_visited[t] = true;

		// parent of target is set to -1
		t_parent[t] = -1;

		while (s_queue.Count > 0 && t_queue.Count > 0)
		{
			// Do BFS from source and target vertices
			BFS2(graph1, s_queue, s_visited, s_parent);
			BFS2(graph1, t_queue, t_visited, t_parent);

			// check for intersecting vertex
			intersectNode = IsIntersecting(graph1, s_visited, t_visited);

			// If intersecting vertex is found
			// that means there exist a path
			if (intersectNode != -1)
			{
				Console.WriteLine(
					"Path exist between {0} and {1}", s, t);
				Console.WriteLine("Intersection at: {0}",
								  intersectNode);

				// print the path and exit the program
				//PrintPath(s_parent, t_parent, s, t,intersectNode);
				return PrintPath(s_parent, t_parent, s, t, intersectNode);
				//Environment.Exit(0);
				//break;
			}
		}
		return null;
	}
}



public static class GreedyBestFirstSearch
{
	private static List<List<Node>> _adjList;
	private static Dictionary<int, int> _heuristicDistances;

	public static List<int> FindPath(List<List<Node>> adjList, Dictionary<int, int> distances, int startNode, int endNode)
	{
		_adjList = adjList;
		_heuristicDistances = distances;

		List<int> path = new List<int>();
		HashSet<int> visited = new HashSet<int>();

		PriorityQueue<int> priorityQueue = new PriorityQueue<int>((x, y) => _heuristicDistances[x] - _heuristicDistances[y]);
		Dictionary<int, int> parent = new Dictionary<int, int>();

		priorityQueue.Enqueue(startNode);
		visited.Add(startNode);

		while (priorityQueue.Count > 0)
		{
			int currentNode = priorityQueue.Dequeue();

			if (currentNode == endNode)
			{
				path = ReconstructPath(parent, currentNode);
				return path;
			}

			foreach (var neighbor in adjList[currentNode])
			{
				if (!visited.Contains(neighbor.country))
				{
					visited.Add(neighbor.country);
					parent[neighbor.country] = currentNode;
					priorityQueue.Enqueue(neighbor.country);
				}
			}
		}

		return null; // No path found
	}

	private static List<int> ReconstructPath(Dictionary<int, int> parent, int currentNode)
	{
		List<int> path = new List<int>();
		while (parent.ContainsKey(currentNode))
		{
			path.Insert(0, currentNode);
			currentNode = parent[currentNode];
		}
		path.Insert(0, currentNode);
		return path;
	}

	// Priority Queue implementation
	public class PriorityQueue<T>
	{
		private List<T> data;
		private Comparison<T> comparison;

		public int Count { get { return data.Count; } }

		public PriorityQueue(Comparison<T> comparison)
		{
			this.data = new List<T>();
			this.comparison = comparison;
		}

		public void Enqueue(T item)
		{
			data.Add(item);
			int ci = data.Count - 1; // child index; start at end
			while (ci > 0)
			{
				int pi = (ci - 1) / 2; // parent index
				if (comparison(data[ci], data[pi]) >= 0)
					break; // child item is larger than (or equal) parent so we're done
				T tmp = data[ci];
				data[ci] = data[pi];
				data[pi] = tmp;
				ci = pi;
			}
		}

		public T Dequeue()
		{
			// assumes pq isn't empty; up to calling code
			int li = data.Count - 1; // last index (before removal)
			T frontItem = data[0];   // fetch the front
			data[0] = data[li];
			data.RemoveAt(li);

			--li; // last index (after removal)
			int pi = 0; // parent index. start at front of pq
			while (true)
			{
				int ci = pi * 2 + 1; // left child index of parent
				if (ci > li)
					break;  // no children so done
				int rc = ci + 1;     // right child
				if (rc <= li && comparison(data[rc], data[ci]) < 0) // if there is a rc (ci + 1), and it is smaller than left child, use the rc instead
					ci = rc;
				if (comparison(data[pi], data[ci]) <= 0)
					break; // parent is smaller than (or equal to) smallest child so done
				T tmp = data[pi];
				data[pi] = data[ci];
				data[ci] = tmp; // swap parent and child
				pi = ci;
			}
			return frontItem;
		}
	}
}


public static class AStar
{
	private static List<List<Node2>> _adjList;
	private static Dictionary<int, int> _heuristicDistances;

	public static List<int> FindPath(List<List<Node2>> adjList, Dictionary<int, int> heuristicDistances, int startNode, int endNode)
	{
		_adjList = adjList;
		_heuristicDistances = heuristicDistances;

		List<int> path = new List<int>();
		HashSet<int> visited = new HashSet<int>();

		PriorityQueue<Node2> priorityQueue = new PriorityQueue<Node2>((x, y) => x.TotalCost.CompareTo(y.TotalCost));
		Dictionary<int, int> gCosts = new Dictionary<int, int>();
		Dictionary<int, int> parent = new Dictionary<int, int>();

		priorityQueue.Enqueue(new Node2(startNode, 0, _heuristicDistances[startNode]));
		gCosts[startNode] = 0;
		visited.Add(startNode);

		while (priorityQueue.Count > 0)
		{
			Node2 currentNode = priorityQueue.Dequeue();

			if (currentNode.Id == endNode)
			{
				path = ReconstructPath(parent, currentNode.Id);
				return path;
			}

			foreach (var neighbor in _adjList[currentNode.Id])
			{
				int tentativeGCost = gCosts[currentNode.Id] + neighbor.Weight;
				if (!gCosts.ContainsKey(neighbor.Country) || tentativeGCost < gCosts[neighbor.Country])
				{
					gCosts[neighbor.Country] = tentativeGCost;
					int totalCost = tentativeGCost + _heuristicDistances[neighbor.Country];
					if (!visited.Contains(neighbor.Country))
					{
						visited.Add(neighbor.Country);
						parent[neighbor.Country] = currentNode.Id;
						priorityQueue.Enqueue(new Node2(neighbor.Country, tentativeGCost, totalCost));
					}
				}
			}
		}

		return null; // No path found
	}

	private static List<int> ReconstructPath(Dictionary<int, int> parent, int currentNode)
	{
		List<int> path = new List<int>();
		while (parent.ContainsKey(currentNode))
		{
			path.Insert(0, currentNode);
			currentNode = parent[currentNode];
		}
		path.Insert(0, currentNode);
		return path;
	}

	public struct Node2
	{
		public int Country { get; set; }
		public int Weight { get; set; }
		public int Id { get; set; }
		public int TotalCost { get; set; }

		public Node2(int country, int weight, int totalCost)
		{
			Country = country;
			Weight = weight;
			Id = country; // Assuming Id is same as Country in this context
			TotalCost = totalCost;
		}
	}

	// Priority Queue implementation
	public class PriorityQueue<T>
	{
		private List<T> data;
		private Comparison<T> comparison;

		public int Count { get { return data.Count; } }

		public PriorityQueue(Comparison<T> comparison)
		{
			this.data = new List<T>();
			this.comparison = comparison;
		}

		public void Enqueue(T item)
		{
			data.Add(item);
			int ci = data.Count - 1; // child index; start at end
			while (ci > 0)
			{
				int pi = (ci - 1) / 2; // parent index
				if (comparison(data[ci], data[pi]) >= 0)
					break; // child item is larger than (or equal) parent so we're done
				T tmp = data[ci];
				data[ci] = data[pi];
				data[pi] = tmp;
				ci = pi;
			}
		}

		public T Dequeue()
		{
			// assumes pq isn't empty; up to calling code
			int li = data.Count - 1; // last index (before removal)
			T frontItem = data[0];   // fetch the front
			data[0] = data[li];
			data.RemoveAt(li);

			--li; // last index (after removal)
			int pi = 0; // parent index. start at front of pq
			while (true)
			{
				int ci = pi * 2 + 1; // left child index of parent
				if (ci > li)
					break;  // no children so done
				int rc = ci + 1;     // right child
				if (rc <= li && comparison(data[rc], data[ci]) < 0) // if there is a rc (ci + 1), and it is smaller than left child, use the rc instead
					ci = rc;
				if (comparison(data[pi], data[ci]) <= 0)
					break; // parent is smaller than (or equal to) smallest child so done
				T tmp = data[pi];
				data[pi] = data[ci];
				data[ci] = tmp; // swap parent and child
				pi = ci;
			}
			return frontItem;
		}
	}
}


public class UniformCostSearch
{
	private List<List<Node9>> adjList;

	public List<int> FindPath(List<List<Node9>> adjList, int startNode, int endNode)
	{
		List<int> path = new List<int>();
		HashSet<int> visited = new HashSet<int>();

		PriorityQueue<Node> priorityQueue = new PriorityQueue<Node>((x, y) => x.Cost.CompareTo(y.Cost));
		Dictionary<int, int> parent = new Dictionary<int, int>();

		priorityQueue.Enqueue(new Node(startNode, 0));
		visited.Add(startNode);

		while (priorityQueue.Count > 0)
		{
			Node currentNode = priorityQueue.Dequeue();

			if (currentNode.Id == endNode)
			{
				path = ReconstructPath(parent, currentNode.Id);
				return path;
			}

			foreach (var neighbor in adjList[currentNode.Id])
			{
				if (!visited.Contains(neighbor.Id))
				{
					visited.Add(neighbor.Id);
					parent[neighbor.Id] = currentNode.Id;
					priorityQueue.Enqueue(new Node(neighbor.Id, currentNode.Cost + neighbor.Cost));
				}
			}
		}

		return null; // No path found
	}

	private List<int> ReconstructPath(Dictionary<int, int> parent, int currentNode)
	{
		List<int> path = new List<int>();
		while (parent.ContainsKey(currentNode))
		{
			path.Insert(0, currentNode);
			currentNode = parent[currentNode];
		}
		path.Insert(0, currentNode);
		return path;
	}

	public class Node
	{
		public int Id { get; }
		public int Cost { get; }

		public Node(int id, int cost)
		{
			Id = id;
			Cost = cost;
		}
	}

	// Priority Queue implementation
	public class PriorityQueue<T>
	{
		private List<T> data;
		private Comparison<T> comparison;

		public int Count { get { return data.Count; } }

		public PriorityQueue(Comparison<T> comparison)
		{
			this.data = new List<T>();
			this.comparison = comparison;
		}

		public void Enqueue(T item)
		{
			data.Add(item);
			int ci = data.Count - 1; // child index; start at end
			while (ci > 0)
			{
				int pi = (ci - 1) / 2; // parent index
				if (comparison(data[ci], data[pi]) >= 0)
					break; // child item is larger than (or equal) parent so we're done
				T tmp = data[ci];
				data[ci] = data[pi];
				data[pi] = tmp;
				ci = pi;
			}
		}

		public T Dequeue()
		{
			// assumes pq isn't empty; up to calling code
			int li = data.Count - 1; // last index (before removal)
			T frontItem = data[0];   // fetch the front
			data[0] = data[li];
			data.RemoveAt(li);

			--li; // last index (after removal)
			int pi = 0; // parent index. start at front of pq
			while (true)
			{
				int ci = pi * 2 + 1; // left child index of parent
				if (ci > li)
					break;  // no children so done
				int rc = ci + 1;     // right child
				if (rc <= li && comparison(data[rc], data[ci]) < 0) // if there is a rc (ci + 1), and it is smaller than left child, use the rc instead
					ci = rc;
				if (comparison(data[pi], data[ci]) <= 0)
					break; // parent is smaller than (or equal to) smallest child so done
				T tmp = data[pi];
				data[pi] = data[ci];
				data[ci] = tmp; // swap parent and child
				pi = ci;
			}
			return frontItem;
		}
	}
}


public class Node9
{
	public int Id { get; }
	public int Cost { get; }

	public Node9(int id, int cost)
	{
		Id = id;
		Cost = cost;
	}
}