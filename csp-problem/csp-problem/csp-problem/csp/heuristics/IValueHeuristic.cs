using System.Collections.Generic;

namespace csp_problem.csp.heuristics
{
    public interface IValueHeuristic<V, D>
    {
        IEnumerable<int> GetOrderedDomainValues(IAssignment<V, D> assignment, Csp<V, D> csp, V variable);
    }
}