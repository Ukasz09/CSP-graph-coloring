using System.Collections.Generic;

namespace csp_problem.csp.heuristics
{
    public interface IValueOrderHeuristic<V, D>
    {
        List<D> GetOrderedDomainValues(V variable, IEnumerable<D> domains, IAssignment<V, D> assignment);
    }
}