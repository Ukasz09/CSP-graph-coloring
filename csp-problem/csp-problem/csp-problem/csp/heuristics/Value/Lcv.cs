using System.Collections.Generic;
using System.Linq;

namespace csp_problem.csp.heuristics
{
    public class Lcv<V, D> : IValueOrderHeuristic<V, D>
    {
        public List<D> GetOrderedDomainValues(V variable, IEnumerable<D> domains, IAssignment<V, D> assignment)
        {
            var connectedVariables = assignment.GetConnectedVariables(variable);
            var domainsAndConsistentQty = new Dictionary<D, int>();
            foreach (var domainValue in domains)
            {
                var consistentQty = 0;
                foreach (var neighbourVariable in connectedVariables)
                {
                    var willBeConsistent = assignment.WillBeConsistent(neighbourVariable, domainValue);
                    if (willBeConsistent)
                    {
                        consistentQty++;
                    }
                }

                domainsAndConsistentQty[domainValue] = consistentQty;
            }

            var sortedDomainsAndConsistency = domainsAndConsistentQty
                .OrderByDescending(pair => pair.Value)
                .Select(pair => pair.Key)
                .ToList();
            return sortedDomainsAndConsistency;
        }
    }
}