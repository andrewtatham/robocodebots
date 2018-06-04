using System.Collections.Generic;
using System.Linq;
using AndrewTatham.Helpers;
using AndrewTatham.Logic.Enemies;
using Robocode;

namespace AndrewTatham.Logic.Behaviors.Strategies.Aiming.Prediction
{
    public class PlayItForward : PredictionAlgorithm
    {
        private Vector _targetLocation;
        private IEnumerable<Vector> _abs;

        public override Vector GetFuturePosition(IEnemy target, long delta)
        {
            const int roundTo = 20;
            var relative = target.GetPifData(delta);

            if (relative != null && relative.Any())
            {
                _targetLocation = target.Location;
                _abs = relative.Select(r => target.Location
                        + new Vector(r.Magnitude, target.Heading + r.Heading));

                var mode = relative.Mode(roundTo);

                if (mode != null)
                {
                    return target.Location
                        + new Vector(mode.Magnitude,
                            target.Heading + mode.Heading);
                }
            }
            return null;
        }

        public override void Render(IGraphics graphics)
        {
            if (_targetLocation != null && _abs != null && _abs.Any())
            {
                _abs.Select(a => new[] { _targetLocation, a })
                    .ForEach(x => x.Render(graphics, ColourPalette.PifColour));
            }
        }
    }
}