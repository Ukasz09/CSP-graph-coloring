namespace csp_problem.csp.constraints
{
    public interface IBinaryConstraint<V, D> : IConstraint<V, D>
    {
        V GetVarA { get; }
        V GetVarB { get; }
        IBinaryConstraint<V, D> Reverse();

        /**
         * Workaround for error about checking equality on virtual, generic types, implicitly casted to object
         */
        bool IsEqualToVarB(V otherVar);

        bool IsSatisfied(D domainValueForVarA, D domainValueForVarB);
    }
}