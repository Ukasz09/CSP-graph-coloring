using System.Collections.Generic;

namespace csp_problem
{
    public class Graph
    {
        private List<Edge> _edges;
        private List<Node> _nodes;

        public Graph(List<Edge> edges, List<Node> nodes)
        {
            _edges = edges;
            _nodes = nodes;
        }

        public override string ToString()
        {
            return string.Join(",", _edges);
        }
    }
}