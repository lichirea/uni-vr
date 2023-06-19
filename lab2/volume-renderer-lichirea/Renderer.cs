using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace rt
{
    class Renderer
    {
        private Grid Grid;
        private SortedDictionary<int, Color> ColorMap;

        public Renderer(Grid grid, SortedDictionary<int, Color> colorMap)
        {
            this.Grid = grid;
            this.ColorMap = colorMap;

        }

        private double ImageToViewPlane(int n, int imgSize, double viewPlaneSize)
        {
            var u = n * viewPlaneSize / imgSize;
            u -= viewPlaneSize / 2;
            return u;
        }

        public Color sample(Line ray, Intersection intersection, int ipix, int jpix)
        {
            double totalDistance = 0;
            const double stepSize = 0.05;
            int numberOfSteps = (int)Math.Ceiling((intersection.Tmax - intersection.Tmin) / stepSize);

            Vector currentPosition = intersection.Position;
            byte currentDensity = Grid.NearestVoxel(currentPosition);
            Color finalColor = new Color();
            for (int i = 0; i < numberOfSteps; i++)
            {
                currentPosition = ray.CoordinateToPosition(intersection.Tmin + totalDistance);

                currentDensity = Grid.NearestVoxel(currentPosition);

                if (currentDensity >= 255)
                {
                    return ColorMap[256];
                }


                foreach (KeyValuePair<int, Color> colorMapping in ColorMap)
                {
                    if (currentDensity < colorMapping.Key)
                    {
                        double R1 = finalColor.Red * finalColor.Alpha + colorMapping.Value.Red * (1 - finalColor.Alpha);
                        double G1 = finalColor.Green * finalColor.Alpha + colorMapping.Value.Green * (1 - finalColor.Alpha);
                        double B1 = finalColor.Blue * finalColor.Alpha + colorMapping.Value.Blue * (1 - finalColor.Alpha);
                        double Alpha1 = finalColor.Alpha + (1 - finalColor.Alpha) * colorMapping.Value.Alpha;
                        finalColor = new Color(R1, G1, B1, Alpha1);
                        if (1 - finalColor.Alpha < 0.005)
                        {
                            return finalColor;
                        }
                        break;

                    }
                }
                totalDistance += stepSize;
            }
            return finalColor;
        }


        public void Render(Camera camera, int width, int height, string filename)
        {
            var background = new Color();
            var image = new Image(width, height);

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    Vector x0 = camera.Position;
                    Vector x1 = camera.Position +
                                camera.Direction * camera.ViewPlaneDistance +
                                camera.Up * ImageToViewPlane(j, height, camera.ViewPlaneHeight) +
                               (camera.Up ^ camera.Direction) * ImageToViewPlane(i, width, camera.ViewPlaneWidth);
                    Intersection intersection = Grid.Intersect(new Line(x0, x1));
                    if (!intersection.Valid || !intersection.Visible)
                    {
                        image.SetPixel(i, j, background);
                    }
                    else
                    {
                        Color finalColor = sample(new Line(x0, x1), intersection, i, j);

                        image.SetPixel(i, j, finalColor);
                    }

                }
            }

            image.Store(filename);
        }
    }
}