using AndrewTatham.Logic.Enemies;
using Robocode;
using System;

namespace AndrewTatham.Logic.Behaviors.Strategies.Aiming.Prediction.GuessFactor
{
    public class GFStatKey : IEquatable<GFStatKey>
    {
        private readonly int _key;

        public GFStatKey(IEnemy enemy)
        {
            _key = Convert.ToInt32(Math.Round(enemy.LateralVelocityScalar / Rules.MAX_VELOCITY));
        }

        #region IEquatable<GuessFactorStatsKey> Members

        public bool Equals(GFStatKey other)
        {
            return _key == other._key;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(GFStatKey))
            {
                return Equals((GFStatKey)obj);
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