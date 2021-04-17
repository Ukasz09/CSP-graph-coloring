using System.Collections.Generic;
using csp_problem.csp.constraints;

namespace csp_problem.csp
{
    public class Ac3<V>
    {
        public Queue<IBinaryConstraint<V>> GenerateArcs(IEnumerable<IBinaryConstraint<V>> constraints)
        {
            var arcs = new Queue<IBinaryConstraint<V>>();
            foreach (var constraint in constraints)
            {
                var reversedConstraint = constraint.Reverse();
                arcs.Enqueue(constraint);
                arcs.Enqueue(reversedConstraint);
            }
            return arcs;
        }

      
    }
}