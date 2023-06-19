using System;
using System.Runtime.InteropServices;

namespace rt
{
    class RayTracer
    {
        private Geometry[] geometries;
        private Light[] lights;

        public RayTracer(Geometry[] geometries, Light[] lights)
        {
            this.geometries = geometries;
            this.lights = lights;
        }

        private double ImageToViewPlane(int n, int imgSize, double viewPlaneSize)
        {
            var u = n * viewPlaneSize / imgSize;
            u -= viewPlaneSize / 2;
            return u;
        }

        private Intersection FindFirstIntersection(Line ray, double minDist, double maxDist)
        {
            var intersection = new Intersection();

            foreach (var geometry in geometries)
            {
                var intr = geometry.GetIntersection(ray, minDist, maxDist);

                if (!intr.Valid || !intr.Visible) continue;

                if (!intersection.Valid || !intersection.Visible)
                {
                    intersection = intr;
                }  
                else if (intr.T < intersection.T)
                {
                    intersection = intr;
                }
            }

            return intersection;
        }

        private bool IsLit(Vector point, Light light, Geometry geometry)
        {
            // ADD CODE HERE: Detect whether the given point has a clear line of sight to the given light
            Line ray = new Line(light.Position, point);
            var intersection = FindFirstIntersection(ray, 0.0, (light.Position - point).Length());

            if(intersection.Geometry == geometry){
                return intersection.Valid;
            }
            return !intersection.Valid;
        }

        public void Render(Camera camera, int width, int height, string filename)
        {
            var background = new Color();
            var image = new Image(width, height);

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    // ADD CODE HERE: Implement pixel color calculation
                    Vector viewParallel = camera.Up ^ camera.Direction;
                    Vector x1 = camera.Position
                        + camera.Direction * camera.ViewPlaneDistance
                        + camera.Up * ImageToViewPlane(j, height, camera.ViewPlaneHeight)
                        + viewParallel * ImageToViewPlane(i, width, camera.ViewPlaneWidth);
                    var line = new Line(camera.Position, x1);
                    Intersection intersection = FindFirstIntersection(line, camera.FrontPlaneDistance, camera.BackPlaneDistance);
                    if(intersection.Visible){

                        Color color = new Color();
                        Material material = intersection.Geometry.Material;

                        foreach (var light in lights)
                        {
                            var lightColor = material.Ambient * light.Ambient;

                            if (IsLit(intersection.Position, light, intersection.Geometry))
                            {
                                var N = (intersection.Position - ((Sphere)intersection.Geometry).Center).Normalize();
                                var T = (light.Position - intersection.Position).Normalize();

                                var E = (camera.Position - intersection.Position).Normalize();
                                var R = N * (N * T) * 2 - T;

                                if (N * T > 0)
                                    lightColor = lightColor + material.Diffuse * light.Diffuse * (N * T);

                                if (E * R > 0)
                                    lightColor = lightColor + material.Specular * light.Specular * Math.Pow((E * R), material.Shininess);

                                lightColor = lightColor * light.Intensity;
                            }

                            color = color + lightColor;
                        }
                        
                        image.SetPixel(i, j, color);
                    }
                    else{
                        image.SetPixel(i, j, background);
                    }
                }
            }

            image.Store(filename);
        }
    }
}