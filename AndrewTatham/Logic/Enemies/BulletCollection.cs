using System.Collections.Generic;
using System.Linq;
using Robocode;

namespace AndrewTatham.Logic.Enemies
{
    public class BulletCollection
    {
        private readonly string _enemyName;
        private readonly List<Bullet> _bullets = new List<Bullet>();

        public BulletCollection(string enemyName)
        {
            _enemyName = enemyName;
        }

        public void Add(Bullet bullet)
        {
            _bullets.Add(bullet);
        }

        public double DamageInTransit
        {
            get
            {
                var bulletsInFlight = _bullets
                    .Where(b => b != null && b.IsActive && b.Victim == null)
                    .ToList();
                return _bullets.Any() ? bulletsInFlight.Sum(b => Rules.GetBulletDamage(b.Power)) : 0d;
            }
        }

        public double? Accuracy
        {
            get
            {
                var bulletsThatCount = _bullets
                    .Where(b => b != null && !b.IsActive).ToList();
                var hits = bulletsThatCount.Count(b => b != null && b.Victim == _enemyName);
                var total = bulletsThatCount.Count;
                return total == 0 ? (double?)null : hits / (double)total;
            }
        }
    }
}