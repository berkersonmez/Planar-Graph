using System;
using System.Collections.Generic;
using Bs.PlanarGraph.Algorithm.Entities;

namespace Bs.PlanarGraph.Algorithm
{

    /// <summary>
    /// Fints cut vertices of a graph.
    /// Algorithm described in "Algorithmic Graph Theory" slides of
    /// Paul Bonsma (2011) (HU Berlin) (http://goo.gl/dh5DXP) is used.
    /// </summary>
    public class CutVerticesFinder
    {
        bool[] Visited { get; set; }
        int[] Disc { get; set; }
        int[] Low { get; set; }
        int?[] Parent { get; set; }
        bool[] IsCutVertice { get; set; }
        int Time { get; set; }

        // Uses DFS approach to find cut vertices.
        public List<Node> FindCutVertices(Graph graph)
        {
            int n = graph.Nodes.Count;
            Visited = new bool[n];
            Disc = new int[n];
            Low = new int[n];
            Parent = new int?[n];
            IsCutVertice = new bool[n];

            for (int i = 0; i < n; i++)
            {
                Parent[i] = null;
                Visited[i] = false;
                IsCutVertice[i] = false;
            }

            for (int i = 0; i < n; i++)
            {
                if (!Visited[i])
                {
                    DfsTraversal(i, graph);
                }
            }

            List<Node> cutVertices = new List<Node>();

            for (int i = 0; i < n; i++)
            {
                if (IsCutVertice[i])
                {
                    cutVertices.Add(graph.Nodes[i]);
                }
            }

            return cutVertices;
        }

        public void DfsTraversal(int u, Graph graph)
        {
            int children = 0;
            Visited[u] = true;
            Disc[u] = Low[u] = ++Time;

            foreach (Node node in graph.GetAdjacents(graph.Nodes[u]))
            {
                int v = graph.Nodes.IndexOf(node);
                if (!Visited[v])
                {
                    children++;
                    Parent[v] = u;
                    DfsTraversal(v, graph);

                    Low[u] = Math.Min(Low[u], Low[v]);

                    if (Parent[u] == null && children > 1)
                        IsCutVertice[u] = true;

                    if (Parent[u] != null && Low[v] >= Disc[u])
                        IsCutVertice[u] = true;
                }
                else if (v != Parent[u])
                {
                    Low[u] = Math.Min(Low[u], Disc[v]);
                }

            }
        }
    }
}