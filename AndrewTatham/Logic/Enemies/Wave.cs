using AndrewTatham.Helpers;
using AndrewTatham.Logic.Behaviors.Strategies.Aiming.Prediction.GF;
using Robocode;

namespace AndrewTatham.Logic.Enemies
{
    public class Wave : IRender
    {
        private readonly double _bulletSpeed;
        private readonly Vector _initialEnemyDirect;
        private readonly Vector _initialEnemyLocation;
        private readonly Vector _initialMyLocation;

        private readonly long _initialTime;
        private readonly double _maxEscapeAngle;
        private Vector _currentDirect;
        private Vector _currentMyLocation;
        private double _currentRadius;

        public Wave(IContext context, IEnemy enemy, double waveEnergy)
        {
            // Use previous Because the bullet was fired on the previous go
            _initialEnemyLocation = enemy.PenultimateBlipLocation;
            _initialMyLocation = context.MyLocation;
            _initialEnemyDirect = _initialMyLocation - _initialEnemyLocation;
            _initialTime = context.Time;

            _bulletSpeed = Rules.GetBulletSpeed(waveEnergy);
            _maxEscapeAngle = VirtualBullet.GetMaximumEscapeAngle(_bulletSpeed);
        }

        public bool HasWavePassedMe(IContext context)
        {
            _currentMyLocation = context.MyLocation;
            _currentDirect = context.MyLocation - _initialEnemyLocation;
            _currentRadius = _bulletSpeed * (context.Time - _initialTime);

            if (_currentDirect.Magnitude <= _currentRadius)
            {
                return true;
            }
            return false;
        }

        public double? GetGuessFactor(HitByBulletEvent hitByBulletEvent)
        {
            // todo

            double gf = new Angle(hitByBulletEvent.Heading - _initialEnemyDirect.Heading.Degrees).Degrees180 / (2.0 * _maxEscapeAngle);
            return gf;
        }

        public double? GetGuessFactor(BulletHitBulletEvent bulletHitBulletEvent)
        {
            // todo
            double gf = new Angle(bulletHitBulletEvent.HitBullet.Heading - _initialEnemyDirect.Heading.Degrees).Degrees180 /
                        (2.0 * _maxEscapeAngle);
            return gf;
        }

        #region IRender Members

        public void Render(IGraphics graphics)
        {
            if (_initialMyLocation != null && _initialEnemyLocation != null)
            {
                new[]
                    {
                        _initialEnemyLocation,
                        _initialMyLocation
                    }.Render(graphics, ColourPalette.WavePen);
            }

            if (_initialEnemyLocation != null && _currentMyLocation != null)
            {
                new[]
                    {
                        _initialEnemyLocation,
                        _currentMyLocation
                    }.Render(graphics, ColourPalette.WavePen);
            }

            if (
                _initialEnemyLocation != null
                && _initialEnemyDirect != null
                )
            {
                graphics.DrawArc(
                    ColourPalette.WavePen,
                    (int)(_initialEnemyLocation.X - _currentRadius),
                    (int)(_initialEnemyLocation.Y - _currentRadius),
                    (int)(2d * _currentRadius),
                    (int)(2d * _currentRadius),
                    (int)(_initialEnemyDirect.Heading.Degrees - _maxEscapeAngle),
                    (int)(2d * _maxEscapeAngle));
            }
        }

        #endregion IRender Members
    }
}