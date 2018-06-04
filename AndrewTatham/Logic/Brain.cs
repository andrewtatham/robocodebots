using System;
using System.Collections.Generic;
using System.Drawing;
using AndrewTatham.Helpers;
using AndrewTatham.Logic.Behaviors;
using AndrewTatham.Logic.Behaviors.Strategies;
using AndrewTatham.Logic.Behaviors.Strategies.Aiming.Prediction;
using AndrewTatham.Logic.Behaviors.Strategies.Movement.Forces;
using AndrewTatham.Logic.Behaviors.Strategies.Radar;
using AndrewTatham.Logic.Enemies;
using Robocode;

namespace AndrewTatham.Logic
{
    public class Brain : IBrain
    {
        private readonly IBaseAdvancedRobot _advancedRobot;
        private readonly IEnumerable<BaseBehavior> _behaviours;

        private readonly IContext _context;

        public BaseBehavior SelectedBehavior { get; private set; }

        public Brain(IBaseAdvancedRobot bot, IEnumerable<BaseBehavior> behaviours, IContext iContext)
        {
            _behaviours = behaviours;
            _advancedRobot = bot;
            _context = iContext;
            SelectedBehavior = null;
        }

        public void RunInit()
        {
            _advancedRobot.SetColors(Color.Magenta, Color.Magenta, Color.Magenta);

            _advancedRobot.IsAdjustGunForRobotTurn = true;
            _advancedRobot.IsAdjustRadarForGunTurn = true;
            _advancedRobot.IsAdjustRadarForRobotTurn = true;

            BaseBehavior.Out = _advancedRobot.Out;
            BaseStrategy.Out = _advancedRobot.Out;
            Enemy.Out = _advancedRobot.Out;
            PredictionCollection.Out = _advancedRobot.Out;
            Force.Context = _context;

            //ForceCollection.N = 5;
            //ForceCollection.BattleFieldWidth = _advancedRobot.BattleFieldWidth;
            //ForceCollection.BattleFieldHeight = _advancedRobot.BattleFieldHeight;

            _context.RunInit();
        }

        public void Run()
        {
            ExecuteFireGun();
            NewTurn();
            Think();
            Radar();
            Move();
            Aim();
            Execute();
        }

        public void Render(IGraphics graphics)
        {
            //if (_behaviours != null)
            //{
            //    foreach (BaseBehavior behaviour in _behaviours)
            //    {
            //        _advancedRobot.Out.WriteLine("Render: {0}", behaviour);
            //        behaviour.Render(graphics);
            //    }
            //}
            if (SelectedBehavior != null)
            {
                _advancedRobot.Out.WriteLine("Render: {0}", SelectedBehavior);
                SelectedBehavior.Render(graphics);
            }

            if (_context != null)
            {
                _advancedRobot.Out.WriteLine("Render: {0}", _context);
                _context.Render(graphics);
            }
        }

        #region private methods

        private void Think()
        {
            _advancedRobot.Out.WriteLine("Thinking");

            if (_behaviours == null) return;
            foreach (BaseBehavior behaviour in _behaviours)
            {
                _advancedRobot.Out.WriteLine("Checking: {0}", behaviour);
                if (!behaviour.Condition()) continue;

                SelectedBehavior = behaviour;
                break;
            }

            _advancedRobot.Out.WriteLine("Execute: {0}", SelectedBehavior);
            SelectedBehavior.Execute();
        }

        private void NewTurn()
        {
            _context.NewTurn();
        }

        private void Radar()
        {
            _advancedRobot.Out.WriteLine("Radar");
            if (_context != null
                && _context != null
                && _context.RadarResult != null)
            {
                switch (_context.RadarResult.ScanType)
                {
                    case ScanType.FullScan:
                        _advancedRobot.SetTurnRadarRight(360);
                        _advancedRobot.Out.WriteLine("Radar Full Scan");
                        break;

                    case ScanType.TurnTo:
                        _advancedRobot.Out.WriteLine("Turning Radar to {0}", _context.RadarResult.TurnTo.Degrees);
                        TurnRadarTo(_context.RadarResult.TurnTo);
                        break;
                }
            }
            else
            {
                _advancedRobot.Out.WriteLine("Radar Full Scan");
                _advancedRobot.SetTurnRadarRight(360);
            }
        }

