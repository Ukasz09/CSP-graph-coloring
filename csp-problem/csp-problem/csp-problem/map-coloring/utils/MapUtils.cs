using System;
using System.Collections.Generic;
using System.Linq;

namespace csp_problem
{
    public class MapUtils
    {
        // The main function that returns true if line segment 'p1q1'
        // and 'p2q2' intersect.
        public static bool EgdesIntersect(Edge e1, Edge e2)
        {
            var nodes = new[] {e1.startNode, e1.endNode, e2.startNode, e2.endNode};
            var distinctNodes = nodes.Distinct().ToArray();
            var atLeastOneCommonNode = distinctNodes.Length != nodes.Length;
            // at least one node is common
            if (atLeastOneCommonNode)
            {
                // Are the same edges
                return distinctNodes.Length < 3 || AreCollinear(distinctNodes[0], distinctNodes[1], distinctNodes[2]);
            }

            var p1 = e1.startNode;
            var q1 = e1.endNode;
            var p2 = e2.startNode;
            var q2 = e2.endNode;

            // Find the four orientations needed for general and
            // special cases
            var o1 = Orientation(p1, q1, p2);
            var o2 = Orientation(p1, q1, q2);
            var o3 = Orientation(p2, q2, p1);
            var o4 = Orientation(p2, q2, q1);

            // General case
            if (o1 != o2 && o3 != o4)
                return true;

            // Special Cases
            // p1, q1 and p2 are collinear and p2 lies on segment p1q1
            if (o1 == 0 && OnSegment(p1, p2, q1))
            {
                return true;
            }

            // p1, q1 and q2 are collinear and q2 lies on segment p1q1
            if (o2 == 0 && OnSegment(p1, q2, q1))
            {
                return true;
            }

            // p2, q2 and p1 are collinear and p1 lies on segment p2q2
            if (o3 == 0 && OnSegment(p2, p1, q2))
            {
                return true;
            }

            // p2, q2 and q1 are collinear and q1 lies on segment p2q2
            // or doesn't fall in anY of the above cases
            return o4 == 0 && OnSegment(p2, q1, q2);
        }

        // Given three collinear points p, q, r, the function checks if
        // point q lies on line segment 'pr'
        private static bool OnSegment(Node p, Node q, Node r)
        {
            return q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) &&
                   q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y);
        }

        // To find orientation of ordered triplet (p, q, r).
        // The function returns following values
        // 0 --> p, q and r are collinear
        // 1 --> Clockwise
        // 2 --> Counterclockwise
        private static int Orientation(Node p, Node q, Node r)
        {
            // See https://www.geeksforgeeks.org/orientation-3-ordered-points/
            // for details of below formula.
            var val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);
            // collinear
            if (val == 0)
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
    }
}