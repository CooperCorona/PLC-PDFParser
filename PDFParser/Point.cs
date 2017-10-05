using System;
namespace PDFParser
{
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y) {
            X = x;
            Y = y;
        }

        public bool NearBy(Point that, int axisDistance) {
            return Math.Abs(X - that.X) <= axisDistance && Math.Abs(Y - that.Y) <= axisDistance;
        }

        public override int GetHashCode() {
            return (X << 16) & Y;
        }

		public override bool Equals(Object obj)
		{
            if (obj == null || !(obj is Point)) {
                return false;
            }
            Point p = (Point)obj;
            return X == p.X && Y == p.Y;
		}
	}
}
