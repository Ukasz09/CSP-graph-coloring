namespace csp_problem.csp.heuristics
{
    public interface IVariableHeuristic<V, D>
    {
        V ChooseVariable(IAssignment<V, D> assignment, Csp<V, D> csp);
    }
}