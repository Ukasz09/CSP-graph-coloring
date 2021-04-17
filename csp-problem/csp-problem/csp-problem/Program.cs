using System;
using System.Collections.Generic;
using System.Linq;
using csp_problem.csp.cspSolver;
using csp_problem.csp.heuristics;
using NLog;

namespace csp_problem
{
    internal static class Program
    {
        private const string CspExerciseBasePath = "/home/ukasz09/Documents/OneDrive/Uczelnia/Semestr_VI/SI-L/2/";
        private const string GraphColoringBasePath = CspExerciseBasePath + "graph-coloring-ui/";
        private const string GraphFilePath = GraphColoringBasePath + "graph.json";
        private const string GraphColoringSolutionFilePath = GraphColoringBasePath + "solution.json";
        private const string ZebraPuzzleSolutionFilePath = CspExerciseBasePath + "zebra-puzzle-solution.json";
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private const string ZebraProblemArgName = "zebra";
        private const string MapColoringProblemArgName = "map";

        private static void Main(string[] args)
        {
            var passedAnyArg = args.Length > 0;
            if (passedAnyArg)
            {
                var problemArgName = args[0];
                switch (problemArgName)
                {
                    // map coloring
                    case MapColoringProblemArgName:
                        SolveMapColoring();
                        break;
                    // zebra puzzle
                    case ZebraProblemArgName:
                        SolveZebraPuzzles();
                        break;
                    // incorrect arg name
                    default:
                        Console.WriteLine($"Incorrect arg name: {problemArgName}");
                        break;
                }
            }
            // Solve all problems
            else
            {
                SolveMapColoring();
                SolveZebraPuzzles();
            }
        }

        private static void SolveZebraPuzzles()
        {
            #region initialization

            var valueOrderHeuristicZebra = new TrivialOrderValues<string, int>();
            var variableOrderHeuristicZebra = new FirstVariableHeuristic<string, int>();
            var solver = new BacktrackSolver<string, int>(valueOrderHeuristicZebra, variableOrderHeuristicZebra);
            var zebraPuzzleSolver = new ZebraPuzzleSolver(solver);

            #endregion

            #region solutionSearching

            _logger.Info("--------------------------------------------");
            _logger.Info("Started solving Zebra Puzzle Problem");
            var allSolutions = zebraPuzzleSolver.SolveAllSolutions();
            var searchTimeInMs = zebraPuzzleSolver.SearchTimeInMs;
            var visitedNodesQty = zebraPuzzleSolver.VisitedNodesQty;

            #endregion
        }

        private static void SolveMapColoring()
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

            _logger.Info("--------------------------------------------");
            _logger.Info("Started solving Graph Coloring Problem");
            var domains = new List<string>() {"red", "blue", "green", "orange"};
            var result = mapColoringSolver.Solve(map, domains);
            var searchTimeInMs = mapColoringSolver.SearchTimeInMs;
            var visitedNodesQty = mapColoringSolver.VisitedNodesQty;
            SaveMapColoringSolution(result, searchTimeInMs, visitedNodesQty);

            #endregion
        }

        private static void SaveMapColoringSolution(IDictionary<string, string> solution, long searchTimeInMs,
            int visitedNodesQty)
        {
            var logMsg = solution.Select(kvp => kvp.Key + ": " + kvp.Value.ToString()).ToArray();
            LogResult(logMsg, searchTimeInMs, visitedNodesQty);
            DataUtils.SaveGraphColoringSolution(solution, GraphColoringSolutionFilePath);
        }

        private static void SaveZebraPuzzleSolution(IDictionary<string, int> solution, long searchTimeInMs,
            int visitedNodesQty)
        {
            var logMsg = solution.Select(kvp => kvp.Key + ": " + kvp.Value).ToArray();
            LogResult(logMsg, searchTimeInMs, visitedNodesQty);
            DataUtils.SaveZebraPuzzleSolution(solution, ZebraPuzzleSolutionFilePath);
        }

        private static void LogResult(string[] logMsg, long searchTimeInMs, int visitedNodesQty)
        {
            _logger.Info("---------------------------------");
            _logger.Info($"Found solution: {string.Join(",", logMsg)}");
            _logger.Info($"Search time: {searchTimeInMs.ToString()} ms");
            _logger.Info($"Visited nodes: {visitedNodesQty.ToString()}");
        }
    }
}