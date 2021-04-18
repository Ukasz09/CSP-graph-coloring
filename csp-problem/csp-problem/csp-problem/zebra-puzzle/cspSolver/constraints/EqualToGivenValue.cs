using csp_problem.csp.constraints;

namespace csp_problem.cspSolver.constraints
{
    public class ValueEqualToGiven : UnaryConstraint<string, int>
    {
        public ValueEqualToGiven(string varA, int value) : base(varA, value)
        {
        }

        public override bool IsSatisfied(int domainValue)
        {
            return GetValue == domainValue;
        }
    }
}