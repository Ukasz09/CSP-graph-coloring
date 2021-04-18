using System.Collections.Generic;
using System.Linq;

namespace csp_problem.csp.heuristics
{
    public class TrivialOrderValues<V, D> : IValueOrderHeuristic<V, D>
    {
        public List<D> GetOrderedDomainValues(V variable, IEnumerable<D> domains, IAssignment<V, D> assignment)
        {
            return domains.ToList();
        }
    }
}