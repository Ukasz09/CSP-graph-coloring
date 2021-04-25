using System;
using System.Collections.Generic;
using System.IO;
using csp_problem.csp.cspSolver;
using csp_problem.csp.heuristics;
using NLog;

namespace csp_problem
{
    public class ReportMaker
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #region file-path-constants

        private const string BaseReportsPath = "../../report-data";
        private static readonly string GraphFilePathPrefix = $"{BaseReportsPath}/graphs";
        private const string GraphFilePrefix = "graph-";
        private static readonly string FcBcCompareNodesQtyPath = $"{BaseReportsPath}/fcBcCompare/nodes.csv";
        private static readonly string FcBcCompareTimePath = $"{BaseReportsPath}/fcBcCompare/time.csv";

        #endregion

        #region graph-coloring-constants

        private static readonly List<string> _defaultMapDomains = new List<string> {"red", "blue", "green", "purple"};

        #endregion

        #region data-generating

        public static Graph GenerateRandomDefaultMap(int nodesQty)
        {
            var graph = new MapGenerator().GenerateMap(nodesQty, 25, 25);
            var outputFilePath = $"{GraphFilePathPrefix}/{GraphFilePrefix}{nodesQty.ToString()}.json";
            DataUtils.SaveMap(graph, outputFilePath);
            return graph;
        }

        #endregion

        #region report-tests

        public static void BtFcCompare(Dictionary<int, Graph> graphs)
        {
            #region solverInitialization

            var valueOrderHeuristic = new TrivialOrderValues<string, string>();
            var variableOrderHeuristic = new FirstVariableHeuristic<string, string>();
            var backtrackSolver = new BacktrackSolver<string, string>(valueOrderHeuristic, variableOrderHeuristic);
            var mapColoringSolver = new MapColoringSolver(backtrackSolver);

            #endregion

            // Clear report files
            File.WriteAllLines(FcBcCompareNodesQtyPath,
                new[] {"nodes-qty;fst-nodes;all-nodes;fc-fst-nodes;fc-all-nodes"});
            File.WriteAllLines(FcBcCompareTimePath, new[] {"nodes-qty;fst-time;all-time;fc-fst-time;fc-all-time"});

            foreach (var (nodesQty, graph) in graphs)
            {
                // Forward Checking
                mapColoringSolver.SolveAll(graph, _defaultMapDomains, true);
                var fcFstSolutionSearchTime = mapColoringSolver.SearchTimeTillFstSolutionInMs;
                var fcAllSolutionsSearchTime = mapColoringSolver.SearchTimeInMs;
                var fcFstSolutionVisitedNodesQty = mapColoringSolver.VisitedNodesTillFstSolution;
                var fcAllSolutionsVisitedNodesQty = mapColoringSolver.VisitedNodesQty;

                // No Forward Checking
                mapColoringSolver.SolveAll(graph, _defaultMapDomains, false);
                var fstSolutionSearchTime = mapColoringSolver.SearchTimeTillFstSolutionInMs;
                var allSolutionsSearchTime = mapColoringSolver.SearchTimeInMs;
                var fstSolutionVisitedNodesQty = mapColoringSolver.VisitedNodesTillFstSolution;
                var allSolutionsVisitedNodesQty = mapColoringSolver.VisitedNodesQty;

                var nodesExaminationTextLine = string.Join(";",
                    nodesQty.ToString(),
                    fstSolutionVisitedNodesQty.ToString(),
                    allSolutionsVisitedNodesQty.ToString(),
                    fcFstSolutionVisitedNodesQty.ToString(),
                    fcAllSolutionsVisitedNodesQty.ToString()
                );
                
                var timeExaminationTextLine = string.Join(";",
                    nodesQty.ToString(),
                    fstSolutionSearchTime.ToString(),
                    allSolutionsSearchTime.ToString(),
                    fcFstSolutionSearchTime.ToString(),
                    fcAllSolutionsSearchTime.ToString()
                );

                File.AppendAllLines(FcBcCompareNodesQtyPath, new[]{nodesExaminationTextLine});
                File.AppendAllLines(FcBcCompareTimePath, new[]{timeExaminationTextLine});
                _logger.Info($"Correct saved result for FB-BC investigation: nodesQty={nodesQty.ToString()}");
            }
        }

        #endregion
    }
}