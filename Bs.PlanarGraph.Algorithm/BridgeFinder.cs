using System;
using System.Collections.Generic;
using System.Linq;
using Bs.PlanarGraph.Algorithm.Entities;

namespace Bs.PlanarGraph.Algorithm
{
    public class BridgeFinder
    {
        List<Edge> VisitedEdges { get; set; }
        Graph MainGraph { get; set; }
        Graph SubGraph { get; set; }
         

        // Finds bridges of mainGraph with respect to subGraph using DFS.
        public List<Graph> FindBridges(Graph mainGraph, Graph subGraph)
        {
            VisitedEdges = new List<Edge>();
            MainGraph = mainGraph;
            SubGraph = subGraph;
            List <Graph> bridges = new List<Graph>();

            foreach (Node node in subGraph.Nodes)
            {
                List<Edge> edges = mainGraph.GetEdges(node).Where(e => !subGraph.Edges.Contains(e)).ToList();
                foreach (Edge edge in edges)
                {
                    if (VisitedEdges.Contains(edge)) continue;
                    List<Edge> piece = new List<Edge>();
                    ExplorePieceDfs(node, edge, piece);

                    // Determine if the piece is a bridge
                    bool isBridge = false;
                    Graph bridge = new Graph();
                    bridge.PointsOfContract = new List<Node>();
                    foreach (Edge pieceEdge in piece)
                    {
                        if (subGraph.Nodes.Contains(pieceEdge.Node1) && !bridge.PointsOfContract.Contains(pieceEdge.Node1))
                        {
                            bridge.PointsOfContract.Add(pieceEdge.Node1);
                        }
                        if (subGraph.Nodes.Contains(pieceEdge.Node2) && !bridge.PointsOfContract.Contains(pieceEdge.Node2))
                        {
                            bridge.PointsOfContract.Add(pieceEdge.Node2);
                        }
                        if (bridge.PointsOfContract.Count >= 2)
                        {
                            // The piece is a bridge because it has 2 or more points of contract
                            isBridge = true;
                        }
                        bridge.AddNode(pieceEdge.Node1);
                        bridge.AddNode(pieceEdge.Node2);
                        bridge.AddEdge(pieceEdge);
                    }
                    if (isBridge)
                    {
                        bridges.Add(bridge);
                    }
                }
            }

            return bridges;
        }

        public void ExplorePieceDfs(Node conNode, Edge edge, List<Edge> piece)
        {
            piece.Add(edge);
            VisitedEdges.Add(edge);
            Node otherNode = edge.OtherNode(conNode);
            if (SubGraph.Nodes.Contains(otherNode)) return;
            foreach (Edge source in MainGraph.GetEdges(otherNode).Where(e => !VisitedEdges.Contains(e)))
            {
                ExplorePieceDfs(otherNode, source, piece);
            }
        }
    }
}