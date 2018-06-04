using System;
using System.Collections.Generic;
using System.Text;
using Robocode;

namespace AndrewTatham.Helpers
{
    public static class RandomHelper
    {
        private static readonly double BattlefieldX;
        private static readonly double BattlefieldY;

        static RandomHelper()
        {
            if (RandomBool())
            {
                BattlefieldX = 1000;
                BattlefieldY = 1000;
            }
            else
            {
                BattlefieldX = 800;
                BattlefieldY = 600;
            }
        }

        private static readonly Random InstanceRandom = new Random();

        public static double NextDouble(double max)
        {
            return InstanceRandom.NextDouble() * max;
        }

        public static double NextDouble(double min, double max)
        {
            return min + (max - min) * InstanceRandom.NextDouble();
        }

        public static int NextInt(int max)
        {
            return InstanceRandom.Next(max);
        }

        public static int NextInt(int min, int max)
        {
            return InstanceRandom.Next(min, max);
        }

        public static int Next()
        {
            return InstanceRandom.Next();
        }

        public static int[] IntArray(int length, int max)
        {
            var retval = new List<int>();
            for (int i = 0; i < length; i++)
            {
                retval.Add(InstanceRandom.Next(max));
            }
            return retval.ToArray();
        }

        public static Bullet RandomBullet()
        {
            return new Bullet(
                RandomHeading(),
                RandomX(),
                RandomY(),
                RandomBulletPower(),
                RandomRobotName(),
                RandomRobotName(),
                true,
                RandomInt()
                );
        }

        public static bool RandomBool()
        {
            return InstanceRandom.Next(0, 2) == 0;
        }

        public static int RandomInt()
        {
            return InstanceRandom.Next();
        }

        public static string RandomRobotName()
        {
            return RandomString(8);
        }

        public static string RandomString(int numberOfLetters)
        {
            StringBuilder builder = new StringBuilder(numberOfLetters);
            for (int i = 0; i < numberOfLetters; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * InstanceRandom.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public static double RandomBulletPower()
        {
            return InstanceRandom.NextDouble() * (Rules.MAX_BULLET_POWER - Rules.MIN_BULLET_POWER) + Rules.MIN_BULLET_POWER;
        }

        public static Vector RandomLocation()
        {
            return new Vector(RandomX(), RandomY());
        }

        public static double RandomY()
        {
            return InstanceRandom.NextDouble() * BattlefieldY;
        }

        public static double RandomX()
        {
            return InstanceRandom.NextDouble() * BattlefieldX;
        }

        public static double RandomHeading()
        {
            return InstanceRandom.NextDouble() * 360d;
        }

        public static double RandomBearing()
        {
            return InstanceRandom.NextDouble() * 360d;
        }

        public static double RandomEnergy()
        {
            return InstanceRandom.NextDouble() * 100d;
        }

        public static double RandomDistance()
        {
            return InstanceRandom.NextDouble() * 100d;
        }

        public static double RandomVelocity()
        {
            return InstanceRandom.NextDouble() * Rules.MAX_VELOCITY;
        }

        public static Angle RandomAngle()
        {
            return new Angle(RandomBearing());
        }
    }
}