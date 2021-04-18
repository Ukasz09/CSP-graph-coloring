namespace csp_problem.csp.constraints
{
    public interface IUnaryConstraint<V, D> : IConstraint<V, D>
    {
        V GetVar { get; }
        D GetValue { get; }
        
        bool IsSatisfied(D domainValue);
    }
}