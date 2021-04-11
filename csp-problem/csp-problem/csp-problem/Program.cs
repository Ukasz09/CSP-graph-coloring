using System.Collections.Generic;
using System.Linq;
using csp_problem.csp.cspSolver;
using csp_problem.csp.heuristics;
using NLog;

namespace csp_problem
{
    internal static class Program
    {
        private const string GraphColoringBasePath =
            "/home/ukasz09/Documents/OneDrive/Uczelnia/Semestr_VI/SI-L/2/graph-coloring-ui/";

        private const string GraphFilePath = GraphColoringBasePath + "graph.json";
        private const string SolutionFilePath = GraphColoringBasePath + "solutions.json";
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();


        private static void Main(string[] args)
        {
            #region mapInitialization

            var map = new MapGenerator().GenerateMap(6, 20, 20);
            DataUtils.SaveMap(map, GraphFilePath);

            #endregion

            #region solverInitialization

            var valueOrderHeuristic = new TrivialOrderValues<string, string>();
            var variableOrderHeuristic = new FirstVariableHeuristic<string, string>();
            var backtrackSolver = new BacktrackSolver<string, string>(valueOrderHeuristic, variableOrderHeuristic);
            var mapColoringSolver = new MapColoringSolver(backtrackSolver);

            #endregion

            #region solutionSearching

            var domains = new List<string>() {"red", "blue", "green", "pink"};
            var result = Solve(mapColoringSolver, map, domains);
            _logger.Debug(string.Join(",", result.Select(kvp => kvp.Key + ": " + kvp.Value.ToString())));
            var searchTimeInMs = mapColoringSolver.SearchTimeInMs();
            SaveGraphColoringSolution(result, searchTimeInMs);

            #endregion
        }

        private static IDictionary<string, string> Solve(MapColoringSolver solver, Graph map,
            ICollection<string> domains)
        {
            return solver.Solve(map, domains);
        }

        private static void SaveGraphColoringSolution(IDictionary<string, string> solution, long searchTimeInMs)
        {
            var resultDomains = solution.Values.ToArray();
            _logger.Info($"---------------------------------");
            _logger.Info($"Found solution: {string.Join(",", resultDomains)}");
            _logger.Info($"Search time: {searchTimeInMs.ToString()}");
            DataUtils.SaveSolution(resultDomains, SolutionFilePath);
        }
    }
}