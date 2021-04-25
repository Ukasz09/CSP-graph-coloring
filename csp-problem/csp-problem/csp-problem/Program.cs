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

        private const string GraphColoringSolutionForwardCheckingFilePath =
            GraphColoringBasePath + "solution-forward-checking.json";

        private const string ZebraPuzzleSolutionFilePath = CspExerciseBasePath + "zebra-solution.json";

        private const string ZebraPuzzleSolutionForwardCheckingFilePath =
            CspExerciseBasePath + "zebra-solution-forward-checking.json";

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private const string ZebraProblemArgName = "zebra";
        private const string MapColoringProblemArgName = "map";
        private static List<string> defaultMapDomains = new List<string> {"red", "blue", "green", "purple"};

        private static void Main(string[] args)
        {
            // var passedAnyArg = args.Length > 0;
            // // Solve chosen problem
            // if (passedAnyArg)
            // {
            //     var problemArgName = args[0];
            //     switch (problemArgName)
            //     {
            //         // map coloring
            //         case MapColoringProblemArgName:
            //         {
            //             var map = GenerateRandomDefaultMap();
            //             SolveMapColoring(map);
            //             break;
            //         }
            //         // zebra puzzle
            //         case ZebraProblemArgName:
            //             SolveZebraPuzzles();
            //             break;
            //         // incorrect arg name
            //         default:
            //             Console.WriteLine($"Incorrect arg name: {problemArgName}");
            //             break;
            //     }
            // }
            // // Solve all problems
            // else
            // {
            //     var map = GenerateRandomDefaultMap();
            //     SolveMapColoring(map);
            //     SolveZebraPuzzles();
            // }

            // TODO: tests
            // ReportMaker.GenerateRandomDefaultMap(5);
            var graphs = new Dictionary<int, Graph>
            {
                [8] = ReportMaker.GenerateRandomDefaultMap(8),
                [14] = ReportMaker.GenerateRandomDefaultMap(14),
                [18] = ReportMaker.GenerateRandomDefaultMap(18),
                [22] = ReportMaker.GenerateRandomDefaultMap(22)
            };
            ReportMaker.BtFcCompare(graphs);
        }

        private static Graph GenerateRandomDefaultMap()
        {
            var map = new MapGenerator().GenerateMap(20, 20, 20);
            DataUtils.SaveMap(map, GraphFilePath);
            return map;
        }

        #region Solvers

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

            // No forward checking
            var solutionsNoForwardChecking = zebraPuzzleSolver.SolveAllSolutions(false).ToList();
            SaveZebraPuzzleSolution(
                solutionsNoForwardChecking[0], zebraPuzzleSolver.SearchTimeInMs,
                zebraPuzzleSolver.SearchTimeTillFstSolutionInMs,
                zebraPuzzleSolver.VisitedNodesQty, zebraPuzzleSolver.VisitedNodesTillFstSolution,
                zebraPuzzleSolver.SolutionsQty, ZebraPuzzleSolutionFilePath, false,
                false
            );

            // With forward checking
            var solutionsForwardChecking = zebraPuzzleSolver.SolveAllSolutions(true).ToList();
            SaveZebraPuzzleSolution(
                solutionsForwardChecking[0], zebraPuzzleSolver.SearchTimeInMs,
                zebraPuzzleSolver.SearchTimeTillFstSolutionInMs,
                zebraPuzzleSolver.VisitedNodesQty, zebraPuzzleSolver.VisitedNodesTillFstSolution,
                zebraPuzzleSolver.SolutionsQty,
                ZebraPuzzleSolutionForwardCheckingFilePath, false, true
            );

            #endregion
        }

        private static void SolveMapColoring(Graph map)
        {
            #region solverInitialization

            // var valueOrderHeuristic = new Lcv<string, string>();
            var valueOrderHeuristic = new TrivialOrderValues<string, string>();
            // var variableOrderHeuristic = new Mrv<string, string>();
            var variableOrderHeuristic = new FirstVariableHeuristic<string, string>();
            var backtrackSolver = new BacktrackSolver<string, string>(valueOrderHeuristic, variableOrderHeuristic);
            var mapColoringSolver = new MapColoringSolver(backtrackSolver);

            #endregion

            #region solutionSearching

            _logger.Info("--------------------------------------------");
            _logger.Info("Started solving Graph Coloring Problem");

            // No forward checking
            var resultNoForwardChecking = mapColoringSolver.SolveAll(map, defaultMapDomains.ToList(), false);
            SaveMapColoringAllSolutions(resultNoForwardChecking, mapColoringSolver.SearchTimeInMs,
                mapColoringSolver.SearchTimeTillFstSolutionInMs,
                mapColoringSolver.VisitedNodesQty, mapColoringSolver.VisitedNodesTillFstSolution,
                mapColoringSolver.FoundSolutionsQty, false, false);

            // With forward checking

            var resultForwardChecking = mapColoringSolver.SolveAll(map, defaultMapDomains.ToList(), true);
            SaveMapColoringAllSolutions(resultForwardChecking, mapColoringSolver.SearchTimeInMs,
                mapColoringSolver.SearchTimeTillFstSolutionInMs,
                mapColoringSolver.VisitedNodesQty, mapColoringSolver.VisitedNodesTillFstSolution,
                mapColoringSolver.FoundSolutionsQty, false, true);

            #endregion
        }

        #endregion


        #region Result saving and logging

        private static void SaveMapColoringAllSolutions(
            IList<IDictionary<string, string>> solutions,
            long searchTimeInMs,
            long searchTimeTillFstSolutionInMs,
            int visitedNodesQty,
            int visitedNodesTillFstSolution,
            int solutionsQty,
            bool withSolutionLogging,
            bool withForwardChecking
        )
        {
            var logMsg = DataUtils.GetMapColoringAllSolutionsContent(solutions);
            LogResult(logMsg.ToArray(), searchTimeInMs, searchTimeTillFstSolutionInMs, visitedNodesQty,
                visitedNodesTillFstSolution, solutionsQty,
                withSolutionLogging,
                withForwardChecking);
            DataUtils.SaveGraphColoringAllSolutions(solutions, GraphColoringSolutionFilePath);
        }

        private static void SaveZebraPuzzleSolution(IDictionary<string, int> solution, long searchTimeInMs,
            long searchTimeTillFstSolutionInMs,
            int totalVisitedNodesQty, int visitedNodesTillFstSolution, int solutionsQty, string filePath,
            bool withSolutionLogging,
            bool withForwardChecking)
        {
            var logMsg = solution.Select(kvp => kvp.Key + ": " + kvp.Value).ToArray();
            LogResult(logMsg, searchTimeInMs, searchTimeTillFstSolutionInMs, totalVisitedNodesQty,
                visitedNodesTillFstSolution, solutionsQty,
                withSolutionLogging, withForwardChecking);
            DataUtils.SaveZebraPuzzleSolution(solution, filePath);
        }

        private static void LogResult(
            string[] logMsg, long searchTimeInMs, long searchTimeTillFstSolutionInMs, int totalVisitedNodesQty,
            int visitedNodesTillFstSolution,
            int solutionsQty, bool withSolutionLogging,
            bool withForwardChecking)
        {
            _logger.Info("---------------------------------");
            _logger.Info($"Forward checking: {withForwardChecking.ToString()}");
            if (withSolutionLogging)
            {
                _logger.Info($"Found solution: {string.Join(",", logMsg)}");
            }

            _logger.Info($"Search time: {searchTimeInMs.ToString()} ms");
            _logger.Info($"Search time till first solution: {searchTimeTillFstSolutionInMs.ToString()} ms");
            _logger.Info($"Total visited nodes qty: {totalVisitedNodesQty.ToString()}");
            _logger.Info($"Visited nodes till first solution: {visitedNodesTillFstSolution.ToString()}");
            _logger.Info($"Amount of found solutions: {solutionsQty.ToString()}");
            _logger.Info("---------------------------------\n");
        }

        #endregion
    }
}