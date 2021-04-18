using csp_problem.csp;
using csp_problem.csp.constraints;

namespace csp_problem.cspSolver.constraints
{
    public class OtherValueAssigned : BinaryConstraint<string, int>
    {
        public OtherValueAssigned(string varA, string varB) : base(varA, varB)
        {
        }

        public override IBinaryConstraint<string, int> Reverse()
        {
            return new OtherValueAssigned(GetVarB, GetVarA);
        }

        public override bool IsEqualToVarB(string otherVar)
        {
            return GetVarB.Equals(otherVar);
        }

        public override bool IsSatisfied(int domainValueForVarA, int domainValueForVarB)
        {
            return domainValueForVarA != domainValueForVarB;
        }
    }
}