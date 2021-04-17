using System.Collections.Generic;
using csp_problem.csp.heuristics;

namespace csp_problem.csp.cspSolver
{
    public interface ISolver<V, D>
    {
        IValueOrderHeuristic<V, D> ValuesOrderHeuristic { get; }
        IVariableHeuristic<V, D> VariableHeuristic { get; }
        IAssignment<V, D> Solve(Csp<V, D> csp, IAssignment<V, D> assignment);
        IList<IAssignment<V, D>> SolveAll(Csp<V, D> csp, IAssignment<V, D> assignment);
        long ExecutionTimeInMs { get; }
        int VisitedNodesQty { get; }
    }
}