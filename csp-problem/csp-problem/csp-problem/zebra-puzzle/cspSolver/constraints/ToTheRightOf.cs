using csp_problem.csp.constraints;

namespace csp_problem.cspSolver.constraints
{
    public class ToTheRightOf : BinaryConstraint<string, int>
    {
        public ToTheRightOf(string varA, string varB) : base(varA, varB)
        {
        }

        public override IBinaryConstraint<string, int> Reverse()
        {
            return new ToTheLeftOf(GetVarB, GetVarA);
        }

        public override bool IsEqualToVarB(string otherVar)
        {
            return GetVarB.Equals(otherVar);
        }

        public override bool IsSatisfied(int domainValueForVarA, int domainValueForVarB)
        {
            return domainValueForVarA == domainValueForVarB + 1;
        }
    }
}