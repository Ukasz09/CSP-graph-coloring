using System.Collections.Generic;
using System.IO;
using csp_problem.csp.cspSolver;
using csp_problem.csp.heuristics;
using NLog;

namespace csp_problem
{
    public static class ReportMaker
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #region file-path-constants

        private const string BaseReportsPath = "../../report-data";
        private static readonly string GraphFilePathPrefix = $"{BaseReportsPath}/graphs";
        private const string GraphFilePrefix = "graph-";

        private static readonly string FcBcCompareNodesQtyPath = $"{BaseReportsPath}/fcBcCompare/nodes.csv";
        private static readonly string FcBcCompareTimePath = $"{BaseReportsPath}/fcBcCompare/time.csv";

        private static readonly string HeuristicsCompareNodesQtyPath = $"{BaseReportsPath}/heuristicsCompare/nodes.csv";
        private static readonly string HeuristicsCompareTimePath = $"{BaseReportsPath}/heuristicsCompare/time.csv";

        #endregion

        #region graph-coloring-constants

        private static List<string> _defaultMapDomains;

        #endregion

        #region data-generating

        public static Graph GenerateRandomDefaultMap(int nodesQty)
        {
            var graph = new MapGenerator().GenerateMap(nodesQty, 25, 25);
            var outputFilePath = $"{GraphFilePathPrefix}/{GraphFilePrefix}{nodesQty.ToString()}.json";
            DataUtils.SaveMap(graph, outputFilePath);
            return graph;
        }

        private static void ResetDomains()
        {
            _defaultMapDomains = new List<string> {"red", "blue", "green", "purple"};
        }

        #endregion

        #region report-tests

        public static void BtFcCompareGraph(Dictionary<int, Graph> graphs)
        {
            ResetDomains();

            #region solverInitialization

            var valueOrderHeuristic = new TrivialOrderValues<string, string>();
            var variableOrderHeuristic = new FirstVariableHeuristic<string, string>();
            var backtrackSolver = new BacktrackSolver<string, string>(valueOrderHeuristic, variableOrderHeuristic);
            var mapColoringSolver = new MapColoringSolver(backtrackSolver);

            #endregion

            // Clear report files
            const string headerText = "nodes-qty;fst;all;fc-fst;fc-all";
            File.WriteAllLines(FcBcCompareNodesQtyPath, new[] {headerText});
            File.WriteAllLines(FcBcCompareTimePath, new[] {headerText});

            foreach (var (nodesQty, graph) in graphs)
            {
                // Forward Checking
                ResetDomains();
                mapColoringSolver.SolveAll(graph, _defaultMapDomains, true);
                var fcFstSolutionSearchTime = mapColoringSolver.SearchTimeTillFstSolutionInMs;
                var fcAllSolutionsSearchTime = mapColoringSolver.SearchTimeInMs;
                var fcFstSolutionVisitedNodesQty = mapColoringSolver.VisitedNodesTillFstSolution;
                var fcAllSolutionsVisitedNodesQty = mapColoringSolver.VisitedNodesQty;
                ResetDomains();

                // No Forward Checking
                mapColoringSolver.SolveAll(graph, _defaultMapDomains, false);
                var fstSolutionSearchTime = mapColoringSolver.SearchTimeTillFstSolutionInMs;
                var allSolutionsSearchTime = mapColoringSolver.SearchTimeInMs;
                var fstSolutionVisitedNodesQty = mapColoringSolver.VisitedNodesTillFstSolution;
                var allSolutionsVisitedNodesQty = mapColoringSolver.VisitedNodesQty;
                ResetDomains();

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

                File.AppendAllLines(FcBcCompareNodesQtyPath, new[] {nodesExaminationTextLine});
                File.AppendAllLines(FcBcCompareTimePath, new[] {timeExaminationTextLine});
                _logger.Info($"Correct saved result for FB-BC investigation: nodesQty={nodesQty.ToString()}");
            }
        }
        
        public static void HeuristicsCompareGraph(Dictionary<int, Graph> graphs)
        {
            // 0) LCV + MRV
            // 1) TVAL + FVAR
            // 2) LCV + FVAR
            // 3) TVAL + MRV

            // Clear report files
            const string textHeader = "nodes-qty;with-fc;0-fst;0-all;1-fst;1-all;2-fst;2-all;3-fst;3-all";
            File.WriteAllLines(HeuristicsCompareNodesQtyPath, new[] {textHeader});
            File.WriteAllLines(HeuristicsCompareTimePath, new[] {textHeader});

            #region solverInitialization-0

            var valueOrderHeuristic0 = new Lcv<string, string>();
            var variableOrderHeuristic0 = new Mrv<string, string>();
            var backtrackSolver0 = new BacktrackSolver<string, string>(valueOrderHeuristic0, variableOrderHeuristic0);
            var mapColoringSolver0 = new MapColoringSolver(backtrackSolver0);

            #endregion

            #region solverInitialization-1

            var valueOrderHeuristic1 = new TrivialOrderValues<string, string>();
            var variableOrderHeuristic1 = new FirstVariableHeuristic<string, string>();
            var backtrackSolver1 = new BacktrackSolver<string, string>(valueOrderHeuristic1, variableOrderHeuristic1);
            var mapColoringSolver1 = new MapColoringSolver(backtrackSolver1);

            #endregion

            #region solverInitialization-2

            var valueOrderHeuristic2 = new Lcv<string, string>();
            var variableOrderHeuristic2 = new FirstVariableHeuristic<string, string>();
            var backtrackSolver2 = new BacktrackSolver<string, string>(valueOrderHeuristic2, variableOrderHeuristic2);
            var mapColoringSolver2 = new MapColoringSolver(backtrackSolver2);

            #endregion

            #region solverInitialization-3

            var valueOrderHeuristic3 = new TrivialOrderValues<string, string>();
            var variableOrderHeuristic3 = new Mrv<string, string>();
            var backtrackSolver3 = new BacktrackSolver<string, string>(valueOrderHeuristic3, variableOrderHeuristic3);
            var mapColoringSolver3 = new MapColoringSolver(backtrackSolver3);

            #endregion

            // No FC
            HeuristicCompareGraphHelper(graphs, mapColoringSolver0, mapColoringSolver1, mapColoringSolver2,
                mapColoringSolver3, false);
            
            // With FC
            HeuristicCompareGraphHelper(graphs, mapColoringSolver0, mapColoringSolver1, mapColoringSolver2,
                mapColoringSolver3, true);
        }

