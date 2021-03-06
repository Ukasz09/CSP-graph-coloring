using System;
using System.Collections.Generic;
using System.Linq;

namespace csp_problem.csp.heuristics
{
    public class RandomVariableHeuristic<V, D> : IVariableHeuristic<V, D>
    {
        public V ChooseVariable(IAssignment<V, D> assignment, Csp<V, D> csp, IDictionary<V, ICollection<D>> varDomains)
        {
            return csp.Variables
                .Where(variable => !assignment.IsAssigned(variable))
                .OrderBy(variable => Guid.NewGuid())
                .FirstOrDefault();
        }
    }
}