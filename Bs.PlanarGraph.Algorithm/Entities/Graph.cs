using System;
using System.Collections.Generic;
using System.Linq;

namespace Bs.PlanarGraph.Algorithm.Entities
{
    public class Graph : ICloneable
    {
        public List<Edge> Edges { get; set; }
        public List<Node> Nodes { get; set; }
        public List<Face> Faces { get; set; } 

        // If the graph is a bridge, this holds the list of nodes that are the points
        // of contract to the main graph
        public List<Node> PointsOfContract { get; set; } 

        public Graph()
        {
            Edges = new List<Edge>();
            Nodes = new List<Node>();
        }

        public Node GetOrCreateNode(string id)
        {
            Node node = Nodes.FirstOrDefault(n => n.Id == id);
            if (node != null)
            {
                return node;
            }
            node = new Node {Id = id};
            Nodes.Add(node);
            return node;
        }

        public void AddNode(Node node)
        {
            if (!Nodes.Contains(node))
            {
                Nodes.Add(node);
            }
        }

        public void AddEdge(Edge edge)
        {
            if (!Edges.Contains(edge))
            {
                Edges.Add(edge);
            }
        }

        public Edge GetOrCreateEdge(Node node1, Node node2)
        {
            Edge edge = Edges.FirstOrDefault(e => (e.Node1 == node1 && e.Node2 == node2) || (e.Node2 == node1 && e.Node1 == node2));
            if (edge != null)
            {
                return edge;
            }
            edge = new Edge {Node1 = node1, Node2 = node2};
            Edges.Add(edge);
            return edge;
        }

        // If the graph is not connected, get a list of connected components of the graph.
        // Uses breadth-first search to explore nodes.
        public List<Graph> SplitDisonnectedComponents()
        {
            List<Graph> connComps = new List<Graph>();

            List<Node> unexploredNodes = new List<Node>(Nodes);
            Queue<Node> nodesToExplore = new Queue<Node>();
            List<List<Node>> connNodes = new List<List<Node>>();

            while (unexploredNodes.Count > 0)
            {
                connNodes.Add(new List<Node>());
                int index = connNodes.Count - 1;
                nodesToExplore.Enqueue(unexploredNodes[0]);
                while (nodesToExplore.Count > 0)
                {
                    Node nodeToExplore = nodesToExplore.Dequeue();
                    connNodes[index].Add(nodeToExplore);
                    List<Node> adjNodes = GetAdjacents(nodeToExplore);
                    foreach (Node adjNode in adjNodes)
                    {
                        if (!connNodes[index].Contains(adjNode))
                        {
                            nodesToExplore.Enqueue(adjNode);
                        }
                    }
                    unexploredNodes.Remove(nodeToExplore);
                }
            }

            if (connNodes.Count == 1)
            {
                return new List<Graph> {this};
            }

            foreach (List<Node> connNodeGroup in connNodes)
            {
                Graph connComp = new Graph();
                foreach (Node node in connNodeGroup)
                {
                    connComp.AddNode(node);
                    List<Edge> edges = GetEdges(node);
                    foreach (Edge edge in edges)
                    {
                        connComp.AddEdge(edge);
                    }
                }
                connComps.Add(connComp);
            }

            return connComps;
        }

        public List<Node> GetAdjacents(Node node)
        {
            List<Node> adjList = new List<Node>();
            adjList.AddRange(Edges.Where(e => e.Node1 == node).Select(e => e.Node2));
            adjList.AddRange(Edges.Where(e => e.Node2 == node).Select(e => e.Node1));
            return adjList;
        }

        public List<Edge> GetEdges(Node node)
        {
            List<Edge> edgeList = new List<Edge>();
            edgeList.AddRange(Edges.Where(e => e.Node1 == node));
            edgeList.AddRange(Edges.Where(e => e.Node2 == node));
            return edgeList;
        }

        // While this graph is a circuit, this function initializes its faces
        public void SetCircuitFaces()
        {
            Faces = new List<Face>();
            Face innerFace = new Face();
            Face outerFace = new Face();
            foreach (Edge edge in Edges)
            {
                innerFace.AddEdge(edge);
                innerFace.AddNode(edge.Node1);
                innerFace.AddNode(edge.Node2);
                outerFace.AddEdge(edge);
                outerFace.AddNode(edge.Node1);
                outerFace.AddNode(edge.Node2);
            }
            Faces.Add(innerFace);
            Faces.Add(outerFace);
        }

        // Simulates the drawing of the edges into the designated face
        // and splits the face into two
        public void AddBridgeAndSeperateFace(List<Edge> edgesToAdd, Face face, Node[] pointsOfContract)
        {
            Face face1 = new Face();
            Face face2 = new Face();
            foreach (Edge edge in edgesToAdd)
            {
                AddNode(edge.Node1);
                AddNode(edge.Node2);
                AddEdge(edge);
                face1.AddNode(edge.Node1);
                face1.AddNode(edge.Node2);
                face1.AddEdge(edge);
                face2.AddNode(edge.Node1);
                face2.AddNode(edge.Node2);
                face2.AddEdge(edge);
            }
            Face oldFace = face;
            Edge faceEdge = oldFace.Edges.First(e => e.Node1 == pointsOfContract[0] || e.Node2 == pointsOfContract[0]);
            Node prevNode = pointsOfContract[0];
            Node otherNode;
            while (faceEdge.Node1 != PointsOfContract[1] || faceEdge.Node2 != PointsOfContract[1])
            {
                face1.AddNode(faceEdge.Node1);
                face1.AddNode(faceEdge.Node2);
                face1.AddEdge(faceEdge);
                otherNode = faceEdge.OtherNode(prevNode);
                faceEdge = oldFace.GetNextEdge(faceEdge, prevNode);
                prevNode = otherNode;
            }
            face1.AddNode(faceEdge.Node1);
            face1.AddNode(faceEdge.Node2);
            face1.AddEdge(faceEdge);
            otherNode = faceEdge.OtherNode(prevNode);
            faceEdge = oldFace.GetNextEdge(faceEdge, prevNode);
            prevNode = otherNode;
            while (faceEdge.Node1 != PointsOfContract[0] || faceEdge.Node2 != PointsOfContract[0])
            {
                face2.AddNode(faceEdge.Node1);
                face2.AddNode(faceEdge.Node2);
                face2.AddEdge(faceEdge);
                otherNode = faceEdge.OtherNode(prevNode);
                faceEdge = oldFace.GetNextEdge(faceEdge, prevNode);
                prevNode = otherNode;
            }
            face2.AddNode(faceEdge.Node1);
            face2.AddNode(faceEdge.Node2);
            face2.AddEdge(faceEdge);

            Faces.Remove(oldFace);
            Faces.Add(face1);
            Faces.Add(face2);
        }

        public Edge GetEdge(Node node1, Node node2)
        {
            return Edges.FirstOrDefault(e => (e.Node1 == node1 && e.Node2 == node2) || (e.Node2 == node1 && e.Node1 == node2));
        }

        public object Clone()
        {
            Graph clonedGraph = new Graph();
            foreach (Edge edge in this.Edges)
            {
                Node clonedNode1 = clonedGraph.GetOrCreateNode(edge.Node1.Id);
                Node clonedNode2 = clonedGraph.GetOrCreateNode(edge.Node2.Id);
                clonedGraph.GetOrCreateEdge(clonedNode1, clonedNode2);
            }
            return clonedGraph;
        }
    }
}
