using System.Collections.Generic;
using System.Linq;

namespace csp_problem.csp.heuristics
{
    public class Mrv<V, D> : IVariableHeuristic<V, D>
    {
        public V ChooseVariable(IAssignment<V, D> assignment, Csp<V, D> csp, IDictionary<V, ICollection<D>> varDomains)
        {
            var legalChoicesQty = int.MaxValue;
            V variableToReturn = default;
            foreach (var variable in varDomains.Keys)
            {
                if (!assignment.IsAssigned(variable))
                {
                    var domains = varDomains[variable];
                    var consistentQty = domains
                        .Select(domain => assignment.WillBeConsistent(variable, domain))
                        .Count(willBeConsistent => willBeConsistent);
                    if (consistentQty < legalChoicesQty)
                    {
                        legalChoicesQty = consistentQty;
                        variableToReturn = variable;
                    }
                }
            }

            return variableToReturn;
        }
    }
}