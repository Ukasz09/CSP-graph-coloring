namespace csp_problem.csp.constraints
{
    public interface IBinaryConstraint<V, D> : IConstraint<V, D>
    {
        V GetVarA { get; }
        V GetVarB { get; }
        IBinaryConstraint<V, D> Reverse();
        bool IsEqualToVarB(V otherVar);
        bool IsSatisfied(D domainValueForVarA, D domainValueForVarB);
    }
}