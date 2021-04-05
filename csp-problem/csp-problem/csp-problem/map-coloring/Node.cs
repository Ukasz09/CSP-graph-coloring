using System;

namespace csp_problem
{
    public class Node
    {
        public int X { get; }
        public int Y { get; }

        public Node(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"[{X.ToString()},{Y.ToString()}]";
        }

        private bool Equals(Node other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Node) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}