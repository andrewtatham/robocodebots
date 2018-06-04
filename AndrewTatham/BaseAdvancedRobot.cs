using System.Collections.Generic;
using AndrewTatham.Logic;
using AndrewTatham.Logic.Behaviors;
using AndrewTatham.Logic.Behaviors.Strategies;
using AndrewTatham.Logic.Behaviors.Strategies.Aiming.Prediction;
using Robocode;

namespace AndrewTatham
{
    public abstract class BaseAdvancedRobot : AdvancedRobot, IBaseAdvancedRobot
    {
        private readonly IBrain _brain;


        protected BaseAdvancedRobot(IEnumerable<BaseBehavior> behaviours)
        {
            IContext context = new Context(this);

            BaseBehavior.Context = context;
            BaseStrategy.Context = context;
            PredictionAlgorithm.Context = context;

            _brain = new Brain(
               this,
               behaviours,
               context);
        }

        public override void Run()
        {
            _brain.RunInit();

            while (true)
            {
                _brain.Run();
            }
            // ReSharper disable once FunctionNeverReturns
        }

        public override void OnScannedRobot(ScannedRobotEvent evnt)
        {
            _brain.OnScannedRobot(evnt);
        }

        public override void OnRobotDeath(RobotDeathEvent evnt)
        {
            _brain.OnRobotDeath(evnt);
        }

        public override void OnBulletHitBullet(BulletHitBulletEvent evnt)
        {
            _brain.OnBulletHitBullet(evnt);
        }

        public override void OnHitByBullet(HitByBulletEvent evnt)
        {
            _brain.OnHitByBullet(evnt);
        }

        public override void OnPaint(IGraphics graphics)
        {
            _brain.Render(graphics);
        }

        public override void OnSkippedTurn(SkippedTurnEvent evnt)
        {
            _brain.OnSkippedTurn(evnt);
        }
    }
}