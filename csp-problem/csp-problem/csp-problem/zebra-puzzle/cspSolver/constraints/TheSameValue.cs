using System.Collections.Generic;
using csp_problem.csp;

namespace csp_problem.cspSolver.constraints
{
    public class TheSameValueAssigned : IConstraint<string, int>
    {
        public ICollection<string> Variables { get; }
        private readonly string _varA;
        private readonly string _varB;

        public TheSameValueAssigned(string varA, string varB)
        {
            Variables = new List<string> {varA, varB};
            _varA = varA;
            _varB = varB;
        }

        public bool Affects(string variable)
        {
            return Variables.Contains(variable);
        }

        public bool IsSatisfied(IAssignment<string, int> inAssignment)
        {
            var varAIsAssigned = inAssignment.IsAssigned(_varA);
            var varBIsAssigned = inAssignment.IsAssigned(_varB);
            if (!varAIsAssigned || !varBIsAssigned)
            {
                return true;
            }

            var varAHouseNumber = inAssignment.GetAssignedValue(_varA);
            var varBHouseNumber = inAssignment.GetAssignedValue(_varB);
            return varAHouseNumber == varBHouseNumber;
        }
    }
}