using System.IO;
using AndrewTatham.Helpers;
using AndrewTatham.Logic.Enemies;
using Robocode;

namespace AndrewTatham.Logic.Behaviors.Strategies
{
    public abstract class BaseStrategy : IRender
    {
        public static IContext Context { get; set; }

        public static TextWriter Out { get; set; }

        public virtual void Render(IGraphics graphics)
        {
        }

        public abstract void Execute();

        public virtual void OnScannedRobot(IEnemy scannedEnemy)
        {
        }

        public virtual void OnFireBullet(double power)
        {
        }
    }
}