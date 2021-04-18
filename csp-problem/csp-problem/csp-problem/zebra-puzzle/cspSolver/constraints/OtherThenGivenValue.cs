using csp_problem.csp.constraints;

namespace csp_problem.cspSolver.constraints
{
    public class OtherThenGivenValue : UnaryConstraint<string, int>
    {
        public OtherThenGivenValue(string varA, int value) : base(varA, value)
        {
        }

        public override bool IsSatisfied(int domainValue)
        {
            return GetValue != domainValue;
        }
    }
}