using AndrewTatham.Helpers;
using AndrewTatham.Logic.Enemies;
using Robocode;

namespace AndrewTatham.Logic.Behaviors.Strategies.Aiming.Prediction
{
    public abstract class PredictionAlgorithm : IRender
    {
        public static IContext Context { get; set; }

        public abstract Vector GetFuturePosition(IEnemy target, long delta);

        public virtual void OnScannedRobot(IEnemy scannedEnemy)
        {
        }

        public virtual void OnFireBullet(double power)
        {
        }

        public virtual void Render(IGraphics graphics)
        {
        }
    }
}