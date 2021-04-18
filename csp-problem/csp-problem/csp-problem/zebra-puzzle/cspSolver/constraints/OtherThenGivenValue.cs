using System.Collections.Generic;
using csp_problem.csp;
using csp_problem.csp.constraints;

namespace csp_problem.cspSolver.constraints
{
    public class OtherThenGivenValue : IUnaryConstraint<string, int>
    {
        public ICollection<string> Variables { get; }
        public string GetVar { get; }
        public int GetValue { get; }

        public OtherThenGivenValue(string varA, int value)
        {
            Variables = new List<string> {varA};
            GetVar = varA;
            GetValue = value;
        }

        public bool Affects(string variable)
        {
            return Variables.Contains(variable);
        }

        public bool IsSatisfied(IAssignment<string, int> inAssignment)
        {
            var varAIsAssigned = inAssignment.IsAssigned(GetVar);
            if (!varAIsAssigned)
            {
                return true;
            }

            var varAHouseNumber = inAssignment.GetAssignedValue(GetVar);
            return IsSatisfied(varAHouseNumber);
        }

        public bool IsSatisfied(int domainValue)
        {
            return GetValue != domainValue;
        }
    }
}