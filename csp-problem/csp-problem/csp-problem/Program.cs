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
        private const string SolutionFilePath = GraphColoringBasePath + "solution.json";
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();


        private static void Main(string[] args)
        {
            // SolveGraphColoring();

            SolveZebraPuzzles();
        }

        private static void SolveZebraPuzzles()
        {
            var valueOrderHeuristicZebra = new TrivialOrderValues<string, int>();
            var variableOrderHeuristicZebra = new FirstVariableHeuristic<string, int>();
            new ZebraPuzzle(new BacktrackSolver<string, int>(valueOrderHeuristicZebra, variableOrderHeuristicZebra))
                .Solve();
        }

        private static void SolveGraphColoring()
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

            var domains = new List<string>() {"red", "blue", "green", "orange"};
            var result = mapColoringSolver.Solve(map, domains);
            var searchTimeInMs = mapColoringSolver.SearchTimeInMs();
            SaveGraphColoringSolution(result, searchTimeInMs);

            #endregion
        }

        private static void SaveGraphColoringSolution(IDictionary<string, string> solution, long searchTimeInMs)
        {
            var logResult = solution.Select(kvp => kvp.Key + ": " + kvp.Value.ToString()).ToArray();
            _logger.Info($"---------------------------------");
            _logger.Info($"Found solution: {string.Join(",", logResult)}");
            _logger.Info($"Search time: {searchTimeInMs.ToString()} ms");
            DataUtils.SaveSolution(solution, SolutionFilePath);
        }
    }
}