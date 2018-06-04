using System;
using System.Collections.Generic;

namespace AndrewTatham.Helpers
{
    /// <summary>
    ///     Cartesian vector
    /// </summary>
    public class Vector
    {
        public Vector(double x, double y)
        {
            X = x;
            Y = y;
            Magnitude = Math.Sqrt(X * X + Y * Y);
            Heading = Angle.FromRadians(Math.Atan2(X, Y));
        }

        public Vector(Vector from, Vector to)
            : this(to.X - from.X, to.Y - from.Y)
        {
        }

        public Vector(double magnitude, Angle heading)
        {
            X = magnitude * Math.Sin(heading.Radians);
            Y = magnitude * Math.Cos(heading.Radians);
            Magnitude = magnitude;
            Heading = heading;
        }

        public Vector(Vector vector)
            : this(vector.X, vector.Y)
        {
        }

        public double X { get; private set; }

        public double Y { get; private set; }

        public double Magnitude { get; private set; }

        public Angle Heading { get; private set; }

        public static Vector[] GetRandom(int number, double maxX, double maxy)
        {
            var retval = new List<Vector>();
            for (int i = 0; i < number; i++)
            {
                retval.Add(new Vector(
                               RandomHelper.NextDouble(maxX),
                               RandomHelper.NextDouble(maxy)));
            }
            return retval.ToArray();
        }

        public static Vector operator +(Vector left, Vector right)
        {
            return new Vector(left.X + right.X, left.Y + right.Y);
        }

        public static Vector operator -(Vector left, Vector right)
        {
            return new Vector(left.X - right.X, left.Y - right.Y);
        }

        public static Vector operator *(double left, Vector right)
        {
            if (right != null)
            {
                return new Vector(right.Magnitude * left, right.Heading);
            }
            return null;
        }

        public static Vector operator *(Vector left, double right)
        {
            if (left != null)
            {
                return new Vector(left.Magnitude * right, left.Heading);
            }
            return null;
        }

        public static Vector operator /(Vector left, double right)
        {
            if (left != null)
            {
                return new Vector(left.Magnitude / right, left.Heading);
            }
            return null;
        }

        public override string ToString()
        {
            return string.Format("Vector({0:F2},{1:F2})", X, Y);
        }
    }
}