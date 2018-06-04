using System;
using System.Linq;
using AndrewTatham.Helpers;
using AndrewTatham.Logic.Enemies;

namespace AndrewTatham.Logic.Behaviors.Strategies.Aiming.Prediction
{
    public class AveragePosition : PredictionAlgorithm
    {
        public override Vector GetFuturePosition(IEnemy target, long delta)
        {
            const int n = 20;
            if (target != null && target.Blips != null && target.Blips.Any())
            {
                var lastBlips = target.Blips.Last(n).ToList();
                Vector result = target.Blips.Last().Location;
                for (int i = 0; i < n; i++)
                {
                    double weighting = Convert.ToDouble(i) / Convert.ToDouble(n);
                    result += weighting * (lastBlips[i].Location - target.Blips.Last().Location);
                }
                return result;
            }
            return null;
        }
    }
}