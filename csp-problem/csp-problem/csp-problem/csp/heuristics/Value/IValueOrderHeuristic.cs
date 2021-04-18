using System.Collections.Generic;

namespace csp_problem.csp.heuristics
{
    public interface IValueOrderHeuristic<V, D>
    {
        List<D> GetOrderedDomainValues(IEnumerable<D> domains, IAssignment<V, D> assignment);
    }
}