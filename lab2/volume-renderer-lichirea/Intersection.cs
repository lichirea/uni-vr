namespace rt
{
    public class Intersection
    {
        public bool Valid { get; set; }
        public bool Visible { get; set; }
        public double Tmin { get; set; }
        public double Tmax { get; set; }
        public Vector Position { get; set; }
        public Line Line { get; set; }

        public Intersection()
        {
            Line = null;
            Valid = false;
            Visible = false;
            Tmin = 0;
            Tmax = 0;
            Position = null;
        }

        public Intersection(bool valid, bool visible, Line line, double tmin, double tmax)
        {
            Line = line;
            Valid = valid;
            Visible = visible;
            Tmin = tmin;
            Tmax = tmax;
            Position = Line.CoordinateToPosition(tmin);
        }
    }
}