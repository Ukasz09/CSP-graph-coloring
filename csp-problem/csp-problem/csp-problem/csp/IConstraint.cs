using System.Collections.Generic;

namespace csp_problem.csp
{
    public interface IConstraint<V, D>
    {
        /**
         * Variables that are affected by constraint
         */
        ICollection<V> Variables { get; }

        /**
         * Determine if constraint affect given variable
         */
        bool Affects(V variable);

        /**
         * Determine if constraint is satisfied on given value
         */
        bool IsSatisfied(D value, IAssignment<V, D> inAssignment);
    }
}