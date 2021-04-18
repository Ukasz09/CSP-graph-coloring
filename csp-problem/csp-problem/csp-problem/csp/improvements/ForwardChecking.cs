using System.Collections.Generic;
using System.Linq;

namespace csp_problem.csp
{
    public static class ForwardChecking<V, D>
    {
        public static Dictionary<V, ICollection<D>> ReduceDomains(
            IDictionary<V, ICollection<D>> varDomains, D assignedValue, V assignedVariable,
            IAssignment<V, D> assignment)
        {
            var connectedVariables = assignment.GetConnectedVariables(assignedVariable);
            var clonedVarDomains = CloneVarDomains(varDomains);

            // Remove inconsistent with the chosen value
            foreach (var variable in connectedVariables)
            {
                var willBeConsistent = assignment.WillBeConsistent(variable, assignedValue);
                if (!willBeConsistent)
                {
                    clonedVarDomains[variable].Remove(assignedValue);
                }
            }

            return clonedVarDomains;
        }

        private static Dictionary<V, ICollection<D>> CloneVarDomains(IDictionary<V, ICollection<D>> varDomains)
        {
            var clonedVarDomains = new Dictionary<V, ICollection<D>>();
            foreach (var variable in varDomains.Keys)
            {
                clonedVarDomains[variable] = varDomains[variable].ToList();
            }

            return clonedVarDomains;
        }
    }
}