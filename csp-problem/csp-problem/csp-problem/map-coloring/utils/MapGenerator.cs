using System;
using System.Collections.Generic;
using System.Linq;

namespace csp_problem
{
    public class MapGenerator
    {
        private readonly Random _randGen;

        public MapGenerator()
        {
            _randGen = new Random();
        }

        public Graph GenerateMap(int nodesQty, int mapWidth, int mapHeight)
        {
            var nodes = GraphUtils.GenerateUniqueNodes(nodesQty, mapWidth, mapHeight, _randGen);
            var nodesCopy = GraphUtils.CloneNodes(nodes);
            var edges = GenerateEdges(nodesCopy);
            var map = new Graph(edges, nodes);
            return map;
        }

        private List<Edge> GenerateEdges(ICollection<Node> nodes)
        {
            var edges = new List<Edge>();
            while (nodes.Count > 1)
            {
                var nodesCopy = GraphUtils.CloneNodes(nodes);

                // Choose started node
                var startedNode = nodesCopy[_randGen.Next(nodesCopy.Count)];
                nodesCopy.Remove(startedNode);

                // Generate not intersected edge
                var generatedEdge = GenerateNotIntersectedEdge(startedNode, nodesCopy, edges);
                if (generatedEdge != null)
                {
                    edges.Add(generatedEdge);
                }
                else
                {
                    // Any node cannot be connected with that one - need to remove it
                    nodes.Remove(startedNode);
                }
            }

            // No more edges to create
            return edges;
        }

        private static Edge GenerateNotIntersectedEdge(Node startedNode, ICollection<Node> otherNodes, List<Edge> edges)
        {
            while (otherNodes.Count > 0)
            {
                // Generate edge
                var closestNode = GraphUtils.GetClosestNode(startedNode, otherNodes);
                otherNodes.Remove(closestNode);
                var edge = new Edge(startedNode, closestNode);

                // Check edge correctness
                var differentThanOthers = !edges.Exists(e => e.Equals(edge));
                var intersectWithAnyEdge = GraphUtils.IntersectWithAnyEdge(edge, edges);
                if (!intersectWithAnyEdge && differentThanOthers)
                {
                    return edge;
                }
            }

            // Any node can be connected with
            return null;
        }
    }
}