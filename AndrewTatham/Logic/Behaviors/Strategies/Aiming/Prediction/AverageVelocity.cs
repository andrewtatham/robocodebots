using System;
using System.Linq;
using AndrewTatham.Helpers;
using AndrewTatham.Logic.Enemies;

namespace AndrewTatham.Logic.Behaviors.Strategies.Aiming.Prediction
{
    [Obsolete]
    public class AverageVelocity : PredictionAlgorithm
    {
        public override Vector GetFuturePosition(IEnemy target, long delta)
        {
            const int n = 20;
            if (target != null)
            {
                Vector result = target.Blips.Last().Location;
                return target.Blips.Last(n).Aggregate(result, (current, blip) => current + new Vector(delta*blip.Velocity/n, new Angle(blip.Heading)));
            }
            return null;
        }
    }
}