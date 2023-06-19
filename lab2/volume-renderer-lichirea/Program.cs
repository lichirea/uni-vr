using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace rt
{
    public class Program
    {
        public static void Main(string[] args)
        {

            // Cleanup
            const string frames = "../volume-renderer-lichirea/frames";
            if (Directory.Exists(frames))
            {
                var d = new DirectoryInfo(frames);
                foreach (var file in d.EnumerateFiles("*.jpeg"))
                {
                    file.Delete();
                }
            }
            Directory.CreateDirectory(frames);

            Grid grid = new Grid();

            grid.DimensionX = 47;
            grid.DimensionY = 512;
            grid.DimensionZ = 512;

            grid.Density = new byte[grid.DimensionX, grid.DimensionY, grid.DimensionZ];

            grid.Bounds = new Vector[2];

            grid.Bounds[0] = new Vector(0, 120, 150);
            grid.Bounds[1] = new Vector(47, 248, 362);
            string fileName = "Resources/vertebra-47x512x512.dat";

            using (BinaryReader br = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                while (br.BaseStream.Position != br.BaseStream.Length)
                {
                    for (int i = 0; i < grid.DimensionX; i++)
                    {
                        for (int j = 0; j < grid.DimensionY; j++)
                        {
                            for (int k = 0; k < grid.DimensionZ; k++)
                            {
                                grid.Density[i, j, k] = br.ReadByte();
                            }
                        }
                    }
                }
            }

            SortedDictionary<int, Color> colorMap = new SortedDictionary<int, Color>();
            colorMap.Add(13, new Color(0, 0, 0, 0));
            colorMap.Add(50, new Color(1, 1, 1, 0.10));
            colorMap.Add(75, new Color(1, 1, 1, 0.25));
            colorMap.Add(100, new Color(1, 1, 1, 0.35));
            colorMap.Add(150, new Color(1, 1, 1, 0.5));
            colorMap.Add(200, new Color(1, 1, 1, 1));


            var renderer = new Renderer(grid, colorMap);

            const int width = 600;
            const int height = 600;

            var middle = new Vector(26, 180, 256);

            var up = new Vector(-Math.Sqrt(0.125), -Math.Sqrt(0.75), Math.Sqrt(0.125)).Normalize();
            var first = (middle ^ up).Normalize();
            const double dist = 200.0;
            const int n = 30;
            const double step = 500.0 / n;

            var tasks = new Task[n];
            for (var i = 0; i < n; i++)
            {
                var ind = new[] { i };
                tasks[i] = Task.Run(() =>
                {
                    var k = ind[0];
                    var a = (step * k) * Math.PI / 180.0;
                    var ca = Math.Cos(a);
                    var sa = Math.Sin(a);

                    var dir = first * ca + (up ^ first) * sa + up * (up * first) * (1.0 - ca);

                    var camera = new Camera(
                        middle + dir * dist,
                        dir * -1.0,
                        up,
                        65.0,
                        160.0,
                        120.0,
                        0.0,
                        1000.0
                    );

                    var filename = frames + "/vertebra/" + $"{k + 1:000}" + ".jpeg";
                    renderer.Render(camera, width, height, filename);
                    Console.WriteLine($"Frame {k + 1}/{n} completed");
                });
            }

            Task.WaitAll(tasks);

        }
    }
}