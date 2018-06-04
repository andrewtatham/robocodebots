using System;

namespace AndrewTatham.Helpers
{
    public class Angle
    {
        private readonly double _degrees;

        public Angle(double degrees)
        {
            while (degrees < 0)
            {
                degrees += 360d;
            }
            _degrees = degrees % 360;
        }

        /// <summary>
        ///     0 -> 360
        /// </summary>
        public double Degrees
        {
            get { return _degrees; }
        }

        /// <summary>
        ///     0 -> 2 * PI
        /// </summary>
        public double Radians
        {
            get
            {
                return _degrees * Math.PI / 180d;
            }
        }

        public double Degrees180
        {
            get { return _degrees <= 180d ? _degrees : _degrees - 360d; }
        }

        public Angle Perpendicular
        {
            get { return new Angle(Degrees + 90d); }
        }

        public Angle Opposite
        {
            get { return new Angle(Degrees + 180d); }
        }

        public static Angle operator +(Angle left, Angle right)
        {
            return new Angle(left.Degrees + right.Degrees);
        }

        public static Angle operator -(Angle left, Angle right)
        {
            return new Angle(left.Degrees - right.Degrees);
        }

        public static Angle FromRadians(double radians)
        {
            return new Angle(180d * radians / Math.PI);
        }

        public static bool operator ==(Angle left, Angle right)
        {
            try
            {
                return left.Degrees == right.Degrees;
            }
            catch
            {
                return false;
            }
        }

        public static bool operator !=(Angle left, Angle right)
        {
            try
            {
                return left.Degrees != right.Degrees;
            }
            catch
            {
                return false;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj.GetType() == typeof(Angle))
            {
                return Degrees == ((Angle)obj).Degrees;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Degrees.GetHashCode();
        }
    }
}