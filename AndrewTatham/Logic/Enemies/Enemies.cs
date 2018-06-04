using System.Collections.Generic;
using System.Linq;
using AndrewTatham.Helpers;
using Robocode;

namespace AndrewTatham.Logic.Enemies
{
    public class Enemies : IRender
    {
        private readonly Dictionary<string, IEnemy> _enemies = new Dictionary<string, IEnemy>();

        public virtual IEnumerable<IEnemy> Alive
        {
            get { return _enemies.Values.Where(robot => !robot.IsDead); }
        }

        public IEnemy this[string name]
        {
            get { return SafeGetEnemy(name); }
        }

        public void Render(IGraphics graphics)
        {
            foreach (var item in _enemies)
            {
                item.Value.Render(graphics);
            }
        }

        public IEnemy OnScannedRobot(IContext context, ScannedRobotEvent evnt)
        {
            if (context != null)
            {
                if (!_enemies.ContainsKey(evnt.Name))
                {
                    _enemies.Add(evnt.Name, new Enemy(context, evnt));
                }
                _enemies[evnt.Name].OnScannedRobot(context, evnt);
                return _enemies[evnt.Name];
            }
            return null;
        }

        public IEnemy SafeGetEnemy(string name)
        {
            return _enemies.ContainsKey(name) ? _enemies[name] : null;
        }

        public void OnRobotDeath(string name)
        {
            IEnemy enemy = SafeGetEnemy(name);
            if (enemy != null)
            {
                enemy.RegisterRobotDeath(name);
            }
        }

        public IEnemy FirstOrDefault()
        {
            return Alive.FirstOrDefault();
        }

        public IEnemy First()
        {
            return Alive.First();
        }

        public bool Contains(string name)
        {
            return _enemies.ContainsKey(name);
        }

        public IEnemy SingleOrDefault()
        {
            return _enemies.Values.SingleOrDefault();
        }

        public void OnBulletHitBullet(BulletHitBulletEvent evnt)
        {
            IEnemy enemy = SafeGetEnemy(evnt.Bullet.Name);
            if (enemy != null)
            {
                enemy.RegisterBulletHitBullet(evnt);
            }
        }

        public void OnHitByBullet(HitByBulletEvent evnt)
        {
            IEnemy enemy = SafeGetEnemy(evnt.Name);
            if (enemy != null)
            {
                enemy.RegisterHitByBullet(evnt);
            }
        }

        public void OnTurn(Context context)
        {
            foreach (var enemy in Alive)
            {
                enemy.OnTurn(context);
            }
        }

        public Vector MeanCentre()
        {
            return Alive.Select(enemy => enemy.Location)
                .Aggregate((e1, e2) => e1 + e2) / Alive.Count();
        }
    }
}