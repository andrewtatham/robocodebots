using AndrewTatham.Helpers;
using AndrewTatham.Logic.Enemies;

namespace AndrewTatham.Logic.Behaviors.Strategies.Aiming.Prediction
{
    public class CurrentPosition : PredictionAlgorithm
    {
        public override Vector GetFuturePosition(IEnemy target, long delta)
        {
            if (target != null && target.Blips.Any())
            {
                return target.Blips.Last().Location;
            }

            return null;
        }
    }
}