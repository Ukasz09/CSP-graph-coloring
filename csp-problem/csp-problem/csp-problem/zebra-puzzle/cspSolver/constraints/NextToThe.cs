using System;
using System.Collections.Generic;
using csp_problem.csp;
using csp_problem.csp.constraints;

namespace csp_problem.cspSolver.constraints
{
    public class NextToThe : IConstraint<string, int>, IBinaryConstraint<string>
    {
        public ICollection<string> Variables { get; }
        public string GetVarA { get; }

        public string GetVarB { get; }

        public NextToThe(string varA, string varB)
        {
            Variables = new List<string> {varA, varB};
            GetVarA = varA;
            GetVarB = varB;
        }

        public IBinaryConstraint<string> Reverse()
        {
            return new NextToThe(GetVarB, GetVarA);
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
            return Math.Abs(varAHouseNumber - varBHouseNumber) == 1;
        }
    }
}