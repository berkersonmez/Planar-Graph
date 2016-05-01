namespace Bs.PlanarGraph.Algorithm.Entities
{
    public class Edge
    {
        public Node Node1 { get; set; }
        public Node Node2 { get; set; }

        public Node OtherNode(Node node)
        {
            return (node == Node1 ? Node2 : Node1);
        }

        public bool Contains(Node node)
        {
            return (node == Node1 || node == Node2);
        }
    }
}