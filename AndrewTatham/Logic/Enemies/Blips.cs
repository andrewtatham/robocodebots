using System;
using System.Collections.Generic;
using System.Linq;
using AndrewTatham.Helpers;
using Robocode;

namespace AndrewTatham.Logic.Enemies
{
    public class Blips : IRender
    {
        private readonly List<Blip> _blips = new List<Blip>();

        public int Count
        {
            get { return _blips.Count; }
        }

        public Blip Penultimate()
        {
            return _blips[_blips.Count - 2];
        }

        public Blip Last()
        {
            return _blips.Last();
        }

        public bool Any()
        {
            return _blips.Any();
        }

        public void Add(Blip blip)
        {
            _blips.Add(blip);

            if (_blips.Count > 20)
                _blips.RemoveAt(0);
        }

        public Blip ThirdFromLast()
        {
            return _blips[_blips.Count - 2];
        }

        public double Average(Func<Blip, double> func)
        {
            return _blips.Average(func);
        }

        public IEnumerable<double> Select(Func<Blip, double> func)
        {
            return _blips.Select(func);
        }

        public IEnumerable<Blip> Last(int p)
        {
            return _blips.Last(p);
        }

        public Vector Interpolate(long t)
        {
            Blip prev = _blips.LastOrDefault(b => b.Time < t);
            Blip next = _blips.FirstOrDefault(b => b.Time >= t);
            if (prev != null && next != null)
            {
                double factor = (t - prev.Time) / (double)(next.Time - prev.Time);
                return prev.Location + factor * (next.Location - prev.Location);
            }
            return null;
        }

        #region IRender Members

        public void Render(IGraphics graphics)
        {
            if (_blips.Any())
            {
                _blips.Last(20)
                    .Select(blip => blip.Location)
                    .Render(graphics, ColourPalette.BlipPen);
            }
        }

        #endregion IRender Members

        internal void OnScannedRobot(IContext context, ScannedRobotEvent evnt)
        {
            var blip = new Blip(context.MyLocation, context.MyHeading, evnt);
            _blips.Add(blip);
        }

        public IEnumerable<Vector> GetPifRelativeVectors(long deltaTime)
        {
            if (_blips.Any())
            {
                return _blips.Last(20).Select(blip => new
                {
                    InitialLocation = blip.Location,
                    InitialHeading = new Angle(blip.Heading),
                    FinalLocation = Interpolate(blip.Time + deltaTime)
                })
                .Where(x => x.FinalLocation != null)
                .Select(x =>
                {
                    var diff = x.FinalLocation - x.InitialLocation;
                    return new Vector(diff.Magnitude,
                        diff.Heading - x.InitialHeading);
                });
            }
            return null;
        }
    }
}