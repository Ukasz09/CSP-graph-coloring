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
        private const string allSolutionsArgName = "all";

        private static void Main(string[] args)
        {
            var passedAnyArg = args.Length > 0;
            if (passedAnyArg)
            {
                var problemArgName = args[0];
                var allSolutions = args.Length > 1 && args[1] == allSolutionsArgName;
                switch (problemArgName)
                {
                    // map coloring
                    case MapColoringProblemArgName:
                        SolveMapColoring(allSolutions);
                        break;
                    // zebra puzzle
                    case ZebraProblemArgName:
                        SolveZebraPuzzles(allSolutions);
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
                SolveMapColoring(false);
                SolveZebraPuzzles(false);
            }
        }

        private static void SolveZebraPuzzles(bool allSolutions)
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

            if (allSolutions)
            {
                // TODO: tmp without forward checking
                var solutions = zebraPuzzleSolver.SolveAllSolutions(true).ToList();
                var searchTimeInMs = zebraPuzzleSolver.SearchTimeInMs;
                var visitedNodesQty = zebraPuzzleSolver.VisitedNodesQty;
                SaveZebraPuzzleSolution(solutions[0], searchTimeInMs, visitedNodesQty, zebraPuzzleSolver.SolutionsQty);
            }
            else
            {
                // TODO: tmp without forward checking
                var solution = zebraPuzzleSolver.Solve(true);
                var searchTimeInMs = zebraPuzzleSolver.SearchTimeInMs;
                var visitedNodesQty = zebraPuzzleSolver.VisitedNodesQty;
                SaveZebraPuzzleSolution(solution, searchTimeInMs, visitedNodesQty,zebraPuzzleSolver.SolutionsQty);
            }

            #endregion
        }

        private static void SolveMapColoring(bool allSolutions)
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
           

            if (allSolutions)
            {
        
              
                
                var domains= new List<string>() {"red", "blue", "green","purple"};
                var resultNoForwardChecking = mapColoringSolver.SolveAll(map, domains, false);
                SaveMapColoringAllSolutions(resultNoForwardChecking, mapColoringSolver.SearchTimeInMs,
                    mapColoringSolver.VisitedNodesQty,mapColoringSolver.FoundSolutionsQty);
                
                domains = new List<string>() {"red", "blue", "green","purple"};
                var resultForwardChecking = mapColoringSolver.SolveAll(map, domains, true);
                SaveMapColoringAllSolutions(resultForwardChecking, mapColoringSolver.SearchTimeInMs,
                    mapColoringSolver.VisitedNodesQty, mapColoringSolver.FoundSolutionsQty);
            }
            else
            {
                var domains = new List<string>() {"red", "blue", "green", "orange"};
                var resultForwardChecking = mapColoringSolver.Solve(map, domains, true);
                SaveMapColoringSolution(resultForwardChecking, mapColoringSolver.SearchTimeInMs,
                    mapColoringSolver.VisitedNodesQty,mapColoringSolver.FoundSolutionsQty);
                
                domains = new List<string>() {"red", "blue", "green", "orange"};
                var resultNotForwardChecking = mapColoringSolver.Solve(map, domains, false);
                SaveMapColoringSolution(resultNotForwardChecking, mapColoringSolver.SearchTimeInMs,
                    mapColoringSolver.VisitedNodesQty,mapColoringSolver.FoundSolutionsQty);
            }

            #endregion
        }

        private static void SaveMapColoringSolution(IDictionary<string, string> solution, long searchTimeInMs,
            int visitedNodesQty, int solutionsQty)
        {
            var logMsg = solution.Select(kvp => kvp.Key + ": " + kvp.Value.ToString()).ToArray();
            LogResult(logMsg, searchTimeInMs, visitedNodesQty, solutionsQty);
            DataUtils.SaveGraphColoringSolution(solution, GraphColoringSolutionFilePath);
        }

        private static void SaveMapColoringAllSolutions(
            IList<IDictionary<string, string>> solutions,
            long searchTimeInMs,
            int visitedNodesQty,
            int solutionsQty
        )
        {
            var logMsg = DataUtils.GetMapColoringAllSolutionsContent(solutions);
            LogResult(logMsg.ToArray(), searchTimeInMs, visitedNodesQty, solutionsQty);
            DataUtils.SaveGraphColoringAllSolutions(solutions, GraphColoringSolutionFilePath);
        }

        private static void SaveZebraPuzzleSolution(IDictionary<string, int> solution, long searchTimeInMs,
            int visitedNodesQty,int solutionsQty)
        {
            var logMsg = solution.Select(kvp => kvp.Key + ": " + kvp.Value).ToArray();
            LogResult(logMsg, searchTimeInMs, visitedNodesQty, solutionsQty);
            DataUtils.SaveZebraPuzzleSolution(solution, ZebraPuzzleSolutionFilePath);
        }

        private static void LogResult(string[] logMsg, long searchTimeInMs, int visitedNodesQty, int solutionsQty)
        {
            _logger.Info("---------------------------------");
            _logger.Info($"Found solution: {string.Join(",", logMsg)}");
            _logger.Info($"Search time: {searchTimeInMs.ToString()} ms");
            _logger.Info($"Visited nodes: {visitedNodesQty.ToString()}");
            _logger.Info($"Amount of found solutions: {solutionsQty.ToString()}");
        }
    }
}