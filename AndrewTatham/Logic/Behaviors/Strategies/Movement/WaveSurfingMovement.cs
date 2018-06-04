using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using AndrewTatham.Helpers;
using AndrewTatham.Logic.Behaviors.Strategies.Aiming.Prediction.GF;
using AndrewTatham.Logic.Enemies;
using Robocode;

namespace AndrewTatham.Logic.Behaviors.Strategies.Movement
{
    public class WaveSurfingMovement : BaseStrategy
    {
        private Vector _direct;
        private Vector _guessFactorRelativeVector;
        private Vector _location;
        private double _maxEscapeAngle;
        private Vector _maxEscapeAngleRelativeVector;
        private Vector _minEscapeAngleRelativeVector;
        private double _power;
        private double _speed;
        private int[] _stats;

        public override void Execute()
        {
            Contract.Requires(Context != null);
            Contract.Requires(Context != null);

            if (Context.Target != null
                && Context.Target.Location != null
                && Context.Target.EnemyDirect != null)
            {
                _location = Context.Target.Location;
                _direct = Context.Target.EnemyDirect;

                // TODO POWER
                _power = Rules.MIN_BULLET_POWER + 0.2 * (Rules.MAX_BULLET_POWER - Rules.MIN_BULLET_POWER);
                _speed = Rules.GetBulletSpeed(_power);
                _maxEscapeAngle = VirtualBullet.GetMaximumEscapeAngle(_speed);

                EnemyGuessFactorData gfData = Context.Target.EnemyGuessFactorData;

                double gf = gfData.GuessFactor;
                _stats = gfData.Statistics;

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
                        + gf * _maxEscapeAngle));
            }

            if (_guessFactorRelativeVector != null)
            {
                Context.MoveToAbsolute += new Vector(
                    _guessFactorRelativeVector.Magnitude *
                    Math.Sin((_guessFactorRelativeVector.Heading - _direct.Heading).Radians),
                    _guessFactorRelativeVector.Heading.Perpendicular);
            }
        }

        public override void Render(IGraphics graphics)
        {
            if (_location != null)
            {
                var vs = new List<Vector[]>();

                if (_direct != null) vs.Add(new[] { _location, _location + _direct });
                if (_minEscapeAngleRelativeVector != null)
                    vs.Add(new[] { _location, _location + _minEscapeAngleRelativeVector });
                if (_maxEscapeAngleRelativeVector != null)
                    vs.Add(new[] { _location, _location + _maxEscapeAngleRelativeVector });
                if (_guessFactorRelativeVector != null) vs.Add(new[] { _location, _location + _guessFactorRelativeVector });

                vs.ForEach(v => v.Render(graphics, ColourPalette.WaveSurfingMovementPen));
            }

            if (_stats != null
                && _location != null
                && _minEscapeAngleRelativeVector != null
                && _maxEscapeAngleRelativeVector != null)
            {
                Vector origin = _location + _minEscapeAngleRelativeVector;
                Vector baseline = _location + _maxEscapeAngleRelativeVector - origin;

                for (int i = 0; i < _stats.Length; i++)
                {
                    Vector centre = origin + baseline * (i / (double)_stats.Length);
                    int radius = _stats[i];
                    graphics.FillEllipse(
                        ColourPalette.WaveSurfingMovementBrush,
                        (float)(centre.X - radius),
                        (float)(centre.Y - radius),
                        (float)(2d * radius),
                        (float)(2d * radius));
                }
            }
        }
    }
}