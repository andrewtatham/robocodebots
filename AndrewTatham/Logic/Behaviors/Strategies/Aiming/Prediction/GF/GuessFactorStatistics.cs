using System;
using System.Collections.Generic;
using AndrewTatham.Helpers;
using AndrewTatham.Logic.Enemies;
using Robocode;

namespace AndrewTatham.Logic.Behaviors.Strategies.Aiming.Prediction.GF
{
    public class GuessFactorStatistics : IRender
    {
        // TODO stats decay / Rolling average
        private readonly Dictionary<GfStatKey, GfStatValue> _stats =
            new Dictionary<GfStatKey, GfStatValue>();

        public void UpdateHit(GfStatKey key, double gf)
        {
            if (-1d <= gf && gf <= 1d)
            {
                if (_stats.ContainsKey(key))
                {
                    _stats[key].Update(gf);
                }
                else
                {
                    var value = new GfStatValue();
                    value.Update(gf);
                    _stats.Add(key, value);
                }
            }
        }

        public void UpdateHit(IEnumerable<Tuple<GfStatKey, double>> gfs)
        {
            foreach (var kvp in gfs)
            {
                GfStatKey key = kvp.Item1;
                double guessFactor = kvp.Item2;

                UpdateHit(key, guessFactor);
            }
        }

        public GuessFactorData GetGuessFactorData(IEnemy enemy)
        {
            var key = new GfStatKey(enemy);

            if (_stats.ContainsKey(key))
            {
                return _stats[key].GetGuessFactorData();
            }
            var value = new GfStatValue();
            _stats.Add(key, value);
            return value.GetGuessFactorData();
        }

        #region IRender Members

        public void Render(IGraphics graphics)
        {
            //throw new NotImplementedException();
        }

        #endregion IRender Members
    }
}