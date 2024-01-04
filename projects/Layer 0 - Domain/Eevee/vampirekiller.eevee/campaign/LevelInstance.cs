using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using Util.entity;
using vampirekiller.eevee.campaign.encounters;

namespace vampirekiller.eevee.campaign;

public class LevelInstance : Identifiable
{
    public ID entityUid { get; set; }

    public LevelModel levelModel { get; set; }

    public LevelInstance() { }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    private static List<EncounterInstance> generateEncounters(LevelModel model)
    {
        List<EncounterInstance> encounters = new List<EncounterInstance>();



        return encounters;
    }

    private static LevelPath[] generatePaths(EncounterInstance[] encounters)
    {
        return Array.Empty<LevelPath>();
    }

    public class LevelGraph
    {
        public List<Point> vertices { get; }
        public List<Tuple<Point, Point>> edges { get; }

        private LevelGraph(List<Point> vertices, List<Tuple<Point, Point>> edges)
        {
            this.vertices = vertices;
            this.edges = edges;
        }

        public static LevelGraph generateGraph(int depth, int minVertices, int maxVertices, int maxWidth) {
            return generateGraph(depth, minVertices, maxVertices, maxWidth, new Random().Next(42069));
        }


        // Inspiration: https://www.reddit.com/r/gamedev/comments/8852z3/slay_the_spire_path_generation/
        public static LevelGraph generateGraph(int depth, int minVertices, int maxVertices, int maxWidth, int seed)
        {
            Random r = new Random(seed);
            List<Point> vertices = new List<Point>();
            List<Tuple<Point, Point>> edges = new List<Tuple<Point, Point>>();

            List<Point> lastRow = generateRow(0, r, 1, 1, maxWidth);
            vertices.AddRange(lastRow);

            for (int i = 1; i < depth + 1; i++)
            {
                List<Point> newVertices = depth != i
                    ? generateRow(i, r, minVertices, maxVertices, maxWidth)
                    : generateRow(i, r, 1, 1, maxWidth); // Last row has a single vertice (same as starting)
                List<Tuple<Point, Point>> newEdges = connectRows(r, lastRow, newVertices);

                vertices.AddRange(newVertices);
                edges.AddRange(newEdges);
                lastRow = newVertices;
            }

            return new LevelGraph(vertices, edges);
        }

        private static List<Point> generateRow(int currentRow, Random r, int minVertices, int maxVertices, int maxWidth)
        {
            // Determine at which x positions the vertices will be present
            List<bool> verticesInRow = new List<bool>();
            int verticesCount = r.Next(minVertices, maxVertices);
            for (int i = 0; i < maxWidth; i++)
            {
                verticesInRow.Add(i < verticesCount);
            }
            List<bool> shuffledVerticesInRow = shuffleArray(r, verticesInRow);

            // Replace booleans with points
            List<Point> row = new List<Point>();
            for (int i = 0; i < shuffledVerticesInRow.Count; i++)
            {
                if (shuffledVerticesInRow[i])
                {
                    row.Add(new Point(i, currentRow));
                }
            }

            return row;
        }

        private static List<Tuple<Point, Point>> connectRows(Random r, List<Point> originRow, List<Point> destRow)
        {
            var generateConnection = (bool isOrigin, Point p, List<Point> options) =>
            {
                Point closestPoint = findClosestPoint(r, p, options);
                Point origin = isOrigin ? p : closestPoint;
                Point dest = isOrigin ? closestPoint : p;
                return new Tuple<Point, Point>(origin, dest);
            };

            List<Tuple<Point, Point>> connections = new List<Tuple<Point, Point>>();

            // First pass to make sure every vertice has atleast one entry/exit
            Dictionary<Point, bool> pointsToConnect = new Dictionary<Point, bool>();
            originRow.ForEach(p => pointsToConnect.Add(p, true));
            destRow.ForEach(p => pointsToConnect.Add(p, false));
            while (pointsToConnect.Count > 0)
            {
                KeyValuePair<Point, bool> p = pointsToConnect.First();
                List<Point> otherRow = p.Value ? destRow : originRow;
                Tuple<Point, Point> newConnection = generateConnection(p.Value, p.Key, otherRow);
                connections.Add(newConnection);

                if (pointsToConnect.ContainsKey(newConnection.Item1))
                {
                    pointsToConnect.Remove(newConnection.Item1);
                }
                if (pointsToConnect.ContainsKey(newConnection.Item2))
                {
                    pointsToConnect.Remove(newConnection.Item2);
                }
            }

            return connections;
        }

        private static List<T> shuffleArray<T>(Random r, List<T> array)
        {
            List<T> result = new List<T>(array);

            for (int i = 0; i < result.Count; i++)
            {
                int rdmIndex = r.Next(result.Count);

                // swap
                T temp = result[rdmIndex];
                result[rdmIndex] = result[i];
                result[i] = temp;
            }

            return result;
        }

        private static Point findClosestPoint(Random r, Point point, List<Point> points)
        {
            Func<Point, double> calcDistance = (Point other) =>
            {
                int xDiff = point.X - other.X;
                int yDiff = point.Y - other.Y;
                return Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
            };

            Point closestPoint = points.First();
            double closestDistance = calcDistance(closestPoint);

            foreach (Point p in points)
            {
                if (closestPoint == p) continue;

                double distance = calcDistance(p);
                if (closestDistance > distance)
                {
                    closestPoint = p;
                    closestDistance = distance;
                }
                else if (closestDistance == distance)
                {
                    int choice = r.Next(1);
                    closestPoint = choice == 0 ? closestPoint : p;
                    closestDistance = choice == 0 ? closestDistance : distance;
                }
            }

            return closestPoint;
        }
    }
}