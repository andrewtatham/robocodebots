using System.Collections.Generic;
using System.IO;
using AndrewTatham.Helpers;
using AndrewTatham.Logic.Behaviors.Strategies;
using AndrewTatham.Logic.Enemies;
using Robocode;

namespace AndrewTatham.Logic.Behaviors
{
    public abstract class BaseBehavior : IRender
    {
        public static IContext Context { get; set; }

        public readonly List<BaseStrategy> Strategies = new List<BaseStrategy>();

        public static TextWriter Out { get; set; }

        public abstract bool Condition();

        public virtual void Execute()
        {
            foreach (BaseStrategy strategy in Strategies)
            {
                Out.WriteLine("Execute: {0}", strategy);
                strategy.Execute();
            }
        }

        public virtual void OnFireBullet(double power)
        {
            foreach (BaseStrategy strategy in Strategies)
            {
                strategy.OnFireBullet(power);
            }
        }

        public virtual void OnScannedRobot(IEnemy scannedEnemy)
        {
            foreach (BaseStrategy strategy in Strategies)
            {
                Out.WriteLine("OnScannedRobot: {0}", strategy);
                strategy.OnScannedRobot(scannedEnemy);
            }
        }

        public virtual void Render(IGraphics graphics)
        {
            foreach (BaseStrategy strategy in Strategies)
            {
                Out.WriteLine("Render: {0}", strategy);
                strategy.Render(graphics);
            }
        }
    }
}