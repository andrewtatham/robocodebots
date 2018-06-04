using System;
using System.Collections.Generic;
using System.Linq;
using AndrewTatham.Helpers;
using AndrewTatham.Logic.Behaviors.Strategies.Aiming.Prediction;
using AndrewTatham.Logic.Enemies;
using Robocode;

namespace AndrewTatham.Logic.Behaviors.Strategies.Aiming
{
    public class PredictionAlgorithms : BaseStrategy
    {
        private readonly IEnumerable<PredictionAlgorithm> _predictors;
        private PredictionAlgorithm _best;

        public PredictionAlgorithms(IEnumerable<PredictionAlgorithm> algo)
        {
            _predictors = algo;
        }

        public override void OnScannedRobot(IEnemy scannedEnemy)
        {
            if (scannedEnemy != null)
            {
                foreach (var predictionAlgorithm in _predictors)
                {
                    predictionAlgorithm.OnScannedRobot(scannedEnemy);
                }

                // GET THE BEST ALGORITHM
                _best = _predictors.Count() == 1 ? _predictors.Single() : scannedEnemy.Predictions.GetBestPredictionAlgorithm(scannedEnemy.Blips, Context.Time);

                if (_best != null) Context.Out.WriteLine("Best: {0}", _best);

                // mAKE FUTURE PREDICTIONS
                if (scannedEnemy.BulletPower.HasValue && scannedEnemy.Direct != null)
                {
                    foreach (var predictionAlgorithm in _predictors)
                    {
                        Double bulletPower = scannedEnemy.BulletPower.Value;
                        long flightTime = (long)(scannedEnemy.Direct.Magnitude / Rules.GetBulletSpeed(bulletPower));

                        var futurePosition = predictionAlgorithm.GetFuturePosition(scannedEnemy, flightTime);
                        if (futurePosition != null)
                        {
                            scannedEnemy.Predictions.Add(
                                predictionAlgorithm,
                                Context.Time,
                                flightTime,
                                futurePosition);
                        }
                    }
                }
            }
        }

        public override void OnFireBullet(double power)
        {
            foreach (var predictionAlgorithm in _predictors)
            {
                predictionAlgorithm.OnFireBullet(power);
            }
        }

        #region IRender Members

        public override void Render(IGraphics graphics)
        {
            if (_best != null)
            {
                _best.Render(graphics);
            }

            //foreach (var predictionAlgorithm in _predictors)
            //{
            //    predictionAlgorithm.Render(graphics);
            //}
        }

        #endregion IRender Members

        public override void Execute()
        {
            Vector future = null;
            if (Context.Target != null
                && Context.Target.Direct != null
                && Context.Target.BulletPower.HasValue
                && _best != null)
            {
                double distance = Context.Target.Direct.Magnitude;

                Out.WriteLine("Predicting: {0}", _best);

                for (int i = 0; i < 5; i++)
                {
                    var bulletTime = (long)(distance / Rules.GetBulletSpeed(Context.Target.BulletPower.Value));

                    future = _best.GetFuturePosition(Context.Target, bulletTime);
                    if (future != null)
                    {
                        // limit to within game grid (esp wallbots)
                        future = new Vector(
                            Math.Min(Context.BattlefieldWidth, Math.Max(0, future.X)),
                            Math.Min(Context.BattlefieldHeight, Math.Max(0, future.Y)));
                        distance = new Vector(Context.MyLocation, future).Magnitude;
                    }
                    else break;
                }

                if (future != null)
                {
                    Context.AimResult = new AimResult
                    {
                        Target = Context.Target,
                        Location = future,
                        Power = Context.Target.BulletPower.Value
                    };
                }
                else
                {
                    Context.AimResult = null;
                }
            }
            else
            {
                Context.AimResult = null;
            }
        }
    }
}