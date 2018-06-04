using System;
using AndrewTatham.Logic.Enemies;
using Robocode;

namespace AndrewTatham.Logic.Behaviors.Strategies.Aiming.Prediction.GF
{
    public class GfStatKey : IEquatable<GfStatKey>
    {
        private readonly int _key;

        public GfStatKey(IEnemy enemy)
        {
            _key = Convert.ToInt32(Math.Round(enemy.LateralVelocityScalar / Rules.MAX_VELOCITY));
        }

        #region IEquatable<GuessFactorStatsKey> Members

        public bool Equals(GfStatKey other)
        {
            return _key == other._key;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(GfStatKey))
            {
                return Equals((GfStatKey)obj);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return _key.GetHashCode();
        }

        #endregion IEquatable<GuessFactorStatsKey> Members

        //public const int Length = 2;
    }
}