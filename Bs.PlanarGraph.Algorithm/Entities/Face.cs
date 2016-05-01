using System.Collections.Generic;
using System.Linq;

namespace Bs.PlanarGraph.Algorithm.Entities
{
    public class Face
    {
        public List<Edge> Edges { get; set; }
        public List<Node> Nodes { get; set; }

        public Face()
        {
            Edges = new List<Edge>();
            Nodes = new List<Node>();
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

        // Can be used to traverse the boundary edges of the
        // face in a clockwise or counter-clockwise sense.
        // Finds the edge that is connected to the given edge but not prevnode
        public Edge GetNextEdge(Edge edge, Node prevNode)
        {
            Node otherNode = edge.OtherNode(prevNode);
            return Edges.Find(e => (e.Node1 == otherNode && e.Node2 != prevNode) || (e.Node2 == otherNode && e.Node1 != prevNode));
        }
    }
}