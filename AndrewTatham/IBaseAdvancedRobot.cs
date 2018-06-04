using System.Drawing;
using System.IO;
using Robocode;

namespace AndrewTatham
{
    public interface IBaseAdvancedRobot
    {
        void OnBulletHitBullet(BulletHitBulletEvent evnt);

        void OnHitByBullet(HitByBulletEvent evnt);

        void OnPaint(IGraphics graphics);

        void OnRobotDeath(RobotDeathEvent evnt);

        void OnScannedRobot(ScannedRobotEvent evnt);

        void OnSkippedTurn(SkippedTurnEvent evnt);

        TextWriter Out { get; }

        void Run();

        void Execute();

        bool IsAdjustGunForRobotTurn { get; set; }

        bool IsAdjustRadarForGunTurn { get; set; }

        bool IsAdjustRadarForRobotTurn { get; set; }

        void SetColors(Color color1, Color color2, Color color3);

        void SetAhead(double p);

        void SetTurnRight(double p);

        void SetTurnRadarRight(double p);

        double GunHeading { get; }

        void SetTurnGunRight(double p);

        double GunTurnRemaining { get; }

        double GunHeat { get; }

        Bullet SetFireBullet(double power);

        double RadarHeading { get; }

        double X { get; }

        double Y { get; }

        double BattleFieldHeight { get; }

        double BattleFieldWidth { get; }

        double GunCoolingRate { get; }

        double Energy { get; }

        double Heading { get; }

        int Others { get; }

        long Time { get; }
    }
}