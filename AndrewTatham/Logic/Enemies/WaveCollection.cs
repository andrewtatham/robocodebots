using System.Collections.Generic;
using AndrewTatham.Helpers;
using Robocode;

namespace AndrewTatham.Logic.Enemies
{
    public class WaveCollection : IRender
    {
        private readonly Queue<Wave> _queue = new Queue<Wave>();

        #region IRender Members

        public void Render(IGraphics graphics)
        {
            if (_queue.Count > 0)
            {
                _queue.Peek().Render(graphics);
            }
        }

        #endregion IRender Members

        public void Update(IContext context)
        {
            bool updated = true;

            while (updated)
            {
                updated = _queue.Count > 0
                          && _queue.Peek().HasWavePassedMe(context);

                if (updated) _queue.Dequeue();
            }
        }

        public double? RegisterHitByBullet(HitByBulletEvent evnt)
        {
            if (_queue.Count > 0)
            {
                return _queue.Peek().GetGuessFactor(evnt);
            }
            return null;
        }

        public double? RegisterBulletHitBullet(BulletHitBulletEvent evnt)
        {
            if (_queue.Count > 0)
            {
                return _queue.Peek().GetGuessFactor(evnt);
            }
            return null;
        }

        public void Add(Wave wave)
        {
            if (wave != null)
            {
                _queue.Enqueue(wave);
            }
        }
    }
}