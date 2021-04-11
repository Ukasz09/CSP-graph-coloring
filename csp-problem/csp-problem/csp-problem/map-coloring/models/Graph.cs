using System.Collections.Generic;
using System.Linq;

namespace csp_problem
{
    public class Graph
    {
        private List<Edge> _edges;
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
            var connectedEdges = _edges.Where(e => e.startNode.Equals(node) || e.endNode.Equals(node));
            var nodes = new List<Node>();
            foreach (var edge in connectedEdges)
            {
                nodes.Add(edge.startNode);
                nodes.Add(edge.endNode);
            }

            var nodesOtherThanGiven = nodes.Where(n => !n.Equals(node));
            return nodesOtherThanGiven.ToList();
        }
    }
}