namespace csp_problem.csp.constraints
{
    public interface IBinaryConstraint<V>
    {
        V GetVarA { get; }
        V GetVarB { get; }
        IBinaryConstraint<V> Reverse();
    }
}