        private static void HeuristicCompareGraphHelper(
            Dictionary<int, Graph> graphs,
            MapColoringSolver mapColoringSolver0, MapColoringSolver mapColoringSolver1,
            MapColoringSolver mapColoringSolver2, MapColoringSolver mapColoringSolver3,
            bool withFc)
        {
            
            foreach (var (nodesQty, graph) in graphs)
            {
                // 0)
                ResetDomains();
                mapColoringSolver0.SolveAll(graph, _defaultMapDomains, withFc);
                var fstSolutionSearchTime0 = mapColoringSolver0.SearchTimeTillFstSolutionInMs;
                var allSolutionsSearchTime0 = mapColoringSolver0.SearchTimeInMs;
                var fstSolutionVisitedNodesQty0 = mapColoringSolver0.VisitedNodesTillFstSolution;
                var allSolutionsVisitedNodesQty0 = mapColoringSolver0.VisitedNodesQty;
                ResetDomains();

                // 1)
                mapColoringSolver1.SolveAll(graph, _defaultMapDomains, withFc);
                var fstSolutionSearchTime1 = mapColoringSolver1.SearchTimeTillFstSolutionInMs;
                var allSolutionsSearchTime1 = mapColoringSolver1.SearchTimeInMs;
                var fstSolutionVisitedNodesQty1 = mapColoringSolver1.VisitedNodesTillFstSolution;
                var allSolutionsVisitedNodesQty1 = mapColoringSolver1.VisitedNodesQty;
                ResetDomains();

                // 2)
                mapColoringSolver2.SolveAll(graph, _defaultMapDomains, withFc);
                var fstSolutionSearchTime2 = mapColoringSolver2.SearchTimeTillFstSolutionInMs;
                var allSolutionsSearchTime2 = mapColoringSolver2.SearchTimeInMs;
                var fstSolutionVisitedNodesQty2 = mapColoringSolver2.VisitedNodesTillFstSolution;
                var allSolutionsVisitedNodesQty2 = mapColoringSolver2.VisitedNodesQty;
                ResetDomains();

                // 3)
                mapColoringSolver3.SolveAll(graph, _defaultMapDomains, withFc);
                var fstSolutionSearchTime3 = mapColoringSolver3.SearchTimeTillFstSolutionInMs;
                var allSolutionsSearchTime3 = mapColoringSolver3.SearchTimeInMs;
                var fstSolutionVisitedNodesQty3 = mapColoringSolver3.VisitedNodesTillFstSolution;
                var allSolutionsVisitedNodesQty3 = mapColoringSolver3.VisitedNodesQty;
                ResetDomains();

                var nodesExaminationTextLine = string.Join(";",
                    nodesQty.ToString(),
                    withFc.ToString(),
                    fstSolutionVisitedNodesQty0.ToString(),
                    allSolutionsVisitedNodesQty0.ToString(),
                    fstSolutionVisitedNodesQty1.ToString(),
                    allSolutionsVisitedNodesQty1.ToString(),
                    fstSolutionVisitedNodesQty2.ToString(),
                    allSolutionsVisitedNodesQty2.ToString(),
                    fstSolutionVisitedNodesQty3.ToString(),
                    allSolutionsVisitedNodesQty3.ToString()
                );

                var timeExaminationTextLine = string.Join(";",
                    nodesQty.ToString(),
                    withFc.ToString(),
                    fstSolutionSearchTime0.ToString(),
                    allSolutionsSearchTime0.ToString(),
                    fstSolutionSearchTime1.ToString(),
                    allSolutionsSearchTime1.ToString(),
                    fstSolutionSearchTime2.ToString(),
                    allSolutionsSearchTime2.ToString(),
                    fstSolutionSearchTime3.ToString(),
                    allSolutionsSearchTime3.ToString()
                );

                File.AppendAllLines(HeuristicsCompareNodesQtyPath, new[] {nodesExaminationTextLine});
                File.AppendAllLines(HeuristicsCompareTimePath, new[] {timeExaminationTextLine});
                _logger.Info(
                    $"Correct saved result for heurisitcs investigation: nodesQty={nodesQty.ToString()}, FC={withFc.ToString()}");
            }
        }

        #endregion
    }
}