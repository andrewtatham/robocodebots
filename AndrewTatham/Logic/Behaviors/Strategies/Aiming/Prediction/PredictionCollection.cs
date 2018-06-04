using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AndrewTatham.Helpers;
using AndrewTatham.Logic.Enemies;
using Robocode;

namespace AndrewTatham.Logic.Behaviors.Strategies.Aiming.Prediction
{
    public class PredictionCollection : IRender
    {
        private readonly Dictionary<PredictionAlgorithm, Dictionary<long, Vector>> _predictions;
        private readonly Dictionary<PredictionAlgorithm, double> _scores;

        public static TextWriter Out { get; set; }

        public PredictionCollection()
        {
            _predictions = new Dictionary<PredictionAlgorithm, Dictionary<long, Vector>>();
            _scores = new Dictionary<PredictionAlgorithm, double>();
        }

        internal void Add(PredictionAlgorithm type, long absoluteTime, long flightTime, Vector futureLocation)
        {
            if (!_predictions.ContainsKey(type))
            {
                _predictions.Add(type, new Dictionary<long, Vector>());
            }

            var futureTime = absoluteTime + flightTime;
            if (_predictions[type].ContainsKey(futureTime))
            {
                _predictions[type][futureTime] = futureLocation;
            }
            else
            {
                _predictions[type].Add(futureTime, futureLocation);
            }

            // remove expired predictions for all guntypes
            foreach (var key in _predictions.Keys)
            {
                var toRemove = _predictions[key].Where(p => p.Key < absoluteTime).ToList();
                if (toRemove.Any())
                {
                    foreach (var tr in toRemove)
                    {
                        _predictions[key].Remove(tr.Key);
                    }
                }
            }
        }

        public PredictionAlgorithm GetBestPredictionAlgorithm(Blips blips, long absoluteTime)
        {
            foreach (var type in _predictions.Keys)
            {
                if (_predictions[type].Any() && blips.Any())
                {
                    double score = _predictions[type].Aggregate(0d, (seed, prediction) =>
                        {
                            Vector actual = blips.Interpolate(prediction.Key);
                            if (actual != null)
                            {
                                const double limit = 160;
                                double dx = Math.Abs(prediction.Value.X - actual.X);
                                double dy = Math.Abs(prediction.Value.Y - actual.Y);
                                double h = Math.Sqrt(dx * dx + dy * dy);
                                return seed + (h >= limit ? 0d : limit - h);
                            }
                            return seed;
                        });

                    if (_scores.ContainsKey(type))
                    {
                        _scores[type] += score;
                    }
                    else
                    {
                        _scores.Add(type, score);
                    }
                }
            }

            _scores.OrderByDescending(kvp => kvp.Value).ForEach(kvp => Out.WriteLine("Score: {1} {0}", kvp.Key, kvp.Value));

            double? maxScore = null;
            PredictionAlgorithm retval = null;
            foreach (var type in _scores.Keys)
            {
                if (!maxScore.HasValue || _scores[type] > maxScore.Value)
                {
                    maxScore = _scores[type];
                    retval = type;
                }
            }

            //foreach (var type in _scores.Keys)
            //{
            //    //clean up bad predictions
            //    if (_scores[type] <= 0d)
            //    {
            //        _predictions[type].Clear();
            //    }
            //}

            return retval;
        }

        #region IRender Members

        public void Render(IGraphics graphics)
        {
            foreach (var type in _predictions.Keys)
            {
                if (_predictions[type].Values.Any())
                    _predictions[type].Values
                        .Render(graphics, ColourPalette.MovementPen);
            }
        }

        #endregion IRender Members
    }
}