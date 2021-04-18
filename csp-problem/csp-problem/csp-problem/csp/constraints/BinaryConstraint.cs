using System.Collections.Generic;

namespace csp_problem.csp.constraints
{
    public abstract class BinaryConstraint<V, D> : IBinaryConstraint<V, D>
    {
        public ICollection<V> Variables { get; }
        public V GetVarA { get; }
        public V GetVarB { get; }

        protected BinaryConstraint(V varA, V varB)
        {
            Variables = new List<V> {varA, varB};
            GetVarA = varA;
            GetVarB = varB;
        }

        public abstract IBinaryConstraint<V, D> Reverse();

        public abstract bool IsEqualToVarB(V otherVar);

        public bool Affects(V variable)
        {
            return Variables.Contains(variable);
        }

        public bool IsSatisfied(IAssignment<V, D> inAssignment)
        {
            var varAIsAssigned = inAssignment.IsAssigned(GetVarA);
            var varBIsAssigned = inAssignment.IsAssigned(GetVarB);
            if (!varAIsAssigned || !varBIsAssigned)
            {
                return true;
            }

            var varAValue = inAssignment.GetAssignedValue(GetVarA);
            var varBValue = inAssignment.GetAssignedValue(GetVarB);
            return IsSatisfied(varAValue, varBValue);
        }

        public abstract bool IsSatisfied(D domainValueForVarA, D domainValueForVarB);
    }
}