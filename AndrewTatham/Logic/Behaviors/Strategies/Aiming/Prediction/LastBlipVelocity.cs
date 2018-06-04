using AndrewTatham.Helpers;
using AndrewTatham.Logic.Enemies;

namespace AndrewTatham.Logic.Behaviors.Strategies.Aiming.Prediction
{
    public class LastBlipVelocity : PredictionAlgorithm
    {
        public override Vector GetFuturePosition(IEnemy target, long delta)
        {
            if (target != null)
            {
                Vector initialLocation = target.Blips.Last().Location;
                var velocity = new Vector(target.Blips.Last().Velocity,
                                          new Angle(target.Blips.Last().Heading));

                Vector futurePosition = initialLocation + delta * velocity;

                return futurePosition;
            }

            return null;
        }
    }
}