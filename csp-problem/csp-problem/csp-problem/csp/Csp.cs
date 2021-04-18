using System.Collections.Generic;
using System.Linq;

namespace csp_problem.csp
{
    public class Csp<V, D>
    {
        public List<IConstraint<V, D>> Constraints { get; }

        public IDictionary<V, ICollection<D>> Domains { get; }

        public ICollection<V> Variables => Domains.Keys;

        public IDictionary<V, IList<IConstraint<V, D>>> VariableConstraints { get; }

        public Csp(IDictionary<V, ICollection<D>> domains, List<IConstraint<V, D>> constraints)
        {
            Domains = domains;
            Constraints = constraints;
            VariableConstraints = GetVariableConstraints(constraints);
        }

        public Csp(IDictionary<V, ICollection<D>> domains, List<IConstraint<V, D>> constraints,
            IDictionary<V, IList<IConstraint<V, D>>> variableConstraints)
        {
            Domains = domains;
            Constraints = constraints;
            VariableConstraints = variableConstraints;
        }

        private static IDictionary<V, IList<IConstraint<V, D>>> GetVariableConstraints(
            IEnumerable<IConstraint<V, D>> constraints)
        {
            var variableConstraints = new Dictionary<V, IList<IConstraint<V, D>>>();
            foreach (var constraint in constraints)
            {
                foreach (var variable in constraint.Variables)
                {
                    if (!variableConstraints.ContainsKey(variable))
                    {
                        variableConstraints[variable] = new List<IConstraint<V, D>>();
                    }

                    variableConstraints[variable].Add(constraint);
                }
            }

            return variableConstraints;
        }
    }
}