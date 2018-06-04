using AndrewTatham.Helpers;
using AndrewTatham.Logic.Enemies;
using Robocode;

namespace AndrewTatham.Logic.Behaviors.Strategies.Aiming
{
    public class AimResult
    {
        public IEnemy Target { get; set; }

        public Vector Location { get; set; }

        public double Power { get; set; }

        public virtual double Speed
        {
            get { return Rules.GetBulletSpeed(Power); }
        }
    }
}