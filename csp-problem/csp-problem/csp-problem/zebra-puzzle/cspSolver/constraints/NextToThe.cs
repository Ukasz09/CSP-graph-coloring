using System;
using System.Collections.Generic;
using csp_problem.csp;
using csp_problem.csp.constraints;

namespace csp_problem.cspSolver.constraints
{
    public class NextToThe : BinaryConstraint<string, int>
    {
        public NextToThe(string varA, string varB) : base(varA, varB)
        {
        }

        public override IBinaryConstraint<string, int> Reverse()
        {
            return new NextToThe(GetVarB, GetVarA);
        }

        public override bool IsEqualToVarB(string otherVar)
        {
            return GetVarB.Equals(otherVar);
        }

        public override bool IsSatisfied(int domainValueForVarA, int domainValueForVarB)
        {
            return Math.Abs(domainValueForVarA - domainValueForVarB) == 1;
        }
    }
}