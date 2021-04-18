using System.Collections.Generic;
using System.Linq;

namespace csp_problem.csp.constraints
{
    public class AllDifferentConstraint<V, D> : IConstraint<V, D>
    {
        public ICollection<V> Variables { get; }

        public AllDifferentConstraint(ICollection<V> variables)
        {
            Variables = variables;
        }

        public bool Affects(V variable)
        {
            return Variables.Contains(variable);
        }

        public bool IsSatisfied(IAssignment<V, D> inAssignment)
        {
            var assignedValues = new List<D>(Variables.Count);
            foreach (var var in Variables)
            {
                if (inAssignment.IsAssigned(var))
                {
                    var assignedValue = inAssignment.GetAssignedValue(var);
                    assignedValues.Add(assignedValue);
                }
            }

            var allValuesUnique = assignedValues.Distinct().Count() == assignedValues.Count;
            return allValuesUnique;
        }
    }
}