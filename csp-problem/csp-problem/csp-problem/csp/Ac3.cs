using System.Collections.Generic;
using System.Linq;
using csp_problem.csp.constraints;

namespace csp_problem.csp
{
    public class Ac3<V, D>
    {
        private static Queue<IBinaryConstraint<V, D>> GenerateArcs(IEnumerable<IBinaryConstraint<V, D>> constraints)
        {
            var arcs = new Queue<IBinaryConstraint<V, D>>();
            foreach (var constraint in constraints)
            {
                var reversedConstraint = constraint.Reverse();
                arcs.Enqueue(constraint);
                arcs.Enqueue(reversedConstraint);
            }

            return arcs;
        }

        public static void ReduceDomains(IEnumerable<IBinaryConstraint<V, D>> constraints, Csp<V, D> csp)
        {
            var arcsList = GenerateArcs(constraints).ToList();
            var agenda = GenerateArcs(constraints);
            while (agenda.Count > 0)
            {
                var checkedArc = agenda.Dequeue();
                RemoveInconsistent(checkedArc, csp, out var atLeastOneRemoved);
                if (atLeastOneRemoved)
                {
                    var affectedArcs = arcsList.FindAll(arc => arc.IsEqualToVarB(arc.GetVarA));
                    affectedArcs.ForEach(arc => agenda.Enqueue(arc));
                }
            }
        }

        /**
         * Remove inconsistent domain values from csp variables 
         */
        public static void RemoveInconsistent(IBinaryConstraint<V, D> arc, Csp<V, D> csp, out bool atLeastOneRemoved)
        {
            atLeastOneRemoved = false;
            var domainsA = csp.Domains[arc.GetVarA];
            var domainsB = csp.Domains[arc.GetVarB];
            foreach (var domainValueA in domainsA)
            {
                if (CheckInconsistent(domainValueA, domainsB, arc))
                {
                    domainsA.Remove(domainValueA);
                    atLeastOneRemoved = true;
                }
            }
        }

        private static bool CheckInconsistent(D domainValueA, IEnumerable<D> domainsB,
            IBinaryConstraint<V, D> constraint)
        {
            foreach (var domainValueB in domainsB)
            {
                // Not found inconsistency - at least one value satisfy constraint
                if (constraint.IsSatisfied(domainValueA, domainValueB))
                {
                    return false;
                }
            }

            return true;
        }
    }
}