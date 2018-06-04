using System.Linq;
using AndrewTatham.Helpers;
using AndrewTatham.Logic.Behaviors.Strategies.Aiming.Prediction.GF;
using AndrewTatham.Logic.Enemies;
using Robocode;

namespace AndrewTatham.Logic.Behaviors.Strategies.Aiming.Prediction
{
    public class GuessFactorAiming : PredictionAlgorithm
    {
        private Vector _direct;
        private double _guessFactor;
        private Vector _guessFactorRelativeVector;
        private Vector _location;
        private double _maxEscapeAngle;
        private Vector _maxEscapeAngleRelativeVector;
        private Vector _minEscapeAngleRelativeVector;
        private double _power;
        private double _speed;
        private int[] _stats;

        public override void Render(IGraphics graphics)
        {
            //if (_location != null && _direct != null)
            //    new Vector[] { _location, (_location + _direct) }
            //        .Render(graphics, ColourPallette.GuessFactorPen);
            //if (_location != null && _minEscapeAngleRelativeVector != null)
            //    new Vector[] { _location, (_location + _minEscapeAngleRelativeVector) }
            //        .Render(graphics, ColourPallette.GuessFactorPen);
            //if (_location != null && _maxEscapeAngleRelativeVector != null)
            //    new Vector[] { _location, (_location + _maxEscapeAngleRelativeVector) }
            //        .Render(graphics, ColourPallette.GuessFactorPen);
            //if (_location != null && _guessFactorRelativeVector != null)
            //    new Vector[] { _location, (_location + _guessFactorRelativeVector) }
            //        .Render(graphics, ColourPallette.GuessFactorPen);

            if (_stats != null
                && _location != null
                && _minEscapeAngleRelativeVector != null
                && _maxEscapeAngleRelativeVector != null)
            {
                Vector origin = _location + _minEscapeAngleRelativeVector;
                Vector baseline = _location + _maxEscapeAngleRelativeVector - origin;
                Angle perp = baseline.Heading.Perpendicular;

                new[] { origin, origin + baseline }.Render(graphics, ColourPalette.GuessFactorPen);

                for (int i = 0; i < _stats.Length; i++)
                {
                    if (_stats[i] > 0)
                    {
                        Vector dataPointBottom = origin + baseline * (i / (double)_stats.Length);
                        var dataPointTop = new Vector(_stats[i], perp);
                        new[]
                            {
                                dataPointBottom,
                                dataPointBottom + dataPointTop
                            }
                            .Render(graphics, ColourPalette.GuessFactorPen);
                    }
                }
            }
        }

        public override Vector GetFuturePosition(IEnemy target, long delta)
        {
            if (
                Context != null
                && Context.MyLocation != null
                && target != null
                && target.LastBlipDirect != null
                && target.BulletPower.HasValue
                )
            {
                GuessFactorData gfData = target.GetGuessFactorData();
                if (gfData != null)
                {
                    _stats = gfData.Data;
                    _guessFactor = target.Energy > 0d ? gfData.GuessFactor : 0d;

                    _location = Context.MyLocation;
                    _direct = target.LastBlipDirect;

                    _power = target.BulletPower.Value;
                    _speed = Rules.GetBulletSpeed(_power);
                    _maxEscapeAngle = VirtualBullet.GetMaximumEscapeAngle(_speed);

                    _minEscapeAngleRelativeVector = new Vector(
                        _direct.Magnitude,
                        new Angle(
                            _direct.Heading.Degrees
                            - _maxEscapeAngle));

                    _maxEscapeAngleRelativeVector = new Vector(
                        _direct.Magnitude,
                        new Angle(
                            _direct.Heading.Degrees
                            + _maxEscapeAngle));

                    _guessFactorRelativeVector = new Vector(
                        _direct.Magnitude,
                        new Angle(
                            _direct.Heading.Degrees
                            + _guessFactor * _maxEscapeAngle));

                    return _location + _guessFactorRelativeVector;
                }
            }

            return null;
        }

        public override void OnScannedRobot(IEnemy scannedEnemy)
        {
            if (scannedEnemy.Blips.Count > 1)
            {
                double energyDrop = scannedEnemy.Blips.Penultimate().Energy - scannedEnemy.Blips.Last().Energy;
                if (Rules.MIN_BULLET_POWER <= energyDrop && energyDrop <= Rules.MAX_BULLET_POWER)
                {
                    scannedEnemy.Waves.Add(new Wave(Context, scannedEnemy, energyDrop));
                }
            }

            scannedEnemy.Waves.Update(Context);

            if (Context != null && Context.AimResult != null)
            {
                scannedEnemy.VirtualBullets.Add(new VirtualBullet(Context));
            }

            if (scannedEnemy.VirtualBullets != null)
            {
                var newGuessFactors = scannedEnemy.VirtualBullets.Update(Context);
                if (newGuessFactors != null && newGuessFactors.Any())
                {
                    scannedEnemy.GuessFactorStats.UpdateHit(newGuessFactors);
                }
            }
        }

        public override void OnFireBullet(double power)
        {
        }
    }
}