using System;
using System.Collections.Generic;
using System.Diagnostics;
using csp_problem.csp.heuristics;

namespace csp_problem.csp.cspSolver
{
    public class BacktrackSolver<V, D> : ISolver<V, D>
    {
        public IValueOrderHeuristic<V, D> ValuesOrderHeuristic { get; }
        public IVariableHeuristic<V, D> VariableHeuristic { get; }
        public long ExecutionTimeInMs { get; private set; }
        public int VisitedNodesQty { get; private set; }
        public int FoundSolutionsQty { get; private set; }
        private long TimeoutExecutionTimeMs { get; }

        private readonly Stopwatch _watch = new Stopwatch();

        private readonly IList<IDictionary<V, D>> _solutions = new List<IDictionary<V, D>>();

        public BacktrackSolver(IValueOrderHeuristic<V, D> valueOrderHeuristic,
            IVariableHeuristic<V, D> variableOrderHeuristic, long timeoutExecutionTimeMs = 300000)
        {
            ValuesOrderHeuristic = valueOrderHeuristic;
            VariableHeuristic = variableOrderHeuristic;
            TimeoutExecutionTimeMs = timeoutExecutionTimeMs;
            ResetCalcStatistics();
        }

        public IDictionary<V, D> Solve(Csp<V, D> csp, IAssignment<V, D> assignment, bool withForwardChecking)
        {
            var solutions = SolveAll(csp, assignment, withForwardChecking);
            return solutions.Count > 0 ? solutions[0] : null;
        }

        public IList<IDictionary<V, D>> SolveAll(Csp<V, D> csp, IAssignment<V, D> assignment, bool withForwardChecking)
        {
            _watch.Start();
            ResetCalcStatistics();
            solveWithBacktracking(csp.Domains, csp, assignment, true, withForwardChecking);
            _watch.Stop();
            ExecutionTimeInMs = _watch.ElapsedMilliseconds;
            return _solutions;
        }

        private IAssignment<V, D> solveWithBacktracking(IDictionary<V, ICollection<D>> varDomains,
            Csp<V, D> csp, IAssignment<V, D> assignment, bool allSolutions, bool withForwardChecking
        )
        {
            if (_watch.ElapsedMilliseconds > TimeoutExecutionTimeMs)
            {
                throw new Exception($"Timed out - set to {TimeoutExecutionTimeMs} ms.");
            }

            if (assignment.AllVariablesAssigned())
            {
                return assignment;
            }

            var variable = VariableHeuristic.ChooseVariable(assignment, csp, varDomains);
            var orderedValues = ValuesOrderHeuristic.GetOrderedDomainValues(variable, varDomains[variable], assignment);
            foreach (var value in orderedValues)
            {
                VisitedNodesQty++;
                assignment.AssignVariable(variable, value);
                if (assignment.IsConsistent(variable))
                {
                    var newVarDomains = withForwardChecking
                        ? ForwardChecking<V, D>.ReduceDomains(varDomains, value, variable, assignment)
                        : varDomains;
                    var result = solveWithBacktracking
                        (newVarDomains, csp, assignment, allSolutions, withForwardChecking);
                    if (result != null)
                    {
                        FoundSolutionsQty++;
                        if (!allSolutions)
                        {
                            return result;
                        }

                        StoreResult(result);
                    }
                }
                else
                {
                    assignment.UnassignVariable(variable);
                }
            }

            return null;
        }

        private void StoreResult(IAssignment<V, D> result)
        {
            var assignedValuesCopy = new Dictionary<V, D>();
            var assignedValues = result.GetAssignedValueForAll();
            foreach (var (assignedVariable, assignedDomainValue) in assignedValues)
            {
                assignedValuesCopy[assignedVariable] = assignedDomainValue;
            }

            _solutions.Add(assignedValuesCopy);
        }

        private void ResetCalcStatistics()
        {
            ExecutionTimeInMs = 0;
            VisitedNodesQty = 0;
            FoundSolutionsQty = 0;
            _solutions.Clear();
        }
    }
}