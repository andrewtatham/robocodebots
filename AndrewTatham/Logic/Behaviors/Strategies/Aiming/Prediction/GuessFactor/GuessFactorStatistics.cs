using AndrewTatham.Helpers;
using AndrewTatham.Logic.Enemies;
using Robocode;
using System;
using System.Collections.Generic;

namespace AndrewTatham.Logic.Behaviors.Strategies.Aiming.Prediction.GuessFactor
{
    public class GuessFactorStatistics : IRender
    {
        // TODO stats decay / Rolling average
        private readonly Dictionary<GFStatKey, GFStatValue> _stats =
            new Dictionary<GFStatKey, GFStatValue>();

        public void UpdateHit(GFStatKey key, double gf)
        {
            if (-1d <= gf && gf <= 1d)
            {
                if (_stats.ContainsKey(key))
                {
                    _stats[key].Update(gf);
                }
                else
                {
                    var value = new GFStatValue();
                    value.Update(gf);
                    _stats.Add(key, value);
                }
            }
        }

        public void UpdateHit(IEnumerable<Tuple<GFStatKey, double>> gfs)
        {
            foreach (var kvp in gfs)
            {
                GFStatKey key = kvp.Item1;
                double guessFactor = kvp.Item2;

                UpdateHit(key, guessFactor);
            }
        }

        public GuessFactorData GetGuessFactorData(IEnemy enemy)
        {
            var key = new GFStatKey(enemy);

            if (_stats.ContainsKey(key))
            {
                return _stats[key].GetGuessFactorData();
            }
            else
            {
                var value = new GFStatValue();
                _stats.Add(key, value);
                return value.GetGuessFactorData();
            }
        }

        #region IRender Members

        public void Render(IGraphics graphics)
        {
            //throw new NotImplementedException();
        }

        #endregion IRender Members
    }
}