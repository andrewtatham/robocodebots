using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AndrewTatham.Helpers;
using AndrewTatham.Logic.Behaviors.Strategies.Aiming.Prediction;
using AndrewTatham.Logic.Behaviors.Strategies.Aiming.Prediction.GF;
using Robocode;

namespace AndrewTatham.Logic.Enemies
{
    public class Enemy : IEnemy
    {
        private readonly IContext _context;

        private readonly EnemyGuessFactorStatistics _enemyGuessFactorStats;

        public GuessFactorStatistics GuessFactorStats { get; private set; }

        public VirtualBullets VirtualBullets { get; private set; }

        public WaveCollection Waves { get; private set; }

        public PredictionCollection Predictions { get; private set; }

        public Blips Blips { get; private set; }

        public static TextWriter Out { get; set; }

        public Vector Location
        {
            get
            {
                return Blips.Any() ? Blips.Last().Location : null;
            }
        }

        public bool IsDead { get; private set; }

        public double Energy
        {
            get { return Blips.Last().Energy; }
        }

        public double GunHeat { get; private set; }

        public Angle Heading
        {
            get { return new Angle(Blips.Last().Heading); }
        }

        public Vector Direct
        {
            get { return Location - _context.MyLocation; }
        }

        public Vector PenultimateBlipLocation
        {
            get { return Blips.Count > 1 ? Blips.Penultimate().Location : (Blips.Any() ? Blips.Last().Location : null); }
        }

        public Vector LastBlipLocation
        {
            get { return Blips.Any() ? Blips.Last().Location : null; }
        }

        public Vector LastBlipEnemyDirect
        {
            get { return _context.MyLocation - LastBlipLocation; }
        }

        public Vector LastBlipDirect
        {
            get { return LastBlipLocation - _context.MyLocation; }
        }

        public Vector EnemyDirect
        {
            get { return _context.MyLocation - Location; }
        }

        public Vector VelocityVector { get; private set; }

        public Vector LateralVelocityVector { get; private set; }

        public Vector RadialVelocityVector { get; private set; }

        public double LateralVelocityScalar { get; private set; }

        public double RadialVelocityScalar { get; private set; }

        public Vector AccelerationVector { get; private set; }

        public Vector LateralAccelerationVector { get; private set; }

        public Vector RadialAccelerationVector { get; private set; }

        public double LateralAccelerationScalar { get; private set; }

        public double RadialAccelerationScalar { get; private set; }

        public Enemy(IContext context, ScannedRobotEvent evnt)
        {
            _context = context;
            var name = evnt.Name;
            VirtualBullets = new VirtualBullets();
            GuessFactorStats = new GuessFactorStatistics();
            Waves = new WaveCollection();
            Blips = new Blips();
            _enemyGuessFactorStats = new EnemyGuessFactorStatistics();

            Predictions = new PredictionCollection();
            MyBullets = new BulletCollection(name);
            OnScannedRobot(context, evnt);
        }

        public void OnScannedRobot(IContext context, ScannedRobotEvent evnt)
        {
            Blips.OnScannedRobot(context, evnt);

            if (Blips.Any())
            {
                VelocityVector = new Vector(Blips.Last().Velocity, Blips.Last().Heading);
                LateralVelocityScalar = VelocityVector.Magnitude *
                                        Math.Sin((VelocityVector.Heading - Direct.Heading).Radians);
                RadialVelocityScalar = VelocityVector.Magnitude *
                                       Math.Cos((VelocityVector.Heading - Direct.Heading).Radians);
                LateralVelocityVector = new Vector(LateralVelocityScalar, Direct.Heading.Perpendicular);
                RadialVelocityVector = new Vector(RadialVelocityScalar, Direct.Heading);

                if (Blips.Count > 1)
                {
                    Vector previousVelocity = new Vector(Blips.Penultimate().Velocity, Blips.Penultimate().Heading);
                    AccelerationVector = (VelocityVector - previousVelocity) /
                                        (Blips.Last().Time - Blips.Penultimate().Time);
                    LateralAccelerationScalar = AccelerationVector.Magnitude *
                                               Math.Sin((AccelerationVector.Heading - Direct.Heading).Radians);
                    RadialAccelerationScalar = AccelerationVector.Magnitude *
                                              Math.Cos((AccelerationVector.Heading - Direct.Heading).Radians);
                    LateralAccelerationVector = new Vector(LateralAccelerationScalar,
                                                          Direct.Heading.Perpendicular);
                    RadialAccelerationVector = new Vector(RadialAccelerationScalar, Direct.Heading);
                }
            }
        }

