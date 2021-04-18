using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        
        public static void SaveGraphColoringSolution(IDictionary<string, string> solution, string filePath)
        {
            var lines = GetMapColoringSolutionContent(solution);
            File.WriteAllLines(filePath, lines);
            _logger.Log(LogLevel.Info, $"Correct saved solution in file: {filePath}");
        }

        public static void SaveGraphColoringAllSolutions(IEnumerable<IDictionary<string, string>> solutions,
            string filePath)
        {
            var lines = GetMapColoringAllSolutionsContent(solutions);
            File.WriteAllLines(filePath, lines);
            _logger.Log(LogLevel.Info, $"Correct saved solutions in file: {filePath}");
        }

        public static List<string> GetMapColoringAllSolutionsContent(IEnumerable<IDictionary<string, string>> solutions)
        {
            var lines = new List<string> {"["};
            foreach (var solution in solutions)
            {
                var content = GetMapColoringSolutionContent(solution);
                content.Add(",");
                lines.AddRange(content);
            }
            // Remove last comma
            lines[^1] = lines[^1].Remove(lines[^1].Length - 1);
            lines.Add("]");
            return lines;
        }

        private static List<string> GetMapColoringSolutionContent(IDictionary<string, string> solution)
        {
            var lines = new List<string> {"["};
            // Add quotes and comma
            lines.AddRange(solution.Select(varColor =>
                "{" + $"\"node\":{varColor.Key.ToString()},\"color\":\"{varColor.Value.ToString()}\"" + "},"));
            // Remove last comma
            lines[^1] = lines[^1].Remove(lines[^1].Length - 1);
            lines.Add("]");
            return lines;
        }

        public static void SaveZebraPuzzleSolution(IDictionary<string, int> solution, string filePath)
        {
            #region ParsingResultToCorrectJSONFormat

            var lines = new List<string> {"{"};
            // Parse to text lines
            var resultData = solution
                .Select(varHouseNumber => $"\"{varHouseNumber.Key}\":{varHouseNumber.Value.ToString()},")
                .ToList();
            lines.AddRange(resultData);
            // Remove last comma
            lines[^1] = lines[^1].Remove(lines[^1].Length - 1);
            lines.Add("}");

            #endregion

            File.WriteAllLines(filePath, lines);
            _logger.Log(LogLevel.Info, $"Correct saved solution in file: {filePath}");
        }
    }
}