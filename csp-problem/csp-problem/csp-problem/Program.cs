namespace csp_problem
{
    internal static class Program
    {
        private static string _graphFilePath =
            "/home/ukasz09/Documents/OneDrive/Uczelnia/Semestr_VI/SI-L/2/graph-coloring-ui/graph.json";

        private static void Main(string[] args)
        {
            var map = new MapGenerator().GenerateMap(6, 10, 10);
            DataUtils.SaveMap(map, _graphFilePath);
        }
    }
}