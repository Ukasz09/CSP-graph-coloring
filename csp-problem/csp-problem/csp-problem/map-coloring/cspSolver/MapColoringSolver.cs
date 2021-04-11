using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using csp_problem.csp;
using csp_problem.csp.cspSolver;

namespace csp_problem
{
    public class MapColoringSolver
    {
        private ISolver<string, string> solver;

        public MapColoringSolver(ISolver<string, string> solver)
        {
            this.solver = solver;
        }

        public IDictionary<string, string> Solve(Graph graph, ICollection<string> domains)
        {
            var csp = GetCsp(graph, domains);
            var assignment = new Assignment<string, string>(csp);
            var resultAssignment = solver.Solve(csp, assignment);
            if (resultAssignment == null)
            {
                throw new Exception($"Couldn't find solution, time of executing: {solver.ExecutionTimeInMs} ms.");
            }

            var variableValues = resultAssignment.GetAssignedValueForAll();
            return variableValues;
        }

        private static Csp<string, string> GetCsp(Graph graph, ICollection<string> domains)
        {
            var variableNeighbours = GetVariableNeighbours(graph);
            var variables = variableNeighbours.Keys;
            var variableDomains = GetVariablesDomain(variables, domains);
            var noNeighboursWithTheSameColorConstraint = new NoNeighboursWithTheSameColor(variableNeighbours);
            var constraints = new List<IConstraint<string, string>> {noNeighboursWithTheSameColorConstraint};
            var csp = new Csp<string, string>(variableDomains, constraints);
            return csp;
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

        public long SearchTimeInMs()
        {
            return solver.ExecutionTimeInMs;
        }
    }
}