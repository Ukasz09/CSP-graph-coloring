using System.Collections.Generic;

namespace csp_problem.csp.heuristics
{
    public class TrivialOrderValues<V, D> : IValueOrderHeuristic<V, D>
    {
        public IEnumerable<D> GetOrderedDomainValues(IAssignment<V, D> assignment, Csp<V, D> csp, V variable)
        {
            return assignment.GetDomain(variable);
        }
    }
}