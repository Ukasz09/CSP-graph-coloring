using System.Linq;

namespace csp_problem.csp.heuristics
{
    public class FirstVariableHeuristic<V, D> : IVariableHeuristic<V, D>
    {
        public V ChooseVariable(IAssignment<V, D> assignment, Csp<V, D> csp)
        {
            return csp.Variables.FirstOrDefault(variable => !assignment.VariableIsAssigned(variable));
        }
    }
}