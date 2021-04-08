using System;
using System.Collections.Generic;
using System.Linq;

namespace csp_problem
{
    public static class GraphUtils
    {
        public static bool EgdesIntersect(Edge e1, Edge e2)
        {
            if (EdgesHaveCommonNodeAndAreIntersected(e1, e2))
            {
                return true;
            }

            var e1StartNode = e1.startNode;
            var e1EndNode = e1.endNode;
            var e2StartNode = e2.startNode;
            var e2EndNode = e2.endNode;

            // Find the four orientations needed for general and special cases
            var o1 = Orientation(e1StartNode, e1EndNode, e2StartNode);
            var o2 = Orientation(e1StartNode, e1EndNode, e2EndNode);
            var o3 = Orientation(e2StartNode, e2EndNode, e1StartNode);
            var o4 = Orientation(e2StartNode, e2EndNode, e1EndNode);

            // General case
            if (o1 != o2 && o3 != o4)
                return true;

            // Special Cases
            // p1, q1 and p2 are collinear and p2 lies on segment p1q1
            if (o1 == 0 && OnSegment(e1StartNode, e2StartNode, e1EndNode))
            {
                return true;
            }

            // p1, q1 and q2 are collinear and q2 lies on segment p1q1
            if (o2 == 0 && OnSegment(e1StartNode, e2EndNode, e1EndNode))
            {
                return true;
            }

            // p2, q2 and p1 are collinear and p1 lies on segment p2q2
            if (o3 == 0 && OnSegment(e2StartNode, e1StartNode, e2EndNode))
            {
                return true;
            }

            // p2, q2 and q1 are collinear and q1 lies on segment p2q2
            return o4 == 0 && OnSegment(e2StartNode, e1EndNode, e2EndNode);
        }

        private static bool EdgesHaveCommonNodeAndAreIntersected(Edge e1, Edge e2)
        {
            var nodes = new[] {e1.startNode, e1.endNode, e2.startNode, e2.endNode};
            var distinctNodes = nodes.Distinct().ToArray();
            var atLeastOneCommonNode = distinctNodes.Length != nodes.Length;
            if (atLeastOneCommonNode)
            {
                // Are the same edges
                var areTheSameEdge = distinctNodes.Length < 3;
                return areTheSameEdge || AreCollinear(distinctNodes[0], distinctNodes[1], distinctNodes[2]);
            }

            return false;
        }

        // Given three collinear points edgeStartNode, checkedNode, edgeEndNode, the function checks if point q lies on
        // line segment 'edgeStartNode-edgeEndNode'
        private static bool OnSegment(Node edgeStartNode, Node checkedNode, Node edgeEndNode)
        {
            return checkedNode.X <= Math.Max(edgeStartNode.X, edgeEndNode.X) &&
                   checkedNode.X >= Math.Min(edgeStartNode.X, edgeEndNode.X) &&
                   checkedNode.Y <= Math.Max(edgeStartNode.Y, edgeEndNode.Y) &&
                   checkedNode.Y >= Math.Min(edgeStartNode.Y, edgeEndNode.Y);
        }

        // To find orientation of ordered triplet (p, q, r).
        // The function returns following values
        // 0 --> p, q and r are collinear
        // 1 --> Clockwise
        // 2 --> Counterclockwise
        private static int Orientation(Node p, Node q, Node r)
        {
            // See https://www.geeksforgeeks.org/orientation-3-ordered-points/ for details of below formula.
            var val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);
            var areCollinear = val == 0;
            if (areCollinear)
            {
                return 0;
            }

            // clock or counter-clock wise
            return (val > 0) ? 1 : 2;
        }

        private static bool AreCollinear(Node p, Node q, Node r)
        {
            return Orientation(p, q, r) == 0;
        }

        public static List<Node> CloneNodes(IEnumerable<Node> nodes)
        {
            return nodes.Select(n => new Node(n.X, n.Y)).ToList();
        }

        public static List<Node> GenerateUniqueNodes(int qty, int mapWidth, int mapHeight, Random randGen)
        {
            var incorrectParameters = mapWidth < 0 || mapHeight < 0 || qty > mapWidth * mapWidth;
            if (incorrectParameters)
            {
                throw new ArgumentException(
                    $"Given nodes qty = {qty} is bigger than max possible unique nodes (= {mapWidth * mapHeight})" +
                    $" that can be created for this map (size = {mapWidth}x{mapHeight})");
            }

            var nodes = new List<Node>(qty);
            var i = 0;
            do
            {
                var node = GenerateRandomNode(mapWidth, mapHeight, randGen);
                var isUnique = !nodes.Contains(node);
                if (isUnique)
                {
                    nodes.Add(node);
                    i++;
                }
            } while (i < qty);

            return nodes;
        }

        private static Node GenerateRandomNode(int maxX, int maxY, Random randGen)
        {
            var randXIndex = randGen.Next(maxX);
            var randYIndex = randGen.Next(maxY);
            var node = new Node(randXIndex, randYIndex);
            return node;
        }

        public static bool IntersectWithAnyEdge(Edge edge, IEnumerable<Edge> edges)
        {
            return edges.Select(e => EgdesIntersect(edge, e)).Any(intersect => intersect);
        }

        public static double SegmentWidth(Node a, Node b)
        {
            return Math.Sqrt(Math.Pow((b.X - a.X), 2) + Math.Pow(b.Y - a.Y, 2));
        }

        public static Node GetClosestNode(Node node, IEnumerable<Node> nodes)
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