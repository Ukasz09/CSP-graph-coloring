using System.Collections.Generic;
using System.Linq;

namespace csp_problem
{
    public class Graph
    {
        private readonly List<Edge> _edges;
        public List<Node> Nodes { get; }

        public Graph(List<Edge> edges, List<Node> nodes)
        {
            _edges = edges;
            Nodes = nodes;
        }

        public override string ToString()
        {
            return string.Join(",", _edges);
        }

        public IEnumerable<Node> GetNeighbours(Node node)
        {
            var connectedEdges = _edges.Where(e => e.StartNode.Equals(node) || e.EndNode.Equals(node));
            var nodes = new List<Node>();
            foreach (var edge in connectedEdges)
            {
                nodes.Add(edge.StartNode);
                nodes.Add(edge.EndNode);
            }

            var nodesOtherThanGiven = nodes.Where(n => !n.Equals(node));
            return nodesOtherThanGiven.ToList();
        }
    }
}