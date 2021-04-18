using System;
using System.Collections.Generic;
using System.Linq;
using csp_problem.csp;
using csp_problem.csp.cspSolver;

namespace csp_problem
{
    public class MapColoringSolver
    {
        private readonly ISolver<string, string> _solver;
        public long SearchTimeInMs => _solver.ExecutionTimeInMs;
        public int VisitedNodesQty => _solver.VisitedNodesQty;
        public int FoundSolutionsQty => _solver.FoundSolutionsQty;

        public MapColoringSolver(ISolver<string, string> solver)
        {
            _solver = solver;
        }

        public IDictionary<string, string> Solve(Graph graph, ICollection<string> domains, bool withForwardChecking)
        {
            var resultAssignment = SolveAll(graph, domains, withForwardChecking);
            return resultAssignment[0];
        }

        public IList<IDictionary<string, string>> SolveAll(Graph graph, ICollection<string> domains,
            bool withForwardChecking)
        {
            var csp = GetCsp(graph, domains);
            var assignment = new Assignment<string, string>(csp);
            var resultAssignment = _solver.SolveAll(csp, assignment, withForwardChecking);
            if (resultAssignment.Count == 0)
            {
                throw new Exception($"Couldn't find solution, time of executing: {_solver.ExecutionTimeInMs} ms.");
            }

            return resultAssignment;
        }

        private static Csp<string, string> GetCsp(Graph graph, ICollection<string> domains)
        {
            var variableNeighbours = GetVariableNeighbours(graph);
            var variables = variableNeighbours.Keys;
            var variableDomains = GetVariablesDomain(variables, domains);

            var (varConstraints, allConstraints) = GetConstraints(variableNeighbours);
            var csp = new Csp<string, string>(variableDomains, allConstraints, varConstraints);
            return csp;
        }

        private static (Dictionary<string, IList<IConstraint<string, string>>>, List<IConstraint<string, string>>)
            GetConstraints(IDictionary<string, ICollection<string>> variableNeighbours)
        {
            var constraintsDict = new Dictionary<string, IList<IConstraint<string, string>>>(variableNeighbours.Count);
            var allConstraints = new List<IConstraint<string, string>>(variableNeighbours.Count);
            foreach (var variable in variableNeighbours.Keys)
            {
                var neighbours = variableNeighbours[variable];
                var constraint = new NoNeighboursWithTheSameColor(variable, neighbours);
                constraintsDict[variable] = new List<IConstraint<string, string>> {constraint};
                allConstraints.Add(constraint);
            }

            return (constraintsDict, allConstraints);
        }

        private static Dictionary<string, ICollection<string>> GetVariablesDomain(IEnumerable<string> variables,
            ICollection<string> domains)
        {
            var variableDomains = new Dictionary<string, ICollection<string>>();
            foreach (var variable in variables)
            {
                variableDomains[variable] = domains;
            }

            return variableDomains;
        }

        private static IDictionary<string, ICollection<string>> GetVariableNeighbours(Graph graph)
        {
            var varNeighbours = new Dictionary<string, ICollection<string>>();
            foreach (var node in graph.Nodes)
            {
                var neighbours = graph.GetNeighbours(node);
                varNeighbours[node.ToString()] = neighbours.Select(n => n.ToString()).ToList();
            }

            return varNeighbours;
        }
    }
}