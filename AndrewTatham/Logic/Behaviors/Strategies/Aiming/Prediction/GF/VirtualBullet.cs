using System;
using AndrewTatham.Helpers;
using AndrewTatham.Logic.Enemies;
using Robocode;

namespace AndrewTatham.Logic.Behaviors.Strategies.Aiming.Prediction.GF
{
    public class VirtualBullet : IRender
    {
        private readonly double _bulletSpeed;
        private readonly Vector _directOriginal;
        private readonly long _initialTime;

        private readonly double _maxEscapeAngle;
        private readonly Vector _myLocationOriginal;
        private readonly IEnemy _target;

        private double _currentRadius;
        private Vector _directCurrent;

        private readonly GfStatKey _key;

        public VirtualBullet(IContext context)
        {
            _myLocationOriginal = new Vector(context.MyLocation);
            _target = context.Target;
            _directOriginal = new Vector(context.Target.LastBlipDirect);
            _key = new GfStatKey(context.Target);
            _initialTime = context.Time;
            _bulletSpeed = context.AimResult.Speed;
            _maxEscapeAngle = GetMaximumEscapeAngle(context.AimResult.Speed);
        }

        public bool IsFinished(IContext context, out GfStatKey key, out double? guessFactor)
        {
            guessFactor = null;
            key = null;

            long currentTime = context.Time;
            _currentRadius = _bulletSpeed * (currentTime - _initialTime);
            _directCurrent = _target.LastBlipDirect;

            // not context.Direct!, because the target may have changed
            bool isPassedEnemy = _directCurrent.Magnitude < _currentRadius;

            if (isPassedEnemy)
            {
                // the bullet has passed over the enemy
                double angle = (_directCurrent.Heading - _directOriginal.Heading).Degrees180;
                guessFactor = angle / _maxEscapeAngle;
                key = _key;
            }
            return isPassedEnemy;
        }

        public static double GetMaximumEscapeAngle(double speed)
        {
            return Math.Asin(Rules.MAX_VELOCITY / speed) * 180d / Math.PI;
        }

        #region IRender Members

        public void Render(IGraphics graphics)
        {
            if (_myLocationOriginal != null && _directOriginal != null)
            {
                new[] { _myLocationOriginal, _myLocationOriginal + _directOriginal }
                    .Render(graphics, ColourPalette.VirtualBulletPen);
            }

            if (_myLocationOriginal != null && _directCurrent != null)
            {
                new[] { _myLocationOriginal, _myLocationOriginal + _directCurrent }
                    .Render(graphics, ColourPalette.VirtualBulletPen);
            }

            if (
                _myLocationOriginal != null
                && _directOriginal != null

                //&& currentRadius != null
                //&& maxEscapeAngle != null
                )
            {
                graphics.DrawArc(
                    ColourPalette.VirtualBulletPen,
                    (int)(_myLocationOriginal.X - _currentRadius),
                    (int)(_myLocationOriginal.Y - _currentRadius),
                    (int)(2d * _currentRadius),
                    (int)(2d * _currentRadius),
                    (int)(_directOriginal.Heading.Degrees - _maxEscapeAngle),
                    (int)(2d * _maxEscapeAngle));
            }
        }

        #endregion IRender Members
    }
}