using System;

namespace rt
{
    public class Sphere : Geometry
    {
        public Vector Center { get; set; }
        private double Radius { get; set; }

        public Sphere(Vector center, double radius, Material material, Color color) : base(material, color)
        {
            Center = center;
            Radius = radius;
        }

        public override Intersection GetIntersection(Line line, double minDist, double maxDist)
        {
            // ADD CODE HERE: Calculate the intersection between the given line and this sphere
            // A = a^2 + c^2 + e^2
            // B = 2ab - 2ax + 2cd - 2cy + 2ef - 2ez
            // C = b^2 - 2bx + d^2 - 2dy + f^2 -2fz + x^2 + y^2 + z^2 - R^2
            var A = Math.Pow(line.Dx.X, 2) + Math.Pow(line.Dx.Y, 2) + Math.Pow(line.Dx.Z, 2);
            var B = 2 * line.X0.X * line.Dx.X - 2 * line.Dx.X * Center.X + 2 * line.Dx.Y * line.X0.Y - 2 * line.Dx.Y * Center.Y + 2 * line.Dx.Z * line.X0.Z - 2 * line.Dx.Z * Center.Z;
            var C = Math.Pow(line.X0.X, 2) - 2 * line.X0.X * Center.X + Math.Pow(line.X0.Y, 2) - 2 * line.X0.Y * Center.Y + Math.Pow(line.X0.Z, 2) - 2 * line.X0.Z * Center.Z
                    + Math.Pow(Center.X, 2) + Math.Pow(Center.Y, 2) + Math.Pow(Center.Z, 2) - Math.Pow(Radius, 2);

            var determinant = Math.Pow(B, 2) - 4 * A * C;
            if (determinant < 0)
            {
                return new Intersection();
            }
            var t1 = (-B - Math.Sqrt(determinant)) / (2 * A);
            if (determinant == 0)
            {
                if (t1 > maxDist || t1 < minDist)
                {
                    return new Intersection();
                }
                return new Intersection(true, true, this, line, t1);
            }
            else
            {
                var t2 = (-B + Math.Sqrt(determinant)) / (2 * A);
                var t = Math.Min(t1, t2);
                if (t > maxDist || t < minDist)
                {
                    return new Intersection();
                }

                return new Intersection(true, true, this, line, t);
            }

        }

        public override Vector Normal(Vector v)
        {
            var n = v - Center;
            n.Normalize();
            return n;
        }
    }
}