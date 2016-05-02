using System;
using System.Collections.Generic;
using System.Linq;
using Bs.PlanarGraph.Algorithm.Entities;

namespace Bs.PlanarGraph.Algorithm
{
    /// <summary>
    /// Uses DFS to return a circuit of the graph represented as a Graph object
    /// </summary>
    public class CycleFinder
    {
        List<Edge> VisitedEdges { get; set; }
        List<Node> VisitedNodes { get; set; }
        Node StartNode { get; set; }
        Graph Graph { get; set; }
        bool Done { get; set; }

        public Graph FindCycle(Graph graph)
        {
            Graph cycle = new Graph();
            Done = false;
            Graph = graph;
            VisitedEdges = new List<Edge>();
            VisitedNodes = new List<Node>();
            StartNode = graph.Nodes[0];

            FindCircuitDfs(StartNode);

            if (VisitedEdges.Count == 0) return null;

            // Designate circuit from DFS tree using backpropogation
            Node node = StartNode;
            for (int i = VisitedEdges.Count-1; i >= 0; i--)
            {
                if (!VisitedEdges[i].Contains(node))
                {
                    continue;
                }
                Node otherNode = VisitedEdges[i].OtherNode(node);
                cycle.AddNode(node);
                cycle.AddNode(otherNode);
                cycle.AddEdge(VisitedEdges[i]);
                node = otherNode;
            }

            return cycle;
        }

        private void FindCircuitDfs(Node currentNode)
        {
            if (Done) return;
            
            if (currentNode == StartNode && VisitedEdges.Count > 0)
            {
                Done = true;
                return;
            } 
            List<Edge> adjEdges = Graph.GetEdges(currentNode);
            adjEdges = adjEdges.Where(e => VisitedEdges.All(ve => ve != e) && VisitedNodes.All(vn => vn != e.OtherNode(currentNode))).ToList();
            foreach (Edge adjEdge in adjEdges)
            {
                VisitedEdges.Add(adjEdge);
                Node otherNode = adjEdge.OtherNode(currentNode);
                VisitedNodes.Add(otherNode);
                FindCircuitDfs(otherNode);
                if (Done) return;
                VisitedNodes.Remove(otherNode);
                VisitedEdges.Remove(adjEdge);
            }
        }
    }
}