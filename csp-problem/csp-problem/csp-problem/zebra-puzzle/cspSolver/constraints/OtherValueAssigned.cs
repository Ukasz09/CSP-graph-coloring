using System.Collections.Generic;
using csp_problem.csp;
using csp_problem.csp.constraints;

namespace csp_problem.cspSolver.constraints
{
    public class OtherValueAssigned : IBinaryConstraint<string, int>
    {
        public ICollection<string> Variables { get; }
        public string GetVarA { get; }

        public string GetVarB { get; }

        public OtherValueAssigned(string varA, string varB)
        {
            Variables = new List<string> {varA, varB};
            GetVarA = varA;
            GetVarB = varB;
        }

        public IBinaryConstraint<string, int> Reverse()
        {
            return new OtherValueAssigned(GetVarB, GetVarA);
        }

        public bool IsEqualToVarB(string otherVar)
        {
            return GetVarB.Equals(otherVar);
        }

        public bool Affects(string variable)
        {
            return Variables.Contains(variable);
        }

        public bool IsSatisfied(IAssignment<string, int> inAssignment)
        {
            var varAIsAssigned = inAssignment.IsAssigned(GetVarA);
            var varBIsAssigned = inAssignment.IsAssigned(GetVarB);
            if (!varAIsAssigned || !varBIsAssigned)
            {
                return true;
            }

            var varAHouseNumber = inAssignment.GetAssignedValue(GetVarA);
            var varBHouseNumber = inAssignment.GetAssignedValue(GetVarB);
            return IsSatisfied(varAHouseNumber, varBHouseNumber);
        }

        public bool IsSatisfied(int domainValueForVarA, int domainValueForVarB)
        {
            return domainValueForVarA != domainValueForVarB;
        }
    }
}