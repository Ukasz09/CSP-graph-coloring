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
        private long TimeoutExecutionTimeMs { get; }

        private readonly Stopwatch _watch = new Stopwatch();
        private IList<IAssignment<V, D>> solutions = new List<IAssignment<V, D>>();

        public BacktrackSolver(IValueOrderHeuristic<V, D> valueOrderHeuristic,
            IVariableHeuristic<V, D> variableOrderHeuristic, long timeoutExecutionTimeMs = 300000)
        {
            ValuesOrderHeuristic = valueOrderHeuristic;
            VariableHeuristic = variableOrderHeuristic;
            ExecutionTimeInMs = 0;
            VisitedNodesQty = 0;
            TimeoutExecutionTimeMs = timeoutExecutionTimeMs;
        }

        public IAssignment<V, D> Solve(Csp<V, D> csp, IAssignment<V, D> assignment)
        {
            _watch.Start();
            ExecutionTimeInMs = 0;
            VisitedNodesQty = 0;
            var result = solveWithBacktracking(csp, assignment, false);
            _watch.Stop();
            ExecutionTimeInMs = _watch.ElapsedMilliseconds;
            return result;
        }

        public IList<IAssignment<V, D>> SolveAll(Csp<V, D> csp, IAssignment<V, D> assignment)
        {
            _watch.Start();
            ExecutionTimeInMs = 0;
            VisitedNodesQty = 0;
            solutions.Clear();
            solveWithBacktracking(csp, assignment, true);
            _watch.Stop();
            ExecutionTimeInMs = _watch.ElapsedMilliseconds;
            return solutions;
        }

        private IAssignment<V, D> solveWithBacktracking(Csp<V, D> csp, IAssignment<V, D> assignment,
            bool allSolutions)
        {
            if (_watch.ElapsedMilliseconds > TimeoutExecutionTimeMs)
            {
                throw new Exception($"Timed out - set to {TimeoutExecutionTimeMs} ms.");
            }

            if (assignment.AllVariablesAssigned())
            {
                return assignment;
            }

            var variable = VariableHeuristic.ChooseVariable(assignment, csp);
            var orderedValues = ValuesOrderHeuristic.GetOrderedDomainValues(assignment, csp, variable);
            foreach (var value in orderedValues)
            {
                VisitedNodesQty++;
                assignment.AssignVariable(variable, value);
                if (assignment.IsConsistent(variable, value))
                {
                    var result = solveWithBacktracking(csp, assignment, allSolutions);
                    if (result != null)
                    {
                        if (!allSolutions)
                        {
                            return result;
                        }

                        solutions.Add(result);
                    }
                }
                else
                    assignment.UnassignVariable(variable);
            }

            return null;
        }
    }
}