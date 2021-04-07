using System.Collections.Generic;

namespace csp_problem
{
    public class MapGraph
    {
        private List<Edge> edges;
        private List<Node> nodes;

        public MapGraph(List<Edge> edges, List<Node> nodes)
        {
            this.edges = edges;
            this.nodes = nodes;
        }

        public override string ToString()
        {
            return string.Join(",", edges);
        }
    }
}