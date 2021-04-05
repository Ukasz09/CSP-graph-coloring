using System;
using System.Collections.Generic;

namespace csp_problem
{
    public class MapGenerator
    {
        private readonly Random randGen;

        public MapGenerator()
        {
            randGen = new Random();
        }

        public void GenerateMap(int nodesQty, int mapWidth, int mapHeight)
            // public List<Edge> GenerateMap(int nodesQty, int mapWidth, int mapHeight)
        {
            var nodes = DrawNodesWithoutRepeating(nodesQty, mapWidth, mapHeight);
            Console.WriteLine(string.Join(",", nodes));
        }

        private IEnumerable<Node> DrawNodesWithoutRepeating(int qty, int mapWidth, int mapHeight)
        {
            if (mapWidth < 0 || mapHeight < 0 || qty > mapWidth * mapWidth)
            {
                throw new ArgumentException(
                    $"Given nodes qty = {qty} is bigger than max possible unique nodes (= {mapWidth * mapHeight})" +
                    $" that can be created for this map (size = {mapWidth}x{mapHeight})");
            }

            var nodes = new List<Node>(qty);
            var i = 0;
            do
            {
                var randXIndex = randGen.Next(mapWidth);
                var randYIndex = randGen.Next(mapHeight);
                var node = new Node(randXIndex, randYIndex);

                // if found unique then add
                if (!nodes.Contains(node))
                {
                    nodes.Add(node);
                    i++;
                }
            } while (i < qty);

            return nodes;
        }
    }
}