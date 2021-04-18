using System.Collections.Generic;

namespace csp_problem.csp.constraints
{
    public abstract class UnaryConstraint<V, D> : IUnaryConstraint<V, D>
    {
        public ICollection<V> Variables { get; }
        public V GetVar { get; }
        public D GetValue { get; }

        protected UnaryConstraint(V varA, D value)
        {
            Variables = new List<V> {varA};
            GetVar = varA;
            GetValue = value;
        }

        public bool Affects(V variable)
        {
            return Variables.Contains(variable);
        }

        public bool IsSatisfied(IAssignment<V, D> inAssignment)
        {
            var varAIsAssigned = inAssignment.IsAssigned(GetVar);
            if (!varAIsAssigned)
            {
                return true;
            }

            var varAHouseNumber = inAssignment.GetAssignedValue(GetVar);
            return IsSatisfied(varAHouseNumber);
        }

        public abstract bool IsSatisfied(D domainValue);
    }
}