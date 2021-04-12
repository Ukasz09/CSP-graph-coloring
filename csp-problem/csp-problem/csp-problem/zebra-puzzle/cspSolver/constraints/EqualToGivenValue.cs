using System.Collections.Generic;
using csp_problem.csp;

namespace csp_problem.cspSolver.constraints
{
    public class ValueEqualToGiven : IConstraint<string, int>
    {
        public ICollection<string> Variables { get; }
        private readonly string _varA;
        private readonly int _givenValue;

        public ValueEqualToGiven(string varA, int value)
        {
            Variables = new List<string> {varA};
            _varA = varA;
            _givenValue = value;
        }

        public bool Affects(string variable)
        {
            return Variables.Contains(variable);
        }

        public bool IsSatisfied(IAssignment<string, int> inAssignment)
        {
            var varAIsAssigned = inAssignment.IsAssigned(_varA);
            if (!varAIsAssigned)
            {
                return true;
            }

            var varAHouseNumber = inAssignment.GetAssignedValue(_varA);
            return varAHouseNumber == _givenValue;
        }
    }
}