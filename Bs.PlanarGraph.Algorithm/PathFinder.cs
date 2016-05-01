using System.Collections.Generic;
using System.Linq;
using Bs.PlanarGraph.Algorithm.Entities;

namespace Bs.PlanarGraph.Algorithm
{
    /// <summary>
    /// Uses DFS to find a path (not necessarily shortest) between two nodes.
    /// </summary>
    public class PathFinder
    {
        private List<Node> ExploredNodes { get; set; }
        private List<Node> Path { get; set; }
        private Graph Graph { get; set; }
        private bool FoundPath { get; set; }
        private Node FinishNode { get; set; }

        public List<Edge> FindPath(Graph graph, Node from, Node to)
        {
            ExploredNodes = new List<Node>();
            Path = new List<Node>();
            Graph = graph;
            FoundPath = false;
            List<Edge> pathEdges = new List<Edge>(); 
                
            ConstructPathDfs(from);

            for (int i = 1; i < Path.Count; i++)
            {
                pathEdges.Add(graph.GetEdge(Path[i], Path[i - 1]));
            }

            return pathEdges;
        }

        private void ConstructPathDfs(Node node)
        {
            ExploredNodes.Add(node);
            if (node == FinishNode)
            {
                FoundPath = true;
                return;
            }
            foreach (Node adjNode in Graph.GetAdjacents(node).Where(n => !ExploredNodes.Contains(n)))
            {
                Path.Add(adjNode);
                ConstructPathDfs(adjNode);
                if (FoundPath) break;
                Path.Remove(adjNode);
            }
        }
    }
}