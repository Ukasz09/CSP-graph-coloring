using System.Collections.Generic;
using csp_problem.csp.heuristics;

namespace csp_problem.csp.cspSolver
{
    public interface ISolver<V, D>
    {
        IValueOrderHeuristic<V, D> ValuesOrderHeuristic { get; }
        IVariableHeuristic<V, D> VariableHeuristic { get; }
        IDictionary<V, D> Solve(Csp<V, D> csp, IAssignment<V, D> assignment, bool withForwardChecking);
        IList<IDictionary<V, D>> SolveAll(Csp<V, D> csp, IAssignment<V, D> assignment, bool withForwardChecking);
        long ExecutionTimeInMs { get; }
        long SearchTimeTillFstSolutionInMs { get; }
        int TotalVisitedNodesQty { get; }
        int VisitedNodesQtyTillFstSolution { get; }
        int FoundSolutionsQty { get; }
    }
}