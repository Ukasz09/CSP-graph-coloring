using System;

namespace csp_problem
{
    public class Edge
    {
        public Node startNode { get; }
        public Node endNode { get; }

        public Edge(Node startNode, Node endNode)
        {
            this.startNode = startNode;
            this.endNode = endNode;
        }

        private bool Equals(Edge other)
        {
            return
                (other.startNode.Equals(startNode) && other.endNode.Equals(endNode)) ||
                (other.startNode.Equals(endNode) && other.endNode.Equals(startNode));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Edge) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(startNode, endNode);
        }

        public override string ToString()
        {
            return $"[{startNode},{endNode}]";
        }
    }
}