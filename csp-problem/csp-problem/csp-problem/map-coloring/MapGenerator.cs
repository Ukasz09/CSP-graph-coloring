using System;
using System.Collections.Generic;
using System.Linq;

namespace csp_problem
{
    public class MapGenerator
    {
        private readonly Random randGen;

        public MapGenerator()
        {
            randGen = new Random();
        }

        public MapGraph GenerateMap(int nodesQty, int mapWidth, int mapHeight)
        {
            var nodes = DrawNodesWithoutRepeating(nodesQty, mapWidth, mapHeight);
            var nodesCopy = MapUtils.CloneNodes(nodes);
            var edges = GenerateEdges(nodesCopy);
            var map = new MapGraph(edges, nodes);
            return map;
        }

        private List<Node> DrawNodesWithoutRepeating(int qty, int mapWidth, int mapHeight)
        {
            if (mapWidth < 0 || mapHeight < 0 || qty > mapWidth * mapWidth)
            {
                throw new ArgumentException(
                    $"Given nodes qty = {qty} is bigger than max possible unique nodes (= {mapWidth * mapHeight})" +
                    $" that can be created for this map (size = {mapWidth}x{mapHeight})");
            }

            var nodes = new List<Node>(qty);
            var i = 0;
            do
            {
                var randXIndex = randGen.Next(mapWidth);
                var randYIndex = randGen.Next(mapHeight);
                var node = new Node(randXIndex, randYIndex);

                // if found unique then add
                if (!nodes.Contains(node))
                {
                    nodes.Add(node);
                    i++;
                }
            } while (i < qty);

            return nodes;
        }

        private List<Edge> GenerateEdges(IList<Node> nodes)
        {
            var edges = new List<Edge>();
            while (nodes.Count > 1)
            {
                var nodesCopy = MapUtils.CloneNodes(nodes);

                // Choose started node
                var startedNode = nodesCopy[randGen.Next(nodesCopy.Count)];
                nodesCopy.Remove(startedNode);

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

            // Not exist any not intersected edge
            return edges;
        }

        private Edge GenerateNotIntersectedEdge(Node startedNode, ICollection<Node> availableNodes, List<Edge> edges)
        {
            while (availableNodes.Count > 0)
            {
                // Generate edge
                var closestNode = GetClosestNode(startedNode, availableNodes);
                availableNodes.Remove(closestNode);
                var edge = new Edge(startedNode, closestNode);
                var differentThanOthers = !edges.Exists(e => e.Equals(edge));
                var intersectWithAnyEdge = IntersectWithAnyEdge(edge, edges);
                if (!intersectWithAnyEdge && differentThanOthers)
                {
                    return edge;
                }
            }

            // Any node can be connected with
            return null;
        }

        private bool IntersectWithAnyEdge(Edge edge, List<Edge> edges)
        {
            // Check if intersect with any edge
            return edges.Select(e => MapUtils.EgdesIntersect(edge, e)).Any(intersect => intersect);
        }

        private double SegmentWidth(Node a, Node b)
        {
            return Math.Sqrt(Math.Pow((b.X - a.X), 2) + Math.Pow(b.Y - a.Y, 2));
        }

        private Node GetClosestNode(Node node, IEnumerable<Node> nodes)
        {
            var minDistance = double.MaxValue;
            Node closestNode = null;
            foreach (var n in nodes)
            {
                var distance = SegmentWidth(node, n);
                if (minDistance > distance)
                {
                    minDistance = distance;
                    closestNode = n;
                }
            }

            return closestNode;
        }
    }
}