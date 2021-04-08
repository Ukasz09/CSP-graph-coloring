using System.IO;
using NLog;

namespace csp_problem
{
    public static class DataUtils
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static void SaveMap(Graph map, string filePath)
        {
            var lines = new[] {"[", map.ToString(), "]"};
            File.WriteAllLines(filePath, lines);
            _logger.Log(LogLevel.Info, $"Correct saved map graph in file: {filePath}");
        }

        public static void SaveSolution(string[] solution, string filePath)
        {
            File.WriteAllLines(filePath, solution);
            _logger.Log(LogLevel.Info, $"Correct saved solution in file: {filePath}");
        }
    }
}