        public BulletCollection MyBullets { get; private set; }

        public double? BulletPower { get; set; }

        public void RegisterHitByBullet(HitByBulletEvent evnt)
        {
            if (Waves != null)
            {
                double? gf = Waves.RegisterHitByBullet(evnt);
                if (_enemyGuessFactorStats != null && gf != null)
                {
                    _enemyGuessFactorStats.Update(gf.Value);
                }
            }
        }

        public void RegisterBulletHitBullet(BulletHitBulletEvent evnt)
        {
            if (Waves != null)
            {
                double? gf = Waves.RegisterBulletHitBullet(evnt);
                if (_enemyGuessFactorStats != null && gf != null)
                {
                    _enemyGuessFactorStats.Update(gf.Value);
                }
            }
        }

        public void RegisterRobotDeath(string name)
        {
            IsDead = true;
        }

        public GuessFactorData GetGuessFactorData()
        {
            GuessFactorData value = GuessFactorStats.GetGuessFactorData(this);

            return value;
        }

        public EnemyGuessFactorData EnemyGuessFactorData
        {
            get
            {
                if (_enemyGuessFactorStats != null)
                    return _enemyGuessFactorStats.GuessFactorData;
                return null;
            }
        }

        public void OnTurn(Context context)
        {
            GunHeat = Math.Max(0d, GunHeat - context.GunCoolingRate);
        }

        public IEnumerable<Vector> GetPifData(long delta)
        {
            var pif = Blips.GetPifRelativeVectors(delta);
            if (pif != null && pif.Any())
            {
                return pif;
            }
            return null;
        }

        public void Render(IGraphics graphics)
        {
            if (Blips != null)
            {
                Blips.Render(graphics);
            }

            if (VirtualBullets != null)
            {
                VirtualBullets.Render(graphics);
            }
            if (Waves != null)
            {
                Waves.Render(graphics);
            }

            if (GuessFactorStats != null)
            {
                GuessFactorStats.Render(graphics);
            }
            if (_enemyGuessFactorStats != null)
            {
                _enemyGuessFactorStats.Render(graphics);
            }
            if (Predictions != null)
            {
                Predictions.Render(graphics);
            }

            //if (Location != null && LateralVelocity != null)
            //{
            //    graphics.DrawPolygon(Pens.Yellow,
            //        new PointF[]
            //        {
            //            Location.ToPointF(),
            //            (Location + 20d * LateralVelocity).ToPointF()
            //        });
            //}
            //if (Location != null && RadialVelocity != null)
            //{
            //    graphics.DrawPolygon(Pens.Yellow,
            //        new PointF[]
            //        {
            //            Location.ToPointF(),
            //            (Location + 20d * RadialVelocity).ToPointF()
            //        });
            //}

            //if (Location != null && LateralAceleration != null)
            //    //{
            //    graphics.DrawPolygon(Pens.Orange,
            //        new PointF[]
            //        {
            //            Location.ToPointF(),
            //            (Location + 100d * LateralAceleration).ToPointF()
            //        });

            //if (Location != null && RadialAceleration != null)
            //{
            //    graphics.DrawPolygon(Pens.Orange,
            //        new PointF[]
            //        {
            //            Location.ToPointF(),
            //            (Location + 100d * RadialAceleration).ToPointF()
            //        });
            //}
        }
    }
}