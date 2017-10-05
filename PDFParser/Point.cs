using System;
namespace PDFParser
{
    /// <summary>
    /// Represents a two dimensional integer coordinate.
    /// </summary>
    public struct Point
    {
        /// <summary>
        /// The x-value of the coordinate.
        /// </summary>
        /// <value>The x.</value>
        public int X { get; set; }
        /// <summary>
        /// The y-value of the coordinate.
        /// </summary>
        /// <value>The y.</value>
        public int Y { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PDFParser.Point"/> struct.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public Point(int x, int y) {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Determines if the given point is "near" this point. If the difference
        /// between the two x-values and the difference between both the y-values
        /// are less than or equal to the given distance, the points are considered
        /// "near".
        /// </summary>
        /// <returns><c>true</c>, if the point is considered "near" to this one, <c>false</c> otherwise.</returns>
        /// <param name="that">The other point.</param>
        /// <param name="axisDistance">The distance between coordinates to consider points "near".</param>
        public bool NearBy(Point that, int axisDistance) {
            return Math.Abs(X - that.X) <= axisDistance && Math.Abs(Y - that.Y) <= axisDistance;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="T:PDFParser.Point"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode() {
            return (X << 16) & Y;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="T:PDFParser.Point"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="T:PDFParser.Point"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current <see cref="T:PDFParser.Point"/>;
        /// otherwise, <c>false</c>.</returns>
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
