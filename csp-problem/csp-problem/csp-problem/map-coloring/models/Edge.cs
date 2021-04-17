using System;

namespace csp_problem
{
    public class Edge
    {
        public Node StartNode { get; }
        public Node EndNode { get; }

        public Edge(Node startNode, Node endNode)
        {
            StartNode = startNode;
            EndNode = endNode;
        }

        private bool Equals(Edge other)
        {
            return
                other.StartNode.Equals(StartNode) && other.EndNode.Equals(EndNode) ||
                other.StartNode.Equals(EndNode) && other.EndNode.Equals(StartNode);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Edge) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StartNode, EndNode);
        }

        public override string ToString()
        {
            return $"[{StartNode},{EndNode}]";
        }
    }
}