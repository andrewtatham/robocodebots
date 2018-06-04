using System;
using System.IO;
using System.Linq;
using AndrewTatham.Helpers;
using AndrewTatham.Logic.Behaviors.Strategies.Aiming;
using AndrewTatham.Logic.Behaviors.Strategies.Radar;
using AndrewTatham.Logic.Enemies;
using Robocode;

namespace AndrewTatham.Logic
{
    public class Context : IRender, IContext
    {
        private readonly IBaseAdvancedRobot _advancedRobot;

        public Context(IBaseAdvancedRobot bot)
        {
            _advancedRobot = bot;
            Enemies = new Enemies.Enemies();
        }

        public AimResult AimResult { get; set; }

        public double BattlefieldDiag { get; private set; }

        public double BattlefieldHeight { get; private set; }

        public double BattlefieldWidth { get; private set; }

        public Vector CentreAbsolute { get; private set; }

        public Vector CentreRelative
        {
            get { return CentreAbsolute - MyLocation; }
        }

        public Enemies.Enemies Enemies { get; private set; }

        public double GunCoolingRate
        {
            get { return _advancedRobot.GunCoolingRate; }
        }

        public double GunHeat
        {
            get { return _advancedRobot.GunHeat; }
        }

        public Vector MoveToAbsolute { get; set; }

        public double MyEnergy
        {
            get { return _advancedRobot.Energy; }
        }

        public double MyHeading
        {
            get { return _advancedRobot.Heading; }
        }

        public Vector MyLocation
        {
            get
            {
                return new Vector(_advancedRobot.X, _advancedRobot.Y);
            }
        }

        public int OthersCount
        {
            get { return _advancedRobot.Others; }
        }

        public TextWriter Out { get; private set; }

        public RadarResult RadarResult { get; set; }

        public int ScannedCount
        {
            get { return Enemies.Alive.Count(); }
        }

        public IEnemy Target { get; set; }

        public long Time
        {
            get { return _advancedRobot.Time; }
        }

        public void NewTurn()
        {
            Out.WriteLine("Creating New Turn Context");

            MoveToAbsolute = new Vector(_advancedRobot.X, _advancedRobot.Y);

            Enemies.OnTurn(this);
        }

        public IEnemy OnScannedRobot(ScannedRobotEvent evnt)
        {
            if (Enemies != null)
            {
                return Enemies.OnScannedRobot(this, evnt);
            }
            return null;
        }

        public void RunInit()
        {
            Out = _advancedRobot.Out;
            BattlefieldHeight = _advancedRobot.BattleFieldHeight;
            BattlefieldWidth = _advancedRobot.BattleFieldWidth;
            BattlefieldDiag = Math.Sqrt(BattlefieldWidth * BattlefieldWidth + BattlefieldHeight * BattlefieldHeight);
            CentreAbsolute = new Vector(_advancedRobot.BattleFieldWidth / 2d, _advancedRobot.BattleFieldHeight / 2d);

            NewTurn();
        }

        #region IRender Members

        public void Render(IGraphics graphics)
        {
            if (MyLocation != null)
            {
                MyLocation.Render(graphics, ColourPalette.MovementColour);
            }

            if (MyLocation != null && MoveToAbsolute != null)
            {
                new[] { MyLocation, MoveToAbsolute }.Render(graphics, ColourPalette.MovementPen);
            }

            if (MoveToAbsolute != null)
            {
                MoveToAbsolute.Render(graphics, ColourPalette.MovementColour);
            }

            if (Target != null)
            {
                Target.Render(graphics);
            }

            if (MyLocation != null && AimResult != null && AimResult.Location != null)
            {
                new[] { MyLocation, AimResult.Location }.Render(graphics, ColourPalette.AimPen);
            }
        }

        #endregion IRender Members

        public void OnHitByBullet(HitByBulletEvent evnt)
        {
            Enemies.OnHitByBullet(evnt);
        }

        public void OnBulletHitBullet(BulletHitBulletEvent evnt)
        {
            Enemies.OnBulletHitBullet(evnt);
        }

        public void OnRobotDeath(string name)
        {
            Enemies.OnRobotDeath(name);
        }
    }
}