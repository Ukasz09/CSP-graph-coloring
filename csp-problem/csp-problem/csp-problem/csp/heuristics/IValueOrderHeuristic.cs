using System.Collections.Generic;

namespace csp_problem.csp.heuristics
{
    public interface IValueOrderHeuristic<V, D>
    {
        IEnumerable<D> GetOrderedDomainValues(IAssignment<V, D> assignment, Csp<V, D> csp, V variable);
    }
}