        private void Aim()
        {
            _advancedRobot.Out.WriteLine("Aiming");
            if (_context != null
                && _context.AimResult != null
                && _context.AimResult.Location != null)
            {
                Angle newGunHeading = (_context.AimResult.Location - new Vector(_advancedRobot.X, _advancedRobot.Y)).Heading;
                _advancedRobot.Out.WriteLine("Turning Gun to {0}", newGunHeading.Degrees);
                TurnGunToHeading(newGunHeading);
            }
        }

        private void ExecuteFireGun()
        {
            _advancedRobot.Out.WriteLine("Firing");
            if (

                // ReSharper disable CompareOfFloatsByEqualityOperator
                _advancedRobot.GunTurnRemaining == 0d
                && _advancedRobot.GunHeat == 0d
                && _context != null
                && _context.AimResult != null
                && _context.AimResult.Power != 0d
                && _context.AimResult.Location != null

                // ReSharper restore CompareOfFloatsByEqualityOperator
                )
            {
                double power = Math.Max(Rules.MIN_BULLET_POWER, Math.Min(_context.AimResult.Power, Rules.MAX_BULLET_POWER));
                if (Rules.MIN_BULLET_POWER <= power && power <= Rules.MAX_BULLET_POWER)
                {
                    _advancedRobot.Out.WriteLine("Firing Real Bullet");
                    Bullet bullet = _advancedRobot.SetFireBullet(power);
                    _context.AimResult.Target.MyBullets.Add(bullet);

                    if (SelectedBehavior != null)
                    {
                        SelectedBehavior.OnFireBullet(power);
                    }
                }
            }
        }

        private void TurnRadarTo(Angle newHeading)
        {
            double turnAmount = new Angle(newHeading.Degrees - _advancedRobot.RadarHeading).Degrees;
            if (turnAmount != 0d)
            {
                _advancedRobot.SetTurnRadarRight(new Angle(turnAmount).Degrees180);
            }
        }

        private void Move()
        {
            _advancedRobot.Out.WriteLine("Moving");

            if (_context != null && _context.MyLocation != null && _context.MoveToAbsolute != null)
            {
                Vector moveToRelative = _context.MoveToAbsolute - _context.MyLocation;
                Angle relativeTurnAngle = moveToRelative.Heading - new Angle(_context.MyHeading);

                // it may be quicker to go backwards
                bool forward = -90 <= relativeTurnAngle.Degrees180 && relativeTurnAngle.Degrees180 <= 90;

                double turnRight = forward ? relativeTurnAngle.Degrees180 : relativeTurnAngle.Opposite.Degrees180;
                double moveAhead = forward ? moveToRelative.Magnitude : -moveToRelative.Magnitude;

                if (turnRight != 0d)
                {
                    _advancedRobot.SetTurnRight(turnRight);
                }
                if (moveAhead != 0d)
                {
                    _advancedRobot.SetAhead(moveAhead);
                }
            }
        }

        private void TurnGunToHeading(Angle newHeading)
        {
            double turnAmount = newHeading.Degrees - new Angle(_advancedRobot.GunHeading).Degrees;
            if (turnAmount != 0d)
            {
                _advancedRobot.SetTurnGunRight(new Angle(turnAmount).Degrees180);
            }
        }

        private void Execute()
        {
            _advancedRobot.Out.WriteLine("Execute");
            _advancedRobot.Execute();
        }

        #endregion private methods

        #region Events

        public void OnScannedRobot(ScannedRobotEvent evnt)
        {
            var enemy = _context.OnScannedRobot(evnt);
            if (SelectedBehavior != null && enemy != null)
            {
                SelectedBehavior.OnScannedRobot(enemy);
            }
        }

        public void OnRobotDeath(RobotDeathEvent evnt)
        {
            _context.OnRobotDeath(evnt.Name);
        }

        public void OnBulletHitBullet(BulletHitBulletEvent evnt)
        {
            _context.OnBulletHitBullet(evnt);
        }

        public void OnHitByBullet(HitByBulletEvent evnt)
        {
            // TODO BUG: evnt.Name is returning actual name rather than just a number!
            //Out.WriteLine(string.Format("Hit by: {0}", evnt.Name));
            _context.OnHitByBullet(evnt);
        }

        public void OnSkippedTurn(SkippedTurnEvent evnt)
        {
            _advancedRobot.Out.WriteLine("***OnSkippedTurn***");
        }

        #endregion Events
    }
}