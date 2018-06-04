using AndrewTatham.Helpers;
using Robocode;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AndrewTatham.Logic.Behaviors.Strategies.Aiming.Prediction.GuessFactor
{
    public class VirtualBullets : IRender
    {
        private readonly Queue<VirtualBullet> _vbs = new Queue<VirtualBullet>();

        #region IRender Members

        public void Render(IGraphics graphics)
        {
            if (_vbs.Any())
            {
                _vbs.Peek().Render(graphics);
            }
        }

        #endregion IRender Members

        public List<Tuple<GFStatKey, double>> Update(IContext context)
        {
            var guessFactorArray = new List<Tuple<GFStatKey, double>>();

            bool removeVirtualBullet = true;

            while (removeVirtualBullet)
            {
                double? guessFactor = null;
                GFStatKey key = null;

                removeVirtualBullet = _vbs.Any() && _vbs.Peek().IsFinished(context, out key, out guessFactor);

                if (removeVirtualBullet && key != null && guessFactor.HasValue)
                {
                    guessFactorArray.Add(new Tuple<GFStatKey, double>(key, guessFactor.Value));
                }
                if (removeVirtualBullet) _vbs.Dequeue();
            }

            return guessFactorArray;
        }

        public void Add(VirtualBullet virtualBullet)
        {
            _vbs.Enqueue(virtualBullet);
        }
    